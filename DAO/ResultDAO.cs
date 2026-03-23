using System.Data;

namespace EMC.DAO
{
    public class ResultDAO
    {
        private static ResultDAO instance;
        public static ResultDAO Instance
        {
            get { return instance ??= new ResultDAO(); }
            private set { instance = value; }
        }

        // Lưới fResult
        public DataTable GetAllResults()
        {
            return DataProvider.Instance.ExecuteProcedure("USP_GetAllResults");
        }

        // Combobox Hợp đồng
        public DataTable GetAllContracts()
        {
            // Dùng SP có sẵn trong DB
            return DataProvider.Instance.ExecuteProcedure("USP_GetAllContract");
        }

        // Combobox Mã mẫu theo Hợp đồng
        public DataTable GetSamplesByContractId(int contractId)
        {
            var p = new Dictionary<string, object> { { "@ContractId", contractId } };
            return DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetSamplesByContractId", p);
        }

        // Combobox Nhân viên (mã NV) + fill thông tin
        public DataTable GetAllStaff()
        {
            // Dùng SP có sẵn trong DB
            return DataProvider.Instance.ExecuteProcedure("USP_GetAllStaff");
        }

        // Lưu báo cáo

        // ✅ Xác nhận kết quả (is_confirm = 1, confirm_date = GETDATE())
        public int ConfirmResult(int resultId, string confirmBy = null)
        {
            var parameters = new Dictionary<string, object>
            {
        { "@ResultId", resultId },
        { "@ConfirmBy", (object?)confirmBy ?? DBNull.Value }
            };

            return DataProvider.Instance.ExecuteNonQueryProcedure("USP_ConfirmResult", parameters);
        }

        public int ConfirmLatestBySample(int sampleId, string confirmBy = null)
        {
            var p = new Dictionary<string, object> {
                {"@SampleId", sampleId},
                {"@ConfirmBy", (object?)confirmBy ?? DBNull.Value}
            };
            return DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_ConfirmLatestResultBySample", p);
        }
        public int GetEmailedCountBySample(int sampleId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@SampleId", sampleId }
            };

            DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_Result_GetEmailedCountBySample", parameters
            );

            if (dt.Rows.Count > 0 && dt.Rows[0]["emailed_count"] != DBNull.Value)
                return Convert.ToInt32(dt.Rows[0]["emailed_count"]);

            return 0;
        }

        public bool MarkEmailedBySample(int sampleId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@SampleId", sampleId }
            };

            int result = DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_Result_MarkEmailedBySample", parameters
            );

            return result > 0;
        }

        public bool UpdateEmailedDate(int resultId, DateTime emailedDate)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ResultId", resultId },
                { "@EmailedDate", emailedDate }
            };

            int rows = DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_Result_UpdateEmailedDate",
                parameters
            );

            return rows > 0;
        }

        public bool GetLatestConfirmStatus(int sampleId)
        {
            var prms = new Dictionary<string, object>
            {
                ["@SampleId"] = sampleId
            };

            DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_GetLatestConfirmStatus", prms);

            if (dt != null && dt.Rows.Count > 0)
            {
                // cột is_confirm là BIT ⇒ có thể DBNull
                object v = dt.Rows[0]["is_confirm"];
                if (v != DBNull.Value && v != null)
                    return Convert.ToBoolean(v);
            }

            return false;   // mặc định: chưa xác nhận
        }

        public bool ConfirmLatestBySampleId(int sampleId)
        {
            var prms = new Dictionary<string, object> { ["@SampleId"] = sampleId };
            var dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_ConfirmLatestResultBySample", prms);
            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("Success"))
            {
                var ok = dt.Rows[0]["Success"]?.ToString();
                return ok == "1" || ok?.Equals("True", StringComparison.OrdinalIgnoreCase) == true;
            }
            return false;
        }

    }
}
