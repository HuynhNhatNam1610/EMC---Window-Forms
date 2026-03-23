using EMC.DTO;
using System.Data;

namespace EMC.DAO
{
    public class SampleDAO
    {
        private static SampleDAO instance;

        public static SampleDAO Instance
        {
            get { if (instance == null) instance = new SampleDAO(); return SampleDAO.instance; }
            private set { SampleDAO.instance = value; }
        }

        public SampleDAO() { }

        public List<Sample> GetSamples(string orderBy = "order_code")
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orderBy", orderBy }
            };
            DataTable data = DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetAllSamples", parameters);

            List<Sample> list = new List<Sample>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new Sample(row));
            }

            return list;
        }

        public List<Sample> GetSamplesdDistinct(string orderBy = "order_code")
        {
            DataTable data = DataProvider.Instance.ExecuteProcedure("USP_GetAllSamples_Distinct");

            List<Sample> list = new List<Sample>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new Sample(row));
            }

            return list;
        }

        public List<int> GetSampleIdsByOrderCode(string orderCode)
        {
            Dictionary<string, object> param = new()
            {
                { "@order_code", orderCode }
            };

            DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_GetSamples_ByOrderCode", param);

            return dt.AsEnumerable()
                     .Select(r => Convert.ToInt32(r["sample_id"]))
                     .ToList();
        }


        public int InsertSampleWithParams(
            int? contractId,
            string sampleCode,
            int sampleTypeId,
            string takenBy,
            int? storageId,
            decimal? sampleSize,
            decimal? longitude,
            decimal? latitude,
            DateTime? firstDate,
            DateTime? secondDate,
            DateTime? thirdDate,
            DateTime? createdAt,
            string beforePhoto,
            string afterPhoto,
            string description,
            string environmentalConditions,
            DateTime? resultDate,
            int positionId,
            IEnumerable<Sample_Parameter> rows
        )
        {
            var tvp = new DataTable();
            tvp.Columns.Add("parameter_id", typeof(int));
            tvp.Columns.Add("subsample_code", typeof(string));
            tvp.Columns.Add("storage_id", typeof(int));
            tvp.Columns.Add("value", typeof(decimal));
            tvp.Columns.Add("status", typeof(string));
            tvp.Columns.Add("department_responsive_id", typeof(int));
            tvp.Columns.Add("entered_by", typeof(string));
            tvp.Columns.Add("created_at", typeof(DateTime));
            tvp.Columns.Add("is_confirm", typeof(bool));

            if (rows != null)
            {
                foreach (var r in rows)
                {
                    tvp.Rows.Add(
                        r.ParameterId,
                        (object?)r.SubSampleCode ?? DBNull.Value,
                        (object?)r.StorageId ?? DBNull.Value,
                        (object?)r.Value ?? DBNull.Value,
                        (object?)r.Status ?? DBNull.Value,
                        (object?)r.DepartmentResponsiveId ?? DBNull.Value,
                        (object?)r.EnteredBy ?? DBNull.Value,
                        (object?)((r.CreatedAt == default) ? (DateTime?)null : r.CreatedAt) ?? DBNull.Value,
                        (object?)r.Confirm ?? DBNull.Value
                    );
                }
            }

            var parameters = new Dictionary<string, object>
            {
                ["@contract_id"] = contractId,
                ["@sample_code"] = (object?)sampleCode ?? DBNull.Value,
                ["@sample_type_id"] = sampleTypeId,
                ["@taken_by"] = (object?)takenBy ?? DBNull.Value,
                ["@storage_id"] = (object?)storageId ?? DBNull.Value,
                ["@sample_size"] = (object?)sampleSize ?? DBNull.Value,
                ["@longitude"] = (object?)longitude ?? DBNull.Value,
                ["@latitude"] = (object?)latitude ?? DBNull.Value,
                ["@first_sample_date"] = (object?)firstDate ?? DBNull.Value,
                ["@second_sample_date"] = (object?)secondDate ?? DBNull.Value,
                ["@third_sample_date"] = (object?)thirdDate ?? DBNull.Value,
                ["@created_at"] = (object?)createdAt ?? DBNull.Value,
                ["@before_photo"] = (object?)beforePhoto ?? DBNull.Value,
                ["@after_photo"] = (object?)afterPhoto ?? DBNull.Value,
                ["@description"] = (object?)description ?? DBNull.Value,
                ["@environmental_conditions"] = (object?)environmentalConditions ?? DBNull.Value,
                ["@result_date"] = (object?)resultDate ?? DBNull.Value,
                ["@position_id"] = positionId
            };

            object obj = DataProvider.Instance.ExecuteScalarProcWithTVP(
                "USP_AddSampleWithParamsAndResult",
                parameters,
                "@ParamRows",
                "ParamRow",
                tvp
            );

            return (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out var id)) ? id : 0;
        }
        public bool UpdateSampleWithParams(
            int sampleId,
            int? contractId,
            string sampleCode,
            int sampleTypeId,
            string takenBy,
            int? storageId,
            decimal? sampleSize,
            decimal? longitude,
            decimal? latitude,
            DateTime? firstDate,
            DateTime? secondDate,
            DateTime? thirdDate,
            DateTime? createdAt,
            string beforePhoto,
            string afterPhoto,
            string description,
            string environmentalConditions,
            DateTime? resultDate,
            int positionId,
            IEnumerable<Sample_Parameter> rows
        )
        {
            // 1) TVP @ParamRows (y như Insert)
            var tvp = new DataTable();
            tvp.Columns.Add("parameter_id", typeof(int));
            tvp.Columns.Add("subsample_code", typeof(string));
            tvp.Columns.Add("storage_id", typeof(int));
            tvp.Columns.Add("value", typeof(decimal));
            tvp.Columns.Add("status", typeof(string));
            tvp.Columns.Add("department_responsive_id", typeof(int));
            tvp.Columns.Add("entered_by", typeof(string));
            tvp.Columns.Add("created_at", typeof(DateTime));
            tvp.Columns.Add("is_confirm", typeof(bool));


            if (rows != null)
            {
                foreach (var r in rows)
                {
                    tvp.Rows.Add(
                        r.ParameterId,
                        (object?)r.SubSampleCode ?? DBNull.Value,
                        (object?)r.StorageId ?? DBNull.Value,
                        (object?)r.Value ?? DBNull.Value,
                        (object?)r.Status ?? DBNull.Value,
                        (object?)r.DepartmentResponsiveId ?? DBNull.Value,
                        (object?)r.EnteredBy ?? DBNull.Value,
                        (object?)((r.CreatedAt == default) ? (DateTime?)null : r.CreatedAt) ?? DBNull.Value,
                        (object?)r.Confirm ?? DBNull.Value
                    );
                }
            }

            // 2) Params cho proc UPDATE
            var parameters = new Dictionary<string, object>
            {
                ["@sample_id"] = sampleId,
                ["@contract_id"] = contractId,
                ["@sample_code"] = (object?)sampleCode ?? DBNull.Value,
                ["@sample_type_id"] = sampleTypeId,
                ["@taken_by"] = (object?)takenBy ?? DBNull.Value,
                ["@storage_id"] = (object?)storageId ?? DBNull.Value,
                ["@sample_size"] = (object?)sampleSize ?? DBNull.Value,
                ["@longitude"] = (object?)longitude ?? DBNull.Value,
                ["@latitude"] = (object?)latitude ?? DBNull.Value,
                ["@first_sample_date"] = (object?)firstDate ?? DBNull.Value,
                ["@second_sample_date"] = (object?)secondDate ?? DBNull.Value,
                ["@third_sample_date"] = (object?)thirdDate ?? DBNull.Value,
                ["@created_at"] = (object?)createdAt ?? DBNull.Value,
                ["@before_photo"] = (object?)beforePhoto ?? DBNull.Value,
                ["@after_photo"] = (object?)afterPhoto ?? DBNull.Value,
                ["@description"] = (object?)description ?? DBNull.Value,
                ["@environmental_conditions"] = (object?)environmentalConditions ?? DBNull.Value,
                ["@result_date"] = (object?)resultDate ?? DBNull.Value,
                ["@position_id"] = positionId
            };

            // 3) Gọi proc UPDATE (trả về 1 nếu OK)
            var obj = EMC.DAO.DataProvider.Instance.ExecuteScalarProcWithTVP(
                "USP_UpdateSampleWithParamsAndResult",
                parameters,
                "@ParamRows",
                "ParamRow",
                tvp
            ); // cùng kiểu gọi như Insert :contentReference[oaicite:3]{index=3}

            // nếu proc trả 1 → true
            return obj != null && obj != DBNull.Value && obj.ToString() == "1";
        }

        //public bool CheckSampleCodeExists(string sampleCode)
        //{
        //    var inputParams = new Dictionary<string, object>
        //    {
        //        ["@SampleCode"] = sampleCode
        //    };

        //    var outputParams = new Dictionary<string, SqlDbType>
        //    {
        //        ["@Exists"] = SqlDbType.Bit
        //    };

        //    var outs = DataProvider.Instance.ExecuteProcedureWithOutput(
        //        "USP_CheckSampleCodeExists",
        //        inputParams,
        //        outputParams
        //    ); // dùng ExecuteProcedureWithOutput

        //    if (outs != null && outs.TryGetValue("@Exists", out var val) && val != null && val != DBNull.Value)
        //    {
        //        // Chuẩn hoá nhiều trường hợp driver trả về (bool/int/byte/string)
        //        if (val is bool b) return b;
        //        if (val is int i) return i != 0;
        //        if (val is byte by) return by != 0;
        //        if (bool.TryParse(val.ToString(), out var b2)) return b2;
        //    }
        //    return false;
        //}
        public bool CheckSampleCodeExists(string sampleCode, int? sampleId)
        {
            var input = new Dictionary<string, object>
            {
                ["@SampleCode"] = sampleCode,
                ["@SampleId"] = sampleId ?? (object)DBNull.Value
            };

            var output = new Dictionary<string, SqlDbType>
            {
                ["@Exists"] = SqlDbType.Bit
            };

            var outs = DataProvider.Instance.ExecuteProcedureWithOutput(
                "USP_CheckSampleCodeExists",
                input,
                output
            );

            bool exists = false;

            if (outs.TryGetValue("@Exists", out object val))
            {
                if (val is bool b) exists = b;
                else exists = Convert.ToInt32(val) == 1;
            }

            return exists;
        }

        public (Sample Header, List<Sample_Parameter> Parameters) GetSampleFullForEdit(int sampleId)
        {
            var input = new Dictionary<string, object> { ["@SampleId"] = sampleId };
            DataSet ds = DataProvider.Instance.ExecuteProcedureReturnDataSet("USP_GetSampleFullForEdit", input);

            Sample header = null;
            List<Sample_Parameter> details = new List<Sample_Parameter>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                header = new Sample(ds.Tables[0].Rows[0]);

            if (ds.Tables.Count > 1)
            {
                foreach (DataRow row in ds.Tables[1].Rows)
                    details.Add(new Sample_Parameter(row));
            }

            return (header, details);
        }

        public bool DeleteSample(int sampleId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@SampleId"] = sampleId
            };

            // Gọi proc trả về SELECT Success,...
            var dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_DeleteSample", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                // Nếu proc trả 'Success' = 1 thì thành công
                if (row.Table.Columns.Contains("Success") && row["Success"] != DBNull.Value)
                {
                    var okVal = row["Success"].ToString();
                    return okVal == "1" || okVal.Equals("True", StringComparison.OrdinalIgnoreCase);
                }
            }
            return false;
        }

        public int? GetSampleIdByOrderTypePosition(int orderId, int sampleTypeId, int positionId)
        {
            var prms = new Dictionary<string, object>
            {
                ["@OrderId"] = orderId,
                ["@SampleTypeId"] = sampleTypeId,
                ["@PositionId"] = positionId
            };

            var dt = DataProvider.Instance.ExecuteProcedure(
                "USP_GetSampleIdByOrderTypePosition",
                new object[] { orderId, sampleTypeId, positionId } // → @param1,@param2,@param3
            );

            if (dt == null || dt.Rows.Count == 0) return null;

            // cột đầu tiên là id
            return dt.Rows[0][0] == DBNull.Value ? (int?)null : Convert.ToInt32(dt.Rows[0][0]);
        }
        public List<Sample> GetSamplesByOrderId(int orderId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@OrderId"] = orderId
            };

            DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_GetSamplesByOrderId",
                parameters
            );

            List<Sample> list = new List<Sample>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Sample(row));
            }

            return list;
        }

        public List<(int SampleId, string SampleCode, bool IsConfirm)> GetSamplesByOrder(int orderId)
        {
            var prms = new Dictionary<string, object>
            {
                ["@OrderId"] = orderId  // ✅ Correct parameter name
            };

            var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_GetSamplesByOrderId",  // ✅ Correct procedure name (exists in SQL)
                prms);

            var list = new List<(int, string, bool)>();

            if (dt == null || dt.Rows.Count == 0)
                return list;

            foreach (DataRow row in dt.Rows)
            {
                int id = 0;
                string code = null;
                bool isConfirm = false;

                if (row.Table.Columns.Contains("sample_id") && row["sample_id"] != DBNull.Value)
                    id = Convert.ToInt32(row["sample_id"]);

                if (row.Table.Columns.Contains("sample_code") && row["sample_code"] != DBNull.Value)
                    code = row["sample_code"].ToString();

                if (row.Table.Columns.Contains("is_confirm") && row["is_confirm"] != DBNull.Value)
                    isConfirm = Convert.ToBoolean(row["is_confirm"]);

                list.Add((id, code, isConfirm));
            }

            return list;
        }
        // Biểu đồ 1
        public DataTable GetQuarterlyOrderVolumeData(int year)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Year"] = year
            };
            return DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetQuarterlyOrderData", parameters);
        }

        // Biểu đồ 2 Dự đoán tái ký hợp đồng AI
        public DataTable GetContractRenewalData(int year)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Year"] = year
            };
            return DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetContractRenewalPredictionData", parameters);
        }

        public DataTable GetAvailableYears()
        {
            return DataProvider.Instance.ExecuteProcedure("USP_GetAvailableYears");
        }
        public List<Sample> GetSamplesByOrderCode(string orderCode)
        {
            Dictionary<string, object> param = new() {
                { "@order_code", orderCode }
            };

            DataTable data = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_GetSamples_ByOrderCode", param
            );

            List<Sample> list = new();
            foreach (DataRow row in data.Rows)
                list.Add(new Sample(row));

            return list;
        }

        // 🟢 Lấy danh sách SampleType theo OrderCode
        public List<SampleType> GetSampleTypesByOrderCode(string orderCode)
        {
            var param = new Dictionary<string, object>
            {
                ["@OrderCode"] = orderCode
            };

            DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_GetSampleTypes_ByOrderCode",
                param
            );

            List<SampleType> list = new();

            foreach (DataRow row in dt.Rows)
                list.Add(new SampleType(row));   // dùng constructor DataRow

            return list;
        }

        public List<Position> GetPositions(string orderCode, int sampleTypeId)
        {
            var param = new Dictionary<string, object>
            {
                ["@OrderCode"] = orderCode,
                ["@SampleTypeId"] = sampleTypeId
            };

            var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_GetPositions_ByOrderCode_And_Type",
                param
            );

            var list = new List<Position>();

            foreach (DataRow row in dt.Rows)
                list.Add(new Position(row));   // dùng constructor Position(DataRow)

            return list;
        }




    }
}
