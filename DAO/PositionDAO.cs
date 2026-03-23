using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMC.DTO;

namespace EMC.DAO
{
    public class PositionDAO
    {
        private static PositionDAO instance;
        public static PositionDAO Instance => instance ??= new PositionDAO();
        private PositionDAO() { }

        public int GetOrCreate(int contractId, string site)
        {
            var inputs = new Dictionary<string, object>
            {
                ["@contract_id"] = contractId,
                ["@site"] = site
            };
            var outputs = new Dictionary<string, SqlDbType>
            {
                ["@position_id"] = SqlDbType.Int
            };

            var outs = DataProvider.Instance.ExecuteProcedureWithOutput(
                "USP_GetOrCreatePosition", inputs, outputs);

            return (outs != null && outs["@position_id"] != null && outs["@position_id"] != DBNull.Value)
                ? Convert.ToInt32(outs["@position_id"])
                : 0;
        }

        public List<Position> GetByContract(int contractId)
        {
            var list = new List<Position>();

            // Cách 2: dùng ExecuteQuery (phổ biến trong project của bạn)
            DataTable dt = DataProvider.Instance.ExecuteQuery(
                "EXEC USP_GetPositionsByContract @ContractId",
                new object[] { contractId }
            );

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new Position(row));
                }
            }
            return list;
        }

        public List<Position> GetByOrderAndSampleType(int orderId, int sampleTypeId)
        {
            var list = new List<Position>();

            // ✅ Gọi SP bằng overload có dictionary để map đúng @OrderId, @SampleTypeId
            var prms = new Dictionary<string, object>
            {
                ["@OrderId"] = orderId,
                ["@SampleTypeId"] = sampleTypeId
            };

            DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_GetPositionsByOrderAndSampleType", prms
            );

            if (dt == null || dt.Rows.Count == 0) return list;

            foreach (DataRow r in dt.Rows)
            {
                list.Add(new Position
                {
                    Id = r.Table.Columns.Contains("position_id") && r["position_id"] != DBNull.Value
                        ? Convert.ToInt32(r["position_id"]) : 0,
                    ContractId = r.Table.Columns.Contains("contract_id") && r["contract_id"] != DBNull.Value
                        ? Convert.ToInt32(r["contract_id"]) : 0,
                    Site = r.Table.Columns.Contains("site") && r["site"] != DBNull.Value
                        ? r["site"].ToString() : null,
                    ContractCode = r.Table.Columns.Contains("contract_code") && r["contract_code"] != DBNull.Value
                        ? r["contract_code"].ToString() : null,
                    OrderCode = r.Table.Columns.Contains("order_code") && r["order_code"] != DBNull.Value
                        ? r["order_code"].ToString() : null,
                    SampleTypeName = r.Table.Columns.Contains("sample_type_name") && r["sample_type_name"] != DBNull.Value
                        ? r["sample_type_name"].ToString() : null,
                    SampleCount = r.Table.Columns.Contains("sample_count") && r["sample_count"] != DBNull.Value
                        ? Convert.ToInt32(r["sample_count"]) : 0
                });
            }
            return list;
        }
    }
}
