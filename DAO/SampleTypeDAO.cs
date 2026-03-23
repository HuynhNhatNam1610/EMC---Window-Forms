using EMC.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMC.DAO
{
    public class SampleTypeDAO
    {
        private static SampleTypeDAO instance;
        public static SampleTypeDAO Instance
        {
            get { if (instance == null) instance = new SampleTypeDAO(); return instance; }
            private set { instance = value; }
        }

        public List<SampleType> GetAllSampleTypes()
        {
            DataTable data = DataProvider.Instance.ExecuteProcedure("USP_GetAllSampleType");
            List<SampleType> list = new List<SampleType>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new SampleType(row));
            }
            return list;
        }
    }
}
