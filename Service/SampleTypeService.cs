using EMC.DAO;
using EMC.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMC.Service
{
    public class SampleTypeService
    {
        private static SampleTypeService instance;
        public static SampleTypeService Instance
        {
            get { if (instance == null) instance = new SampleTypeService(); return instance; }
            private set { instance = value; }
        }

        public List<SampleType> GetSampleTypes()
        {
            return SampleTypeDAO.Instance.GetAllSampleTypes();
        }
    }
}
