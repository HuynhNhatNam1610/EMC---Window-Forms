using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMC.DAO
{
    public class Sample_ParameterDAO
    {
        private static Sample_ParameterDAO instance;

        public static Sample_ParameterDAO Instance
        {
            get { if (instance == null) instance = new Sample_ParameterDAO(); return Sample_ParameterDAO.instance; }
            private set { Sample_ParameterDAO.instance = value; }
        }

        public Sample_ParameterDAO() { }
    }
}
