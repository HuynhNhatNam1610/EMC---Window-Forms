using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EMC.DTO;

namespace EMC.DAO
{
    public class CompanyDAO
    {
        private static CompanyDAO _instance;

        private CompanyDAO() { }

        public static CompanyDAO Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CompanyDAO();
                return _instance;
            }
        }

        /// <summary>
        /// Lấy thông tin công ty
        /// </summary>
        public Company GetCompanyInfo()
        {
            Company company = null;

            try
            {
                DataTable data = DataProvider.Instance.ExecuteProcedure("USP_GetCompanyInfo");

                if (data.Rows.Count > 0)
                {
                    DataRow row = data.Rows[0];
                    company = new Company
                    {
                        Name = row["name"]?.ToString() ?? "",
                        ShortName = row["short_name"]?.ToString() ?? "",
                        Logo = row["logo"]?.ToString() ?? "",
                        Address = row["address"]?.ToString() ?? "",
                        Hotline = row["hotline"]?.ToString() ?? "",
                        Email = row["email"]?.ToString() ?? "",
                        Description = row["description"]?.ToString() ?? ""
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CompanyDAO.GetCompanyInfo Error: {ex.Message}");
                throw;
            }

            return company;
        }

        /// <summary>
        /// Cập nhật thông tin công ty
        /// </summary>
        public bool UpdateCompanyInfo(Company company)
        {
            if (company == null) return false;
            try
            {
                object[] parameters = new object[]
                {
                    company.Name ?? "",
                    company.ShortName ?? "",
                    company.Logo ?? "",
                    company.Address ?? "",
                    company.Hotline ?? "",
                    company.Email ?? "",
                    company.Description ?? ""
                };

                DataTable result = DataProvider.Instance.ExecuteProcedure(
                    "USP_UpdateCompanyInfo",
                    parameters
                );

                if (result != null && result.Rows.Count > 0)
                {
                    int affectedRows = Convert.ToInt32(result.Rows[0]["affected_rows"]);
                    return affectedRows > 0;
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CompanyDAO.UpdateCompanyInfo Error: {ex.Message}");
                throw;
            }
        }
    }
}