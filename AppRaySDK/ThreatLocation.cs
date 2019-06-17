using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class ThreatLocation
    {
        public string ThreatID { get; private set; }

        public List<string> Locations { get; private set; }

        internal ThreatLocation(string threatID, IEnumerable<string> locations)
        {
            ThreatID = threatID;
            Locations = locations.ToList();
        }

        internal ThreatLocation(string threatID, params string[] locations)
            : this(threatID, locations.ToList()) { }
    }
}