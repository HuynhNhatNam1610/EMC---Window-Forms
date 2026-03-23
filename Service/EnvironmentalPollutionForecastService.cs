using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using static EMC.Service.RatingChartService;

namespace EMC.Service
{
    public class EnvironmentalPollutionForecastService
    {
        private readonly WebClient client = new WebClient();
        private readonly string appid = ConfigurationManager.AppSettings["AppId"];
        private readonly ConcurrentDictionary<string, WeatherInfo> weatherCache = new();
        private static ConcurrentDictionary<string, CachedData> apiCache = new ConcurrentDictionary<string, CachedData>();
        private const string OPENMAP_BASE_URL = "https://mapapis.openmap.vn/v1";
        private readonly string OPENMAP_API_KEY = ConfigurationManager.AppSettings["OpenMap"];
        private const string CAS_BASE_URL = "https://production.cas.so/address-kit";
        private static readonly HttpClient httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
        private static readonly ConcurrentDictionary<string, (DateTime Time, double AQI, string Pollutant)> aqiCache = new();
        private static ConcurrentDictionary<string, (DateTime CacheTime, LocationDetailData Data)> locationCache = new ConcurrentDictionary<string, (DateTime, LocationDetailData)>();
        private const int CACHE_DURATION_MINUTES = 30;
        private static System.Threading.Timer? cacheTimer;
        // Cache dữ liệu cấp tỉnh trong 30 phút
        private static ConcurrentDictionary<string, (DateTime CacheTime, List<CommuneWeatherPoint> Data)> provinceCache
            = new ConcurrentDictionary<string, (DateTime, List<CommuneWeatherPoint>)>();


        public class CommuneInfo
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string FullName { get; set; }
            public string DistrictCode { get; set; }
            public string Type { get; set; }

            public override string ToString()
            {
                return $"{Code} - {FullName} ({Type}) [{DistrictCode}]";
            }
        }

        // 🎯 CLASS MỚI: Gộp tất cả dữ liệu 1 điểm
        public class LocationDetailData
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            // Weather
            public double Temperature { get; set; }
            public int Humidity { get; set; }
            public string WeatherDescription { get; set; }
            public double WindSpeed { get; set; }

            // AQI
            public double AQI { get; set; }
            public string MainPollutant { get; set; }

            // Pollutants Components
            public double PM25 { get; set; }
            public double PM10 { get; set; }
            public double O3 { get; set; }
            public double NO2 { get; set; }
            public double SO2 { get; set; }
            public double CO { get; set; }
            public double NO { get; set; }
            public double NH3 { get; set; }

