using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class ThreatCounts
    {
        public int? Low { get; private set; }

        public int? Medium { get; private set; }

        public int? High { get; private set; }

        public int? Total
        {
            get { return Low + Medium + High; }
        }

        internal ThreatCounts(int? low, int? medium, int? high)
        {
            Low = low;
            Medium = medium;
            High = high;
        }
    }
}
