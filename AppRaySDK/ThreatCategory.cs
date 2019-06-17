using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class ThreatCategory
    {
        public string MainCategory { get; private set; }

        public string Category { get; private set; }

        public List<string> ThreatIDs { get; private set; }

        public ThreatCategory(string mainCategory, string category, List<string> threatIDs)
        {
            MainCategory = mainCategory;
            Category = category;
            ThreatIDs = threatIDs;
        }
    }
}