            // Forecasts
            public List<string> Forecasts { get; set; } = new List<string>();
        }

        private static EnvironmentalPollutionForecastService instance;

        public static EnvironmentalPollutionForecastService Instance
        {
            get { if (instance == null) instance = new EnvironmentalPollutionForecastService(); return instance; }
            private set { instance = value; }
        }

        private EnvironmentalPollutionForecastService()
        {
            if (cacheTimer == null)
            {
                cacheTimer = new System.Threading.Timer(_ =>
                {
                    DisposeProvinceCache();
                }, null, TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(30));
            }
        }

        #region OpenWeatherMap API

        //public LocationDetailData GetCompleteLocationDataCached(double lat, double lon)
        //{
        //    string cacheKey = $"{lat:F4},{lon:F4}";

        //    // Kiểm tra cache
        //    if (locationCache.TryGetValue(cacheKey, out var cached))
        //    {
        //        var timeSinceCache = DateTime.Now - cached.CacheTime;

        //        // Nếu cache còn hợp lệ (< 30 phút)
        //        if (timeSinceCache.TotalMinutes < CACHE_DURATION_MINUTES)
        //        {
        //            Debug.WriteLine($"[CACHE] ✅ Sử dụng cache cho ({lat:F4}, {lon:F4})");
        //            return cached.Data;
        //        }
        //    }

        //    // Nếu không có cache hoặc cache hết hạn → fetch API
        //    Debug.WriteLine($"[CACHE] 🔄 Fetch API cho ({lat:F4}, {lon:F4})");
        //    var data = GetCompleteLocationData(lat, lon);

        //    // Lưu vào cache
        //    locationCache[cacheKey] = (DateTime.Now, data);

        //    return data;
        //}
        public LocationDetailData GetCompleteLocationDataCached(double lat, double lon)
        {
            string cacheKey = $"{lat:F4},{lon:F4}";

            // TryGetValue là thread-safe
            if (locationCache.TryGetValue(cacheKey, out var cached))
            {
                var timeSinceCache = DateTime.Now - cached.CacheTime;

                if (timeSinceCache.TotalMinutes < CACHE_DURATION_MINUTES)
                {
                    Debug.WriteLine($"[CACHE] ✅ Sử dụng cache cho ({lat:F4}, {lon:F4})");
                    return cached.Data;
                }
            }

            Debug.WriteLine($"[CACHE] 🔄 Fetch API cho ({lat:F4}, {lon:F4})");
            var data = GetCompleteLocationData(lat, lon);

            // AddOrUpdate là atomic operation
            locationCache.AddOrUpdate(cacheKey,
                (DateTime.Now, data),
                (key, old) => (DateTime.Now, data));

            return data;
        }

        // 🔄 METHOD TỔNG HỢP: Fetch tất cả dữ liệu 1 lần
        public LocationDetailData GetCompleteLocationData(double lat, double lon)
        {
            var result = new LocationDetailData
            {
                Latitude = lat,
                Longitude = lon
            };

            try
            {
                string latStr = lat.ToString(CultureInfo.InvariantCulture);
                string lonStr = lon.ToString(CultureInfo.InvariantCulture);

                using (var client = new System.Net.WebClient())
                {
                    // WEATHER API (1 call)
                    try
                    {
                        string weatherUrl = $"http://api.openweathermap.org/data/2.5/weather?lat={latStr}&lon={lonStr}&appid={appid}&units=metric&lang=vi";
                        string weatherJson = client.DownloadString(weatherUrl);
                        dynamic weatherResult = JsonConvert.DeserializeObject(weatherJson);

                        result.Temperature = (double)weatherResult.main.temp;
                        result.Humidity = (int)weatherResult.main.humidity;
                        result.WeatherDescription = weatherResult.weather[0].description.ToString();
                        result.WindSpeed = Math.Round((double)weatherResult.wind.speed * 3.6, 1);

                        Debug.WriteLine($"[COMPLETE] ✅ Weather fetched: {result.Temperature}°C, {result.WindSpeed} km/h");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[COMPLETE] ⚠️ Weather fetch failed: {ex.Message}");
                        result.Temperature = 0;
                        result.Humidity = 0;
                        result.WeatherDescription = "Không có dữ liệu";
                        result.WindSpeed = 0;
                    }

                    // AIR POLLUTION API (1 call)
                    try
                    {
                        string airUrl = $"http://api.openweathermap.org/data/2.5/air_pollution?lat={latStr}&lon={lonStr}&appid={appid}";
                        string airJson = client.DownloadString(airUrl);
                        dynamic airResult = JsonConvert.DeserializeObject(airJson);

                        if (airResult?.list != null && airResult.list.Count > 0)
                        {
                            var item = airResult.list[0];
                            var comp = item.components;

                            if (comp != null)
                            {
                                double GetSafe(dynamic c, string name)
                                {
                                    try { return (double)c[name]; } catch { return 0; }
                                }

                                // ✅ Lấy nồng độ các chất
                                result.PM25 = Math.Round(GetSafe(comp, "pm2_5"), 2);
                                result.PM10 = Math.Round(GetSafe(comp, "pm10"), 2);
                                result.O3 = Math.Round(GetSafe(comp, "o3"), 2);
                                result.NO2 = Math.Round(GetSafe(comp, "no2"), 2);
                                result.SO2 = Math.Round(GetSafe(comp, "so2"), 2);
                                result.CO = Math.Round(GetSafe(comp, "co") / 1000.0, 3); // µg/m³ → mg/m³

                                // ✅ Tính AQI theo US EPA
                                var pollutants = new Dictionary<string, double>
                                {
                                    { "PM2.5", result.PM25 },
                                    { "PM10",  result.PM10 },
                                    { "O₃",    result.O3 },
                                    { "NO₂",   result.NO2 },
                                    { "SO₂",   result.SO2 },
                                    { "CO",    result.CO }
                                };

                                double maxAqi = 0;
                                string dominant = "PM2.5";

                                foreach (var kvp in pollutants)
                                {
                                    double subIndex = ComputeSubIndex(kvp.Key, kvp.Value);
                                    if (subIndex > maxAqi)
                                    {
                                        maxAqi = subIndex;
                                        dominant = kvp.Key;
                                    }
                                }

                                result.AQI = Math.Round(maxAqi, 0);
                                result.MainPollutant = dominant;
                            }

                            Debug.WriteLine($"[COMPLETE] ✅ Air pollution (US EPA): AQI={result.AQI}, Main={result.MainPollutant}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[COMPLETE] ⚠️ Air pollution fetch failed: {ex.Message}");
                        result.AQI = 0;
                        result.MainPollutant = "N/A";
                    }

                    // AIR POLLUTION FORECAST (1 call)
                    try
                    {
                        string forecastUrl = $"http://api.openweathermap.org/data/2.5/air_pollution/forecast?lat={latStr}&lon={lonStr}&appid={appid}";
                        string forecastJson = client.DownloadString(forecastUrl);
                        dynamic forecastResult = JsonConvert.DeserializeObject(forecastJson);

                        if (forecastResult?.list != null)
                        {
                            var seenDates = new HashSet<string>();
                            var currentDate = DateTime.Now.Date;

                            foreach (var item in forecastResult.list)
                            {
                                if (result.Forecasts.Count >= 3) break;

                                try
                                {
                                    long dt = (long)item.dt;
                                    var forecastDateTime = DateTimeOffset.FromUnixTimeSeconds(dt).ToLocalTime().DateTime;

                                    if (forecastDateTime.Date <= currentDate)
                                        continue;

                                    string dateKey = forecastDateTime.ToString("dd/MM");
                                    int hour = forecastDateTime.Hour;

                                    if (seenDates.Contains(dateKey) || hour < 9 || hour > 15)
                                        continue;

                                    // ✅ Tính AQI theo chuẩn US EPA
                                    double pm25 = item.components?.pm2_5 ?? 0;
                                    double pm10 = item.components?.pm10 ?? 0;
                                    double o3 = item.components?.o3 ?? 0;
                                    double no2 = item.components?.no2 ?? 0;
                                    double so2 = item.components?.so2 ?? 0;
                                    double co = (item.components?.co ?? 0) / 1000.0; // µg/m³ → mg/m³

                                    var pollutants = new Dictionary<string, double>
                                    {
                                        { "PM2.5", pm25 },
                                        { "PM10",  pm10 },
                                        { "O₃",    o3 },
                                        { "NO₂",   no2 },
                                        { "SO₂",   so2 },
                                        { "CO",    co }
                                    };

                                    double maxAqi = 0;
                                    string dominant = "";

                                    foreach (var kvp in pollutants)
                                    {
                                        double sub = ComputeSubIndex(kvp.Key, kvp.Value);
                                        if (sub > maxAqi)
                                        {
                                            maxAqi = sub;
                                            dominant = kvp.Key;
                                        }
                                    }

                                    string aqiLabel = GetAQILabel((int)maxAqi);
                                    string emoji = GetAQIEmoji((int)maxAqi);

                                    string forecast = $"{emoji} {dateKey}: AQI {maxAqi:F0} ({aqiLabel}) | PM2.5 {pm25:F1}, PM10 {pm10:F1}, O₃ {o3:F1}, NO₂ {no2:F1}, SO₂ {so2:F1}, CO {co:F3}";
                                    result.Forecasts.Add(forecast);
                                    seenDates.Add(dateKey);
                                }
                                catch { continue; }
                            }
                        }

                        while (result.Forecasts.Count < 3)
                            result.Forecasts.Add("📊 Chưa có dữ liệu dự báo");

                        Debug.WriteLine($"[COMPLETE] ✅ Forecasts fetched: {result.Forecasts.Count} items");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[COMPLETE] ⚠️ Forecast fetch failed: {ex.Message}");
                        while (result.Forecasts.Count < 3)
                            result.Forecasts.Add("📊 Lỗi tải dữ liệu dự báo");
                    }

                }

                Debug.WriteLine($"[COMPLETE] ✅ Complete location data for ({lat:F4}, {lon:F4}) ready!");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[COMPLETE] ❌ Error: {ex.Message}");
                return result;
            }
        }

        private string GetAQILabel(int aqi)
        {
            return aqi switch
            {
                >= 1 and <= 50 => "Tốt",
                <= 100 => "Trung bình",
                <= 150 => "Xấu",
                _ => "Nguy hiểm"
            };
        }

        private string GetAQIEmoji(int aqi)
        {
            return aqi switch
            {
                >= 1 and <= 50 => "✅",
                <= 100 => "🙂",
                <= 150 => "😷",
                _ => "☠️"
            };
        }

        public static Color GetAQIColor(double aqi)
        {
            if (aqi <= 50)
                return Color.FromArgb(0, 228, 0);      // 🟢 Green - Tốt
            else if (aqi <= 100)
                return Color.FromArgb(255, 255, 0);    // 🟡 Yellow - Trung bình
            else if (aqi <= 150)
                return Color.FromArgb(255, 126, 0);    // 🟠 Orange - Không tốt
            else if (aqi <= 200)
                return Color.FromArgb(255, 0, 0);      // 🔴 Red - Xấu
            else if (aqi <= 300)
                return Color.FromArgb(153, 0, 76);     // 🟣 Purple - Rất xấu
            else
                return Color.FromArgb(76, 0, 38);      // 🟤 Maroon - Nguy hiểm
        }

        public double GetWindSpeedFromAPI(double lat, double lon)
        {
            try
            {
                using (client)
                {
                    string url = $"http://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={appid}&units=metric";
                    string json = client.DownloadString(url);
                    dynamic result = JsonConvert.DeserializeObject(json);

                    double windSpeedMs = (double)result.wind.speed;
                    double windSpeedKmh = windSpeedMs * 3.6;

                    return Math.Round(windSpeedKmh, 1);
                }
            }
            catch
            {
                return 0;
            }
        }

        public List<string> GetForecastFromAPI(double lat, double lon)
        {
            var forecasts = new List<string>();

            try
            {
                using (client)
                {
                    string latStr = lat.ToString(CultureInfo.InvariantCulture);
                    string lonStr = lon.ToString(CultureInfo.InvariantCulture);

                    string url =
                        $"http://api.openweathermap.org/data/2.5/air_pollution/forecast?lat={latStr}&lon={lonStr}&appid={appid}";

                    string json = client.DownloadString(url);
                    Debug.WriteLine($"✅ [AIR FORECAST] Response received, length: {json.Length} chars");

                    dynamic result = JsonConvert.DeserializeObject(json);

                    if (result.list == null)
                    {
                        Debug.WriteLine("❌ [AIR FORECAST] result.list is NULL!");
                        return FillPlaceholders(forecasts);
                    }

                    Debug.WriteLine($"📊 [AIR FORECAST] Total items in list: {result.list.Count}");

                    var seenDates = new HashSet<string>();
                    var currentDate = DateTime.Now.Date;
                    Debug.WriteLine($"📅 [AIR FORECAST] Current date: {currentDate:yyyy-MM-dd}");

                    int itemIndex = 0;

                    foreach (var item in result.list)
                    {
                        itemIndex++;
                        if (forecasts.Count >= 3)
                        {
                            Debug.WriteLine($"✅ [AIR FORECAST] Got 3 forecasts, stopping at item {itemIndex}");
                            break;
                        }

                        try
                        {
                            long dt = (long)item.dt;
                            var forecastDateTime =
                                DateTimeOffset.FromUnixTimeSeconds(dt).ToLocalTime().DateTime;

                            if (forecastDateTime.Date <= currentDate)
                            {
                                Debug.WriteLine(
                                    $"   ⏭️ SKIPPED: {forecastDateTime:yyyy-MM-dd HH:mm} not after {currentDate:yyyy-MM-dd}");
                                continue;
                            }

                            string dateKey = forecastDateTime.ToString("dd/MM");
                            int hour = forecastDateTime.Hour;

                            Debug.WriteLine(
                                $"   [{itemIndex}] Date: {forecastDateTime:yyyy-MM-dd HH:mm}, Key: {dateKey}, Hour: {hour}");

                            if (seenDates.Contains(dateKey))
                            {
                                Debug.WriteLine($"   ⏭️ SKIPPED: Date {dateKey} already picked");
                                continue;
                            }

                            if (hour < 9 || hour > 15)
                            {
                                Debug.WriteLine($"   ⏭️ SKIPPED: Hour {hour} not in range 9-15");
                                continue;
                            }

                            double pm25 = item.components?.pm2_5 ?? 0;
                            double pm10 = item.components?.pm10 ?? 0;
                            double o3 = item.components?.o3 ?? 0;
                            double no2 = item.components?.no2 ?? 0;
                            double so2 = item.components?.so2 ?? 0;
                            double co = (item.components?.co ?? 0) / 1000.0; // µg/m³ → mg/m³

                            var pollutants = new Dictionary<string, double>
                            {
                                { "PM2.5", pm25 },
                                { "PM10",  pm10 },
                                { "O₃",    o3 },
                                { "NO₂",   no2 },
                                { "SO₂",   so2 },
                                { "CO",    co }
                            };

                            double maxAqi = 0;
                            string dominant = "";

                            foreach (var kvp in pollutants)
                            {
                                double sub = ComputeSubIndex(kvp.Key, kvp.Value);
                                if (sub > maxAqi)
                                {
                                    maxAqi = sub;
                                    dominant = kvp.Key;
                                }
                            }

                            string aqiLabel = GetAQILabel((int)maxAqi);
                            string emoji = GetAQIEmoji((int)maxAqi);
                            string forecast = $"{emoji} {dateKey}: AQI {maxAqi:F0} ({aqiLabel}) | PM2.5 {pm25:F1}, PM10 {pm10:F1}, O₃ {o3:F1}, NO₂ {no2:F1}, SO₂ {so2:F1}, CO {co:F3}";
                            result.Forecasts.Add(forecast);
                            seenDates.Add(dateKey);

                            Debug.WriteLine($"   ✅ ADDED: {forecast}");
                        }
                        catch (Exception itemEx)
                        {
                            Debug.WriteLine(
                                $"   ❌ ERROR parsing item {itemIndex}: {itemEx.Message}");
                            continue;
                        }
                    }

                    Debug.WriteLine($"📋 [AIR FORECAST] Total forecasts collected: {forecasts.Count}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ [AIR FORECAST ERROR] {ex.Message}");
                Debug.WriteLine($"   Stack: {ex.StackTrace}");
            }

            return FillPlaceholders(forecasts);
        }

        private List<string> FillPlaceholders(List<string> forecasts)
        {
            int placeholdersAdded = 0;
            while (forecasts.Count < 3)
            {
                forecasts.Add("📊 Chưa có dữ liệu dự báo chất lượng không khí");
                placeholdersAdded++;
            }

            if (placeholdersAdded > 0)
            {
                Debug.WriteLine($"⚠️ [AIR FORECAST] Added {placeholdersAdded} placeholders");
            }

            Debug.WriteLine("═══════════════════════════════════════════════");
            Debug.WriteLine("🎯 [AIR FORECAST] Final result:");
            for (int i = 0; i < forecasts.Count; i++)
            {
                Debug.WriteLine($"   [{i + 1}] {forecasts[i]}");
            }
            Debug.WriteLine("═══════════════════════════════════════════════\n");

            return forecasts;
        }

        private string GetOwmAQILabel(int aqi)
        {
            return aqi switch
            {
                >= 0 and <= 50 => "Tốt",
                <= 100 => "Trung bình",
                <= 150 => "Không tốt (Nhóm nhạy cảm)",
                <= 200 => "Xấu",
                <= 300 => "Rất xấu",
                _ => "Nguy hiểm"
            };
        }

        private string GetOwnAQIEmoji(int aqi)
        {
            return aqi switch
            {
                >= 0 and <= 50 => "✅",      // Tốt
                <= 100 => "🙂",              // Trung bình
                <= 150 => "😐",              // Không tốt (nhóm nhạy cảm)
                <= 200 => "😷",              // Xấu
                <= 300 => "😨",              // Rất xấu
                _ => "☠️"                     // Nguy hiểm
            };
        }

        //public WeatherInfo GetCachedWeather(string cityName, double lat, double lon, bool forceRefresh = false)
        //{
        //    string key = $"{lat:F4},{lon:F4}";

        //    try
        //    {
        //        if (forceRefresh || !weatherCache.ContainsKey(key))
        //        {
        //            Debug.WriteLine($"[CACHE] Fetch new weather for {cityName} ({lat}, {lon})");
        //            var weather = FetchRealTimeWeather(lat, lon);

        //            if (weather != null)
        //            {
        //                weatherCache[key] = weather;
        //                Debug.WriteLine($"[CACHE] Added new weather for key={key}: {weather.Description}, {weather.Temperature}°C, {weather.Humidity}%");
        //            }
        //            else
        //            {
        //                Debug.WriteLine($"[CACHE][WARN] FetchRealTimeWeather returned null for key={key}");
        //                weatherCache[key] = new WeatherInfo
        //                {
        //                    Description = "Không có dữ liệu",
        //                    Temperature = 0,
        //                    Humidity = 0,
        //                    WindSpeed = 0
        //                };
        //            }
        //        }
        //        else
        //        {
        //            var cached = weatherCache[key];
        //        }

        //        return weatherCache[key];
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"[CACHE][ERROR] Exception for {key}: {ex.Message}");
        //        return new WeatherInfo
        //        {
        //            Description = "Lỗi tải dữ liệu",
        //            Temperature = 0,
        //            Humidity = 0,
        //            WindSpeed = 0
        //        };
        //    }
        //}
        public WeatherInfo GetCachedWeather(string cityName, double lat, double lon, bool forceRefresh = false)
        {
            string key = $"{lat:F4},{lon:F4}";

            try
            {
                if (forceRefresh)
                {
                    Debug.WriteLine($"[CACHE] Force refresh weather for {cityName} ({lat}, {lon})");
                    var weather = FetchRealTimeWeather(lat, lon);

                    if (weather != null)
                    {
                        weatherCache[key] = weather; // ConcurrentDictionary indexer là thread-safe
                        Debug.WriteLine($"[CACHE] Updated weather for key={key}");
                    }
                    else
                    {
                        weatherCache[key] = new WeatherInfo
                        {
                            Description = "Không có dữ liệu",
                            Temperature = 0,
                            Humidity = 0,
                            WindSpeed = 0
                        };
                    }
                    return weatherCache[key];
                }

                // GetOrAdd là atomic operation - thread-safe
                return weatherCache.GetOrAdd(key, k =>
                {
                    Debug.WriteLine($"[CACHE] Fetch new weather for {cityName} ({lat}, {lon})");
                    var weather = FetchRealTimeWeather(lat, lon);

                    if (weather != null)
                    {
                        Debug.WriteLine($"[CACHE] Added new weather: {weather.Description}, {weather.Temperature}°C");
                        return weather;
                    }
                    else
                    {
                        return new WeatherInfo
                        {
                            Description = "Không có dữ liệu",
                            Temperature = 0,
                            Humidity = 0,
                            WindSpeed = 0
                        };
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CACHE][ERROR] Exception for {key}: {ex.Message}");
                return new WeatherInfo
                {
                    Description = "Lỗi tải dữ liệu",
                    Temperature = 0,
                    Humidity = 0,
                    WindSpeed = 0
                };
            }
        }

        // 🔹 Hàm chung tính AQI theo công thức US EPA
        private static double CalculateAQI(double C, (double Clow, double Chigh, int Ilow, int Ihigh)[] bp)
        {
            foreach (var (Clow, Chigh, Ilow, Ihigh) in bp)
            {
                if (C >= Clow && C <= Chigh)
                {
                    return (Ihigh - Ilow) / (Chigh - Clow) * (C - Clow) + Ilow;
                }
            }
            return -1;
        }

        // 🔹 Các bảng giới hạn tiêu chuẩn US EPA
        private static double AQI_PM25(double c) => CalculateAQI(c, new[]
        {
            (0.0, 12.0, 0, 50),
            (12.1, 35.4, 51, 100),
            (35.5, 55.4, 101, 150),
            (55.5, 150.4, 151, 200),
            (150.5, 250.4, 201, 300),
            (250.5, 500.4, 301, 500)
        });

        private static double AQI_PM10(double c) => CalculateAQI(c, new[]
        {
            (0.0, 54.0, 0, 50),
            (55.0, 154.0, 51, 100),
            (155.0, 254.0, 101, 150),
            (255.0, 354.0, 151, 200),
            (355.0, 424.0, 201, 300),
            (425.0, 604.0, 301, 500)
        });

        private static double AQI_SO2(double c) => CalculateAQI(c, new[]
        {
            (0.0, 35.0, 0, 50),
            (36.0, 75.0, 51, 100),
            (76.0, 185.0, 101, 150),
            (186.0, 304.0, 151, 200),
            (305.0, 604.0, 201, 300),
            (605.0, 804.0, 301, 400),
            (805.0, 1004.0, 401, 500)
        });

        private static double AQI_NO2(double c) => CalculateAQI(c, new[]
        {
            (0.0, 53.0, 0, 50),
            (54.0, 100.0, 51, 100),
            (101.0, 360.0, 101, 150),
            (361.0, 649.0, 151, 200),
            (650.0, 1249.0, 201, 300),
            (1250.0, 1649.0, 301, 400),
            (1650.0, 2049.0, 401, 500)
        });

        private static double AQI_CO(double c) => CalculateAQI(c, new[]
        {
            (0.0, 4.4, 0, 50),
            (4.5, 9.4, 51, 100),
            (9.5, 12.4, 101, 150),
            (12.5, 15.4, 151, 200),
            (15.5, 30.4, 201, 300),
            (30.5, 40.4, 301, 400),
            (40.5, 50.4, 401, 500)
        });

        private static double AQI_O3(double c) => CalculateAQI(c, new[]
        {
            (0.000, 0.054, 0, 50),
            (0.055, 0.070, 51, 100),
            (0.071, 0.085, 101, 150),
            (0.086, 0.105, 151, 200),
            (0.106, 0.200, 201, 300)
        });

        public (double aqi, string pollutant) FetchRealTimeAQI_AndPollutant(double lat, double lon)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string url = $"http://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={appid}";
                    string json = client.DownloadString(url);
                    Debug.WriteLine($"[AQI_DEBUG] {lat},{lon} → {json}");
                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                    var comp = result.list[0].components;
                    var pollutants = new Dictionary<string, double>
                    {
                        { "PM2.5", (double)comp.pm2_5 },
                        { "PM10",  (double)comp.pm10 },
                        { "NO2",   (double)comp.no2 },
                        { "SO2",   (double)comp.so2 },
                        { "O3",    (double)comp.o3 },
                        { "CO",    (double)comp.co / 1000.0 } // μg/m³ → mg/m³
                    };

                    double maxAQI = 0;
                    string dominant = "PM2.5";

                    foreach (var kvp in pollutants)
                    {
                        double subIndex = ComputeSubIndex(kvp.Key, kvp.Value);
                        if (subIndex > maxAQI)
                        {
                            maxAQI = subIndex;
                            dominant = kvp.Key;
                        }
                    }

                    return (Math.Round(maxAQI, 0), dominant);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AQI] Lỗi: {ex.Message}");
                return (0, "N/A");
            }
        }

        private double ComputeSubIndex(string pollutant, double conc)
        {
            // Dựa theo bảng chuẩn US EPA
            (double Clow, double Chigh, int Ilow, int Ihigh) range;

            switch (pollutant.ToUpper())
            {
                case "PM2.5":
                    if (conc <= 12) range = (0, 12, 0, 50);
                    else if (conc <= 35.4) range = (12.1, 35.4, 51, 100);
                    else if (conc <= 55.4) range = (35.5, 55.4, 101, 150);
                    else if (conc <= 150.4) range = (55.5, 150.4, 151, 200);
                    else if (conc <= 250.4) range = (150.5, 250.4, 201, 300);
                    else if (conc <= 500.4) range = (250.5, 500.4, 301, 500);
                    else return 500;
                    break;

                case "PM10":
                    if (conc <= 54) range = (0, 54, 0, 50);
                    else if (conc <= 154) range = (55, 154, 51, 100);
                    else if (conc <= 254) range = (155, 254, 101, 150);
                    else if (conc <= 354) range = (255, 354, 151, 200);
                    else if (conc <= 424) range = (355, 424, 201, 300);
                    else if (conc <= 604) range = (425, 604, 301, 500);
                    else return 500;
                    break;

                case "NO2":
                    if (conc <= 53) range = (0, 53, 0, 50);
                    else if (conc <= 100) range = (54, 100, 51, 100);
                    else if (conc <= 360) range = (101, 360, 101, 150);
                    else if (conc <= 649) range = (361, 649, 151, 200);
                    else if (conc <= 1249) range = (650, 1249, 201, 300);
                    else if (conc <= 2049) range = (1250, 2049, 301, 500);
                    else return 500;
                    break;

                case "SO2":
                    if (conc <= 35) range = (0, 35, 0, 50);
                    else if (conc <= 75) range = (36, 75, 51, 100);
                    else if (conc <= 185) range = (76, 185, 101, 150);
                    else if (conc <= 304) range = (186, 304, 151, 200);
                    else if (conc <= 604) range = (305, 604, 201, 300);
                    else if (conc <= 1004) range = (605, 1004, 301, 500);
                    else return 500;
                    break;

                case "O3":
                    if (conc <= 54) range = (0, 54, 0, 50);
                    else if (conc <= 70) range = (55, 70, 51, 100);
                    else if (conc <= 85) range = (71, 85, 101, 150);
                    else if (conc <= 105) range = (86, 105, 151, 200);
                    else if (conc <= 200) range = (106, 200, 201, 300);
                    else return 500;
                    break;

                case "CO":
                    if (conc <= 4.4) range = (0, 4.4, 0, 50);
                    else if (conc <= 9.4) range = (4.5, 9.4, 51, 100);
                    else if (conc <= 12.4) range = (9.5, 12.4, 101, 150);
                    else if (conc <= 15.4) range = (12.5, 15.4, 151, 200);
                    else if (conc <= 30.4) range = (15.5, 30.4, 201, 300);
                    else if (conc <= 50.4) range = (30.5, 50.4, 301, 500);
                    else return 500;
                    break;

                default:
                    return 0;
            }

            return ((range.Ihigh - range.Ilow) / (range.Chigh - range.Clow)) * (conc - range.Clow) + range.Ilow;
        }



        public WeatherInfo FetchRealTimeWeather(double lat, double lon)
        {
            Debug.WriteLine($"[DEBUG] FetchRealTimeWeather START: lat={lat}, lon={lon}");

            if (string.IsNullOrEmpty(appid))
            {
                Debug.WriteLine("[DEBUG] Missing API key, using dummy weather data.");
                return new WeatherInfo
                {
                    Temperature = 28.5,
                    Humidity = 75,
                    Description = "Có mây",
                    WindSpeed = 12.5,
                    Forecast = "..."
                };
            }

            try
            {
                using (var client = new System.Net.WebClient())
                {
                    string url = $"http://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={appid}&units=metric&lang=vi";
                    Debug.WriteLine($"[DEBUG] Fetching weather from URL: {url}");

                    string json = client.DownloadString(url);
                    Debug.WriteLine($"[DEBUG] Weather API Response: {json.Substring(0, Math.Min(json.Length, 150))}...");
                    dynamic result = JsonConvert.DeserializeObject(json);

                    var weather = new WeatherInfo
                    {
                        Temperature = (double)result.main.temp,
                        Humidity = (int)result.main.humidity,
                        Description = result.weather[0].description.ToString(),
                        WindSpeed = (double)result.wind.speed * 3.6,
                        Forecast = GetWeatherForecast(lat, lon)
                    };

                    Debug.WriteLine($"[DEBUG] Parsed weather: {weather.Description}, {weather.Temperature}°C, {weather.Humidity}%");
                    return weather;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] FetchRealTimeWeather failed: {ex.Message}");
                return new WeatherInfo { Description = "Không có dữ liệu" };
            }
        }

        public string GetWeatherForecast(double lat, double lon)
        {
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    string url = $"http://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={appid}&units=metric&lang=vi";
                    string json = client.DownloadString(url);
                    dynamic result = JsonConvert.DeserializeObject(json);

                    var forecasts = new List<string>();
                    var seenDates = new HashSet<string>();
                    var currentDate = DateTime.Now.Date;

                    foreach (var item in result.list)
                    {
                        DateTime forecastDate = DateTime.Parse(item.dt_txt.ToString());
                        string dateKey = forecastDate.ToString("dd/MM");

                        if (forecastDate.Date > currentDate && !seenDates.Contains(dateKey))
                        {
                            seenDates.Add(dateKey);
                            string desc = item.weather[0].description.ToString();
                            double temp = (double)item.main.temp;
                            forecasts.Add($"{dateKey}: {desc} ({temp:F1}°C)");

                            if (forecasts.Count >= 3) break;
                        }
                    }

                    return forecasts.Count > 0 ? string.Join(" | ", forecasts) : "Dự báo không khả dụng";
                }
            }
            catch
            {
                return "Dự báo không khả dụng";
            }
        }

        public async Task<Dictionary<string, ProvinceAirQualityData>> GetProvincesPollutionDataPipeline_Async(string effectiveDate = "2025-07-01")
        {
            var result = new Dictionary<string, ProvinceAirQualityData>();

            Debug.WriteLine($"\n[PIPELINE] ========== BƯỚC 1: Fetch Danh Sách Tỉnh Thành từ CAS ==========");
            var provinces = await FetchProvincesFromCAS_Async(effectiveDate);

            if (provinces == null || provinces.Count == 0)
            {
                Debug.WriteLine("[PIPELINE] ❌ Không lấy được danh sách tỉnh từ CAS");
                return result;
            }

            Debug.WriteLine($"[PIPELINE] ✅ Lấy được {provinces.Count} tỉnh thành");

            foreach (var province in provinces)
            {
                string provinceName = province.Key;
                var provinceInfo = province.Value;

                Debug.WriteLine($"\n[PIPELINE] --- Xử lý: {provinceName} ---");

                try
                {
                    Debug.WriteLine($"[PIPELINE] 🔍 BƯỚC 2: Geocode '{provinceName}'...");
                    var geocodeResult = await GeocodeProvince_Async(provinceName, useOsmFormat: false, adminV2: true);

                    if (geocodeResult == null || geocodeResult.Latitude == 0)
                    {
                        Debug.WriteLine($"[PIPELINE] ⚠️ Không geocode được {provinceName}, bỏ qua");
                        continue;
                    }

                    double lat = geocodeResult.Latitude;
                    double lon = geocodeResult.Longitude;
                    string address = geocodeResult.Address;

                    Debug.WriteLine($"[PIPELINE] ✅ Geocode thành công: {provinceName} -> lat={lat:F6}, lon={lon:F6}");

                    Debug.WriteLine($"[PIPELINE] 🌍 BƯỚC 3: Fetch AQI cho {provinceName}...");
                    var (aqi, pollutant) = FetchRealTimeAQI_AndPollutant(lat, lon);

                    Debug.WriteLine($"[PIPELINE] ✅ AQI lấy được: {aqi:F1} µg/m³, Chất ô nhiễm: {pollutant}");

                    var weather = GetCachedWeather(provinceName, lat, lon);

                    result[provinceName] = new ProvinceAirQualityData
                    {
                        ProvinceName = provinceName,
                        Code = provinceInfo.Code,
                        Latitude = lat,
                        Longitude = lon,
                        Address = address,
                        AQI = aqi,
                        MainPollutant = pollutant,
                        Temperature = weather.Temperature,
                        Humidity = weather.Humidity,
                        Description = weather.Description,
                        WindSpeed = weather.WindSpeed,
                        Forecast = weather.Forecast,
                        FetchedTime = DateTime.Now
                    };

                    Debug.WriteLine($"[PIPELINE] 💾 Lưu dữ liệu cho {provinceName}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[PIPELINE] ❌ Lỗi xử lý {provinceName}: {ex.Message}");
                }
            }

            Debug.WriteLine($"\n[PIPELINE] ========== HOÀN THÀNH ==========");
            Debug.WriteLine($"[PIPELINE] ✅ Xử lý thành công {result.Count}/{provinces.Count} tỉnh thành");

            return result;
        }

        #endregion

        #region API CAS
        public Dictionary<string, ProvinceData> GetRealTimeProvinceData()
        {
            var data = new Dictionary<string, ProvinceData>();
            var provinces = RatingChartService.Instance.Get34ProvincesInfo();

            foreach (var province in provinces)
            {
                try
                {
                    var (aqi, pollutant) = FetchRealTimeAQI_AndPollutant(province.Value.Latitude, province.Value.Longitude);
                    var weather = GetCachedWeather(province.Key, province.Value.Latitude, province.Value.Longitude);

                    data[province.Key] = new ProvinceData(
                        province.Value.Latitude,
                        province.Value.Longitude,
                        aqi,
                        pollutant,
                        new List<PollutionPoint>(),
                        weather,
                        province.Value.IndustrialZones,
                        province.Value.MergedFrom
                    );
                }
                catch
                {
                    data[province.Key] = province.Value;
                }
            }

            return data;
        }

        public async Task<Dictionary<string, ProvinceInfo>> FetchProvincesFromCAS_Async(string effectiveDate = "2025-07-01")
        {
            var result = new Dictionary<string, ProvinceInfo>();
            string url = $"{CAS_BASE_URL}/{effectiveDate}/provinces";

            Debug.WriteLine($"[CAS] 🔄 Fetching provinces from: {url}");

            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                dynamic parsed = JsonConvert.DeserializeObject(json);

                Debug.WriteLine($"[CAS] ✅ RequestId: {parsed.requestId}");

                foreach (var province in parsed.provinces)
                {
                    string code = (string)province.code;
                    string name = (string)province.name;
                    string decree = (string)province.decree ?? "";

                    result[name] = new ProvinceInfo
                    {
                        Code = code,
                        Name = name,
                        Decree = decree
                    };

                    Debug.WriteLine($"[CAS]   {code} - {name} {(string.IsNullOrEmpty(decree) ? "" : $"({decree})")}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CAS] ❌ Failed to fetch provinces: {ex.Message}");
                return null;
            }
        }

        public async Task<List<CommuneInfo>> FetchCommunesOfProvinceFromCAS_Async(
            string provinceCode,
            string effectiveDate = "2025-07-01")
        {
            var communes = new List<CommuneInfo>();

            if (string.IsNullOrWhiteSpace(provinceCode))
            {
                Debug.WriteLine("[CAS] ⚠️ provinceCode trống.");
                return communes;
            }

            string url = $"{CAS_BASE_URL}/{effectiveDate}/provinces/{provinceCode}/communes";
            Debug.WriteLine($"[CAS] 🔄 Fetching communes for province {provinceCode} from: {url}");

            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                dynamic parsed = JsonConvert.DeserializeObject(json);

                var arr = parsed.communes ?? parsed.data ?? parsed.wards;
                if (arr == null)
                {
                    Debug.WriteLine($"[CAS] ⚠️ Response communes rỗng hoặc sai format cho {provinceCode}");
                    return communes;
                }

                foreach (var c in arr)
                {
                    var ci = new CommuneInfo
                    {
                        Code = (string)(c.code ?? ""),
                        Name = (string)(c.name ?? ""),
                        FullName = (string)(c.fullName ?? c.fullname ?? c.name ?? ""),
                        DistrictCode = (string)(c.districtCode ?? c.district_code ?? ""),
                        Type = (string)(c.type ?? "")
                    };

                    communes.Add(ci);
                }

                Debug.WriteLine($"[CAS] ✅ {provinceCode}: {communes.Count} xã/phường.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CAS] ❌ Lỗi fetch communes cho {provinceCode}: {ex.Message}");
            }

            return communes;
        }

        public async Task<string> GetProvinceCodeByNameAsync(string effectiveDate, string provinceName)
        {
            if (string.IsNullOrWhiteSpace(provinceName))
                return null;

            var provinces = await FetchProvincesFromCAS_Async(effectiveDate);
            if (provinces == null || provinces.Count == 0)
                return null;

            if (provinces.TryGetValue(provinceName, out var info))
                return info.Code;

            string Normalize(string s)
            {
                if (string.IsNullOrWhiteSpace(s)) return string.Empty;

                s = s.Trim().ToLowerInvariant();

                s = s
                    .Replace("tỉnh ", "")
                    .Replace("thành phố ", "")
                    .Replace("thanh pho ", "")
                    .Replace("tp. ", "")
                    .Replace("tp ", "");

                while (s.Contains("  "))
                    s = s.Replace("  ", " ");

                return s;
            }

            var target = Normalize(provinceName);

            foreach (var kv in provinces)
            {
                if (Normalize(kv.Key) == target)
                    return kv.Value.Code;
            }

            return null;
        }

        #endregion

        #region API Open Map
        public async Task<GeocodeResult> GeocodeProvince_Async(string provinceName, bool useOsmFormat = false, bool adminV2 = false)
        {
            if (string.IsNullOrWhiteSpace(provinceName))
            {
                Debug.WriteLine("[OPENMAP] ⚠️ Tên tỉnh truyền vào bị trống");
                return null;
            }

            try
            {
                string queryParamName = useOsmFormat ? "text" : "address";
                string encodedAddress = Uri.EscapeDataString(provinceName);

                string url = $"{OPENMAP_BASE_URL}/geocode/forward" +
                    $"?{queryParamName}={encodedAddress}" +
                    $"&apikey={OPENMAP_API_KEY}";

                if (adminV2)
                    url += "&admin_v2=true";

                Debug.WriteLine($"[OPENMAP] 👉 Geocoding: {url}");

                var response = await httpClient.GetAsync(url);
                Debug.WriteLine($"[OPENMAP] HTTP {(int)response.StatusCode} {response.ReasonPhrase}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(json);

                if (!useOsmFormat && obj["results"] != null)
                {
                    var results = obj["results"];
                    if (!results.Any())
                    {
                        Debug.WriteLine($"[OPENMAP] ⚠️ Không tìm thấy {provinceName} (results trống)");
                        return null;
                    }

                    var firstResult = results.First();
                    var formatted = (string)firstResult["formatted_address"] ?? (string)firstResult["address"];
                    var lat = (double?)firstResult["geometry"]?["location"]?["lat"];
                    var lng = (double?)firstResult["geometry"]?["location"]?["lng"];

                    Debug.WriteLine($"[OPENMAP] ✅ Tìm thấy: {formatted} | lat={lat:F6}, lng={lng:F6}");

                    return new GeocodeResult
                    {
                        Address = formatted,
                        Latitude = lat ?? 0,
                        Longitude = lng ?? 0
                    };
                }
                else if (useOsmFormat && obj["features"] != null)
                {
                    var features = obj["features"];
                    if (!features.Any())
                    {
                        Debug.WriteLine($"[OPENMAP] ⚠️ Không tìm thấy {provinceName} (features trống)");
                        return null;
                    }

                    var firstFeature = features.First();
                    var props = firstFeature["properties"];
                    var label = (string)props?["label"] ?? (string)props?["name"];
                    var coords = firstFeature["geometry"]?["coordinates"];
                    double? lng = coords?.Count() >= 1 ? (double?)coords[0] : null;
                    double? lat = coords?.Count() >= 2 ? (double?)coords[1] : null;

                    Debug.WriteLine($"[OPENMAP] ✅ Tìm thấy: {label} | lat={lat:F6}, lng={lng:F6}");

                    return new GeocodeResult
                    {
                        Address = label,
                        Latitude = lat ?? 0,
                        Longitude = lng ?? 0
                    };
                }
                else
                {
                    Debug.WriteLine($"[OPENMAP] ⚠️ Response không đúng cấu trúc");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[OPENMAP] ❌ Geocode error: {ex.Message}");
                return null;
            }
        }
        public async Task<GeocodeResult> GeocodeAddressAsync(string address, bool useOsmFormat = false, bool adminV2 = false)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                Debug.WriteLine("[OPENMAP] ⚠️ Địa chỉ trống.");
                return null;
            }

            int maxRetries = 3;
            int retryCount = 0;
            int delayMs = 1000; // Bắt đầu với 1 giây

            while (retryCount < maxRetries)
            {
                try
                {
                    string queryParamName = useOsmFormat ? "text" : "address";
                    string encodedAddress = Uri.EscapeDataString(address);

                    string url = $"{OPENMAP_BASE_URL}/geocode/forward" +
                                 $"?{queryParamName}={encodedAddress}" +
                                 $"&apikey={OPENMAP_API_KEY}";

                    if (adminV2)
                        url += "&admin_v2=true";

                    Debug.WriteLine($"[OPENMAP] 👉 GeocodeAddressAsync: {url}");

                    var response = await httpClient.GetAsync(url);
                    Debug.WriteLine($"[OPENMAP] HTTP {(int)response.StatusCode} {response.ReasonPhrase}");

                    // ✅ Xử lý 429 - Too Many Requests
                    if ((int)response.StatusCode == 429)
                    {
                        retryCount++;
                        if (retryCount < maxRetries)
                        {
                            Debug.WriteLine($"[OPENMAP] ⏳ Rate limited! Đợi {delayMs}ms trước khi thử lại ({retryCount}/{maxRetries})");
                            await Task.Delay(delayMs);
                            delayMs *= 2; // Exponential backoff: 1s → 2s → 4s
                            continue;
                        }
                        else
                        {
                            Debug.WriteLine($"[OPENMAP] ❌ Rate limited quá nhiều lần, bỏ qua {address}");
                            return null;
                        }
                    }

                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JObject.Parse(json);

                    if (!useOsmFormat && obj["results"] != null)
                    {
                        var results = obj["results"];
                        if (!results.Any())
                        {
                            Debug.WriteLine("[OPENMAP] ⚠️ Không tìm thấy kết quả (results trống).");
                            return null;
                        }

                        var first = results.First();
                        var formatted = (string)first["formatted_address"] ?? (string)first["address"];
                        var lat = (double?)first["geometry"]?["location"]?["lat"];
                        var lng = (double?)first["geometry"]?["location"]?["lng"];

                        if (lat == null || lng == null)
                        {
                            Debug.WriteLine("[OPENMAP] ⚠️ Thiếu toạ độ.");
                            return null;
                        }

                        return new GeocodeResult
                        {
                            Address = formatted,
                            Latitude = lat.Value,
                            Longitude = lng.Value
                        };
                    }

                    if (useOsmFormat && obj["features"] != null)
                    {
                        var features = obj["features"];
                        if (!features.Any())
                        {
                            Debug.WriteLine("[OPENMAP] ⚠️ Không tìm thấy kết quả (features trống).");
                            return null;
                        }

                        var f = features.First();
                        var props = f["properties"];
                        var label = (string)props?["label"] ?? (string)props?["name"];
                        var coords = f["geometry"]?["coordinates"];
                        if (coords == null || coords.Count() < 2)
                        {
                            Debug.WriteLine("[OPENMAP] ⚠️ Thiếu toạ độ trong features.");
                            return null;
                        }

                        double lng = (double)coords[0];
                        double lat = (double)coords[1];

                        return new GeocodeResult
                        {
                            Address = label,
                            Latitude = lat,
                            Longitude = lng
                        };
                    }

                    Debug.WriteLine("[OPENMAP] ⚠️ Response không đúng format.");
                    return null;
                }
                catch (HttpRequestException ex) when (ex.Message.Contains("429"))
                {
                    retryCount++;
                    if (retryCount < maxRetries)
                    {
                        Debug.WriteLine($"[OPENMAP] ⏳ Rate limited! Đợi {delayMs}ms trước khi thử lại ({retryCount}/{maxRetries})");
                        await Task.Delay(delayMs);
                        delayMs *= 2;
                    }
                    else
                    {
                        Debug.WriteLine($"[OPENMAP] ❌ Exceeded max retries for {address}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[OPENMAP] ❌ GeocodeAddressAsync error: {ex.Message}");
                    return null;
                }
            }

            return null;
        }
        //public async Task<List<CommuneWeatherPoint>> GetCommuneWeatherPointsByProvinceAsync(
        //    string effectiveDate,
        //    string provinceCode,
        //    string provinceName)
        //{
        //    string cacheKey = provinceCode;

        //    // ✅ Kiểm tra cache
        //    if (provinceCache.TryGetValue(cacheKey, out var cached)
        //        && (DateTime.Now - cached.CacheTime).TotalMinutes < CACHE_DURATION_MINUTES)
        //    {
        //        Debug.WriteLine($"[CACHE] ✅ Dùng cache cho tỉnh {provinceName} ({provinceCode})");
        //        return cached.Data;
        //    }

        //    Debug.WriteLine($"[CACHE] 🔄 Fetch API cho tỉnh {provinceName} ({provinceCode})");
        //    var result = new List<CommuneWeatherPoint>();

        //    var communes = await FetchCommunesOfProvinceFromCAS_Async(provinceCode, effectiveDate);
        //    if (communes == null || communes.Count == 0)
        //    {
        //        Debug.WriteLine($"[COMMUNE] ❌ Không có xã/phường cho {provinceName} ({provinceCode}).");
        //        return result;
        //    }

        //    Debug.WriteLine($"[COMMUNE] 🔄 Tạo marker cho {communes.Count} xã/phường của {provinceName}.");
        //    var semaphore = new System.Threading.SemaphoreSlim(3);

        //    var tasks = communes.Select(async cm =>
        //    {
        //        string rawAddress = $"{cm.Name}, {provinceName}";
        //        try
        //        {
        //            await semaphore.WaitAsync();
        //            var geo = await GeocodeAddressAsync(rawAddress, useOsmFormat: false, adminV2: true);
        //            if (geo == null || geo.Latitude == 0 || geo.Longitude == 0)
        //                return;

        //            double lat = geo.Latitude;
        //            double lon = geo.Longitude;
        //            var weather = GetCachedWeather(rawAddress, lat, lon);
        //            var (aqi, pollutant) = FetchRealTimeAQI_AndPollutant(lat, lon);

        //            lock (result)
        //            {
        //                result.Add(new CommuneWeatherPoint
        //                {
        //                    ProvinceCode = provinceCode,
        //                    ProvinceName = provinceName,
        //                    CommuneCode = cm.Code,
        //                    CommuneName = cm.Name,
        //                    Address = geo.Address ?? rawAddress,
        //                    Lat = lat,
        //                    Lon = lon,
        //                    Temperature = weather.Temperature,
        //                    Humidity = weather.Humidity,
        //                    Description = weather.Description,
        //                    AQI = aqi,
        //                    MainPollutant = pollutant
        //                });
        //            }
        //        }
        //        finally
        //        {
        //            semaphore.Release();
        //        }
        //    }).ToList();

        //    await Task.WhenAll(tasks);

        //    // ✅ Lưu cache
        //    provinceCache[cacheKey] = (DateTime.Now, result);

        //    Debug.WriteLine($"[CACHE] ✅ Lưu cache cho {provinceName}, tổng {result.Count} xã/phường.");
        //    return result;
        //}
        public async Task<List<CommuneWeatherPoint>> GetCommuneWeatherPointsByProvinceAsync(
            string effectiveDate,
            string provinceCode,
            string provinceName)
        {
            string cacheKey = provinceCode;

            if (provinceCache.TryGetValue(cacheKey, out var cached)
                && (DateTime.Now - cached.CacheTime).TotalMinutes < CACHE_DURATION_MINUTES)
            {
                Debug.WriteLine($"[CACHE] ✅ Dùng cache cho tỉnh {provinceName} ({provinceCode})");
                return cached.Data;
            }

            Debug.WriteLine($"[CACHE] 🔄 Fetch API cho tỉnh {provinceName} ({provinceCode})");
            var result = new ConcurrentBag<CommuneWeatherPoint>(); // ✅ Thay List bằng ConcurrentBag

            var communes = await FetchCommunesOfProvinceFromCAS_Async(provinceCode, effectiveDate);
            if (communes == null || communes.Count == 0)
            {
                Debug.WriteLine($"[COMMUNE] ❌ Không có xã/phường cho {provinceName} ({provinceCode}).");
                return new List<CommuneWeatherPoint>();
            }

            Debug.WriteLine($"[COMMUNE] 🔄 Tạo marker cho {communes.Count} xã/phường của {provinceName}.");
            var semaphore = new System.Threading.SemaphoreSlim(3);

            var tasks = communes.Select(async cm =>
            {
                string rawAddress = $"{cm.Name}, {provinceName}";
                try
                {
                    await semaphore.WaitAsync();
                    var geo = await GeocodeAddressAsync(rawAddress, useOsmFormat: false, adminV2: true);
                    if (geo == null || geo.Latitude == 0 || geo.Longitude == 0)
                        return;

                    double lat = geo.Latitude;
                    double lon = geo.Longitude;
                    var weather = GetCachedWeather(rawAddress, lat, lon);
                    var (aqi, pollutant) = FetchRealTimeAQI_AndPollutant(lat, lon);

                    // ✅ ConcurrentBag.Add là thread-safe
                    result.Add(new CommuneWeatherPoint
                    {
                        ProvinceCode = provinceCode,
                        ProvinceName = provinceName,
                        CommuneCode = cm.Code,
                        CommuneName = cm.Name,
                        Address = geo.Address ?? rawAddress,
                        Lat = lat,
                        Lon = lon,
                        Temperature = weather.Temperature,
                        Humidity = weather.Humidity,
                        Description = weather.Description,
                        AQI = aqi,
                        MainPollutant = pollutant
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToList();

            await Task.WhenAll(tasks);

            var resultList = result.ToList(); // Chuyển về List

            // ✅ AddOrUpdate là thread-safe
            provinceCache.AddOrUpdate(cacheKey,
                (DateTime.Now, resultList),
                (key, old) => (DateTime.Now, resultList));

            Debug.WriteLine($"[CACHE] ✅ Lưu cache cho {provinceName}, tổng {resultList.Count} xã/phường.");
            return resultList;
        }

        //public static void DisposeProvinceCache()
        //{
        //    var expired = provinceCache
        //        .Where(x => (DateTime.Now - x.Value.CacheTime).TotalMinutes >= CACHE_DURATION_MINUTES)
        //        .Select(x => x.Key)
        //        .ToList();

        //    foreach (var key in expired)
        //        provinceCache.Remove(key);

        //    Debug.WriteLine($"[CACHE] 🧹 Dọn {expired.Count} tỉnh hết hạn khỏi cache.");
        //}
        public static void DisposeProvinceCache()
        {
            var expiredKeys = provinceCache
                .Where(x => (DateTime.Now - x.Value.CacheTime).TotalMinutes >= CACHE_DURATION_MINUTES)
                .Select(x => x.Key)
                .ToList();

            foreach (var key in expiredKeys)
            {
                // TryRemove là thread-safe
                provinceCache.TryRemove(key, out _);
            }

            Debug.WriteLine($"[CACHE] 🧹 Dọn {expiredKeys.Count} tỉnh hết hạn khỏi cache.");
        }

        #endregion

        #region Class
        public class ProvinceInfo
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Decree { get; set; }
        }

        public class GeocodeResult
        {
            public string Address { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public class ProvinceAirQualityData
        {
            public string ProvinceName { get; set; }
            public string Code { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Address { get; set; }
            public double AQI { get; set; }
            public string MainPollutant { get; set; }
            public double Temperature { get; set; }
            public int Humidity { get; set; }
            public string Description { get; set; }
            public double WindSpeed { get; set; }
            public string Forecast { get; set; }
            public DateTime FetchedTime { get; set; }

            public override string ToString()
            {
                return $"{ProvinceName} (lat={Latitude:F4}, lng={Longitude:F4}): AQI={AQI:F1} ({MainPollutant}), " +
                       $"Temp={Temperature:F1}°C, Humidity={Humidity}%, Wind={WindSpeed:F1}km/h";
            }
        }

        public class CachedData
        {
            public DateTime Timestamp { get; set; }
            public object Data { get; set; }
        }
        #endregion
    }

    public class CommuneWeatherPoint
    {
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }

        public string CommuneCode { get; set; }
        public string CommuneName { get; set; }

        public string Address { get; set; }

        public double Lat { get; set; }
        public double Lon { get; set; }

        // === WEATHER ===
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public string Description { get; set; }
        public double WindSpeed { get; set; }

        // === AIR QUALITY ===
        public double AQI { get; set; }
        public string MainPollutant { get; set; }

        // === POLLUTANTS DETAIL (µg/m³) ===
        public double PM25 { get; set; }
        public double PM10 { get; set; }
        public double O3 { get; set; }
        public double NO2 { get; set; }
        public double SO2 { get; set; }
        public double CO { get; set; }

        // === FORECASTS ===
        public List<string> Forecasts { get; set; } = new List<string>();

        // ✨ OPTIONAL: Timestamp để biết dữ liệu được cập nhật lúc nào
        public DateTime? FetchedTime { get; set; }

        public override string ToString()
        {
            return $"{CommuneName} ({ProvinceName}): AQI={AQI:F0} ({MainPollutant}), " +
                   $"Temp={Temperature:F1}°C, Humidity={Humidity}%, Wind={WindSpeed:F1}km/h, " +
                   $"PM2.5={PM25:F1}, PM10={PM10:F1}";
        }
    }
}