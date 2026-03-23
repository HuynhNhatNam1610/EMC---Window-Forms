using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMC.Service
{
    public class Sample_ParameterService
    {
        private static Sample_ParameterService instance;

        public static Sample_ParameterService Instance
        {
            get { if (instance == null) instance = new Sample_ParameterService(); return instance; }
            private set { instance = value; }
        }

        private Sample_ParameterService() { }
    }
}
