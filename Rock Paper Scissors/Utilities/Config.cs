using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rock_Paper_Scissors.Utilities
{
    public class Config
    {
        public class Mouve
        {
            public string IDName { get; set; }
            public string[] StrongerThan { get; set; }
        }

        public int NumberOfMatch { get; set; }
        public Mouve[] mouves { get; set; }
    }
}