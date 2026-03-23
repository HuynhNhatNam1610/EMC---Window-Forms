using EMC.DAO;
using EMC.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMC.DAO;
using EMC.DTO;

namespace EMC.Service
{
    public class ParameterService
    {
        private static ParameterService instance;

        public static ParameterService Instance
        {
            get { if (instance == null) instance = new ParameterService(); return instance; }
            private set { instance = value; }
        }

        private ParameterService() { }

        public List<Parameter> GetAllParameters(string keyword = null, string orderBy = "name")
        {
            try
            {
                return ParameterDAO.Instance.GetAllParameters(keyword, orderBy);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách Parameter: " + ex.Message, ex);
            }
        }
        public Parameter GetParameterById(int id)
        {
            try
            {
                var allParams = ParameterDAO.Instance.GetAllParameters();
                return allParams.FirstOrDefault(p => p.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy Parameter theo ID: " + ex.Message, ex);
            }
        }

    }
}
