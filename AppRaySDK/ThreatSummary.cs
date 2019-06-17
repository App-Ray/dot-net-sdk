using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class ThreatSummary
    {
        public string ThreatID { get; private set; }

        public string Description { get; private set; }

        public string Explanation { get; private set; }

        internal ThreatSummary(string threatID, string description, string explanation)
        {
            ThreatID = threatID;
            Description = description;
            Explanation = explanation;
        }
    }
}