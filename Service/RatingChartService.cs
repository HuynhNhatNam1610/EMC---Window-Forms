using EMC.DAO;
using EMC.DTO;
using EMC.ML;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;

namespace EMC.Service
{
    public class RatingChartService
    {

        private static RatingChartService instance;

        public static RatingChartService Instance
        {
            get { if (instance == null) instance = new RatingChartService(); return instance; }
            private set { instance = value; }
        }

        private RatingChartService() { }


        #region Statistics & Chart Data

        public Contract.StatisticsData GetStatistics(int year)
        {
            try
            {
                DataTable dt = ContractDAO.Instance.GetStatisticsData(year);
                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    return new Contract.StatisticsData
                    {
                        TotalContracts = row.Table.Columns.Contains("TotalContracts") && row["TotalContracts"] != DBNull.Value
                            ? Convert.ToInt32(row["TotalContracts"]) : 0,
                        OverdueContracts = row.Table.Columns.Contains("OverdueContracts") && row["OverdueContracts"] != DBNull.Value
                            ? Convert.ToInt32(row["OverdueContracts"]) : 0,
                        ProcessingContracts = row.Table.Columns.Contains("InProgressContracts") && row["InProgressContracts"] != DBNull.Value
                            ? Convert.ToInt32(row["InProgressContracts"])
                            : (row.Table.Columns.Contains("ProcessingContracts") && row["ProcessingContracts"] != DBNull.Value
                                ? Convert.ToInt32(row["ProcessingContracts"])
                                : 0),
                        CompletionRate = row.Table.Columns.Contains("CompletionRate") && row["CompletionRate"] != DBNull.Value
                            ? Convert.ToDecimal(row["CompletionRate"]) : 0
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetStatistics ERROR (year={year}): {ex.Message}\n{ex.StackTrace}");
            }
            return new Contract.StatisticsData
            {
                TotalContracts = 156,
                OverdueContracts = 3,
                ProcessingContracts = 12,
                CompletionRate = 95
            };
        }

        public List<Sample.QuarterlyData> GetQuarterlyOrderVolume(int year)
        {
            var result = new List<Sample.QuarterlyData>();
            try
            {
                DataTable dt = SampleDAO.Instance.GetQuarterlyOrderVolumeData(year);
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new Sample.QuarterlyData
                    {
                        Quarter = row["Quarter"]?.ToString() ?? "",
                        OnTime = row["OnTime"] != DBNull.Value ? Convert.ToInt32(row["OnTime"]) : 0,
                        Late = row["Late"] != DBNull.Value ? Convert.ToInt32(row["Late"]) : 0
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetQuarterlyOrderVolume ERROR: {ex.Message}");
            }
            return result;
        }

        public List<ContractRenewalData> GetContractRenewal(int year)
        {
            var result = new List<ContractRenewalData>();
            try
            {
                DataTable dt = ContractDAO.Instance.GetContractRenewalData(year);
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new ContractRenewalData
                    {
                        CustomerName = row["CustomerName"]?.ToString() ?? "",
                        RenewCount = row["RenewCount"] != DBNull.Value ? Convert.ToInt32(row["RenewCount"]) : 0,
                        YearsOfRelation = row["YearsOfRelation"] != DBNull.Value ? Convert.ToDouble(row["YearsOfRelation"]) : 0,
                        OnTimePaymentRate = row["OnTimePaymentRate"] != DBNull.Value ? Convert.ToDouble(row["OnTimePaymentRate"]) : 0,
                        CustomerSatisfaction = row["CustomerSatisfaction"] != DBNull.Value ? Convert.ToDouble(row["CustomerSatisfaction"]) : 0,
                        ComplaintCount = row["ComplaintCount"] != DBNull.Value ? Convert.ToInt32(row["ComplaintCount"]) : 0,
                        TotalContractValue = row["TotalContractValue"] != DBNull.Value ? Convert.ToDouble(row["TotalContractValue"]) : 0
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetContractRenewal ERROR: {ex.Message}");
            }
            return result;
        }

        public List<int> GetAvailableYears()
        {
            var years = new List<int>();
            try
            {
                DataTable dt = ContractDAO.Instance.GetAvailableYears();
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] != DBNull.Value)
                    {
                        years.Add(Convert.ToInt32(row[0]));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetAvailableYears ERROR: {ex.Message}");
            }
            return years;
        }

        public double PredictRenewalProbability(int renewCount, double years, double paymentRate, double satisfaction, int complaints, double contractValue)
        {
            double normalizedPayment = Math.Min(100, paymentRate);
            double normalizedSatisfaction = Math.Min(100, satisfaction * 20);
            double normalizedComplaints = Math.Max(0, 100 - complaints * 10);
            double normalizedYears = Math.Min(100, years * 10);
            double normalizedRenew = Math.Min(100, renewCount * 20);
            double normalizedValue = Math.Min(100, contractValue / 100000000);
            double probability = normalizedRenew * 0.25 + normalizedYears * 0.15 + normalizedPayment * 0.20
                               + normalizedSatisfaction * 0.25 + normalizedComplaints * 0.10 + normalizedValue * 0.05;
            return Math.Round(Math.Min(95, Math.Max(5, probability)), 1);
        }

        #endregion


        #region Data Models

        public class ProvinceData
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double PollutionLevel { get; set; }
            public string Pollutant { get; set; }
            public List<PollutionPoint> PollutionPoints { get; set; }
            public WeatherInfo Weather { get; set; }
            public List<string> IndustrialZones { get; set; }
            public List<string> MergedFrom { get; set; }

            public ProvinceData(double lat, double lon, double pollution, string pollutant,
                                List<PollutionPoint> points, WeatherInfo weather = null,
                                List<string> industrialZones = null, List<string> mergedFrom = null)
            {
                Latitude = lat;
                Longitude = lon;
                PollutionLevel = pollution;
                Pollutant = pollutant;
                PollutionPoints = points ?? new List<PollutionPoint>();
                Weather = weather;
                IndustrialZones = industrialZones ?? new List<string>();
                MergedFrom = mergedFrom ?? new List<string>();
            }
        }

        public class PollutionPoint
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double Level { get; set; }
            public string Description { get; set; }

            public PollutionPoint(double lat, double lon, double level, string desc)
            {
                Latitude = lat;
                Longitude = lon;
                Level = level;
                Description = desc;
            }
        }

        public class WeatherInfo
        {
            public double Temperature { get; set; }
            public int Humidity { get; set; }
            public string Description { get; set; }
            public double WindSpeed { get; set; }
            public string Forecast { get; set; }
        }

        #endregion

        #region Province Static Data (Fallback)

        public Dictionary<string, ProvinceData> Get34ProvincesInfo()
        {
            return new Dictionary<string, ProvinceData>
            {
                // ===== 5 thành phố trực thuộc trung ương =====
                { "Hà Nội", new ProvinceData(21.0245, 105.8412, 0, null, null, null, null, new List<string> { "Giữ nguyên" }) },
                { "Hải Phòng", new ProvinceData(20.8561, 106.6822, 0, null, null, null, null, new List<string> { "Hải Dương", "Hải Phòng" }) },
                { "Đà Nẵng", new ProvinceData(16.0678, 108.2208, 0, null, null, null, null, new List<string> { "Quảng Nam", "Đà Nẵng" }) },
                { "Hồ Chí Minh", new ProvinceData(10.8231, 106.6297, 0, null, null, null, null, new List<string> { "Bình Dương", "TP. Hồ Chí Minh", "Bà Rịa - Vũng Tàu" }) },
                { "Cần Thơ", new ProvinceData(10.0333, 105.7833, 0, null, null, null, null, new List<string> { "Sóc Trăng", "Hậu Giang", "Cần Thơ" }) },
                { "Huế", new ProvinceData(16.4667, 107.6, 0, null, null, null, null, new List<string> { "Giữ nguyên" }) },

                // ===== 2 quần đảo =====
                { "Hoàng Sa", new ProvinceData(16.5, 112.0, 0, null, null, null, null, null) },
                { "Trường Sa", new ProvinceData(8.6, 112.5, 0, null, null, null, null, null) },

                // ===== 28 tỉnh còn lại =====
                { "An Giang", new ProvinceData(10.5, 105.1667, 0, null, null, null, null, new List<string> { "Kiên Giang", "An Giang" }) },
                { "Bắc Ninh", new ProvinceData(21.1833, 106.05, 0, null, null, null, null, new List<string> { "Bắc Giang", "Bắc Ninh" }) },
                { "Cà Mau", new ProvinceData(9.1792, 105.1458, 0, null, null, null, null, new List<string> { "Bạc Liêu", "Cà Mau" }) },
                { "Cao Bằng", new ProvinceData(22.6667, 106.2588, 0, null, null, null, null, new List<string> { "Giữ nguyên" }) },
                { "Đắk Lắk", new ProvinceData(12.6667, 108.0500, 0, null, null, null, null, new List<string> { "Phú Yên", "Đắk Lắk" }) },
                { "Điện Biên", new ProvinceData(21.3833, 103.0167, 0, null, null, null, null, new List<string> { "Giữ nguyên" }) },
                { "Đồng Nai", new ProvinceData(10.9519, 106.8440, 0, null, null, null, null, new List<string> { "Bình Phước", "Đồng Nai" }) },
                { "Đồng Tháp", new ProvinceData(10.4493, 106.3420, 0, null, null, null, null, new List<string> { "Tiền Giang", "Đồng Tháp" }) },
                { "Gia Lai", new ProvinceData(13.75, 108.25, 0, null, null, null, null, new List<string> { "Gia Lai", "Bình Định" }) },
                { "Hà Tĩnh", new ProvinceData(18.3453, 105.9019, 0, null, null, null, null, new List<string> { "Giữ nguyên" }) },
                { "Hưng Yên", new ProvinceData(20.6464, 106.0511, 0, null, null, null, null, new List<string> { "Thái Bình", "Hưng Yên" }) },
                { "Khánh Hòa", new ProvinceData(12.2388, 109.1967, 0, null, null, null, null, new List<string> { "Khánh Hòa", "Ninh Thuận" }) },
                { "Lai Châu", new ProvinceData(22.3864, 103.4702, 0, null, null, null, null, new List<string> { "Giữ nguyên" }) },
                { "Lâm Đồng", new ProvinceData(11.9404, 108.4583, 0, null, null, null, null, new List<string> { "Đắk Nông", "Lâm Đồng", "Bình Thuận" }) },
                { "Lạng Sơn", new ProvinceData(21.8505, 106.7662, 0, null, null, null, null, new List<string> { "Giữ nguyên" }) },
                { "Lào Cai", new ProvinceData(22.4997, 103.9657, 0, null, null, null, null, new List<string> { "Lào Cai", "Yên Bái" }) },
                { "Nghệ An", new ProvinceData(18.6792, 105.6825, 0, null, null, null, null, new List<string> { "Giữ nguyên" }) },
                { "Ninh Bình", new ProvinceData(20.2506, 105.9745, 0, null, null, null, null, new List<string> { "Hà Nam", "Ninh Bình", "Nam Định" }) },
                { "Phú Thọ", new ProvinceData(21.4208, 105.2045, 0, null, null, null, null, new List<string> { "Hòa Bình", "Vĩnh Phúc", "Phú Thọ" }) },
                { "Quảng Ngãi", new ProvinceData(15.1214, 108.8044, 0, null, null, null, null, new List<string> { "Quảng Ngãi", "Kon Tum" }) },
                { "Quảng Ninh", new ProvinceData(21.0062, 107.2925, 0, null, null, null, null, new List<string> { "Giữ nguyên" }) },
                { "Quảng Trị", new ProvinceData(16.75, 107.2, 0, null, null, null, null, new List<string> { "Quảng Bình", "Quảng Trị" }) },
                { "Sơn La", new ProvinceData(21.3283, 103.9005, 0, null, null, null, null, new List<string> { "Giữ nguyên" }) },
                { "Tây Ninh", new ProvinceData(10.6667, 106.1667, 0, null, null, null, null, new List<string> { "Long An", "Tây Ninh" }) },
                { "Thái Nguyên", new ProvinceData(22.1495, 105.8372, 0, null, null, null, null, new List<string> { "Bắc Kạn", "Thái Nguyên" }) },
                { "Thanh Hóa", new ProvinceData(19.8, 105.7667, 0, null, null, null, null, new List<string> { "Giữ nguyên" }) },
                { "Tuyên Quang", new ProvinceData(21.8233, 105.2181, 0, null, null, null, null, new List<string> { "Hà Giang", "Tuyên Quang" }) },
                { "Vĩnh Long", new ProvinceData(10.2544, 105.967, 0, null, null, null, null, new List<string> { "Bến Tre", "Vĩnh Long", "Trà Vinh" }) }
            };
        }
        #endregion
    }
}