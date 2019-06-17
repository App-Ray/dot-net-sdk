using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class VirusScanDetails
    {
        public string AntiVirusSoftware { get; set; }

        public DateTime? DefTime { get; set; }

        public int? ScanResultI { get; set; }

        public int? ScanTime { get; set; }

        public string ThreatFound { get; set; }

        public VirusScanDetails(string antiVirusSoftware, DateTime? defTime, int? scanResultI,
            int? scanTime, string threatFound)
        {
            AntiVirusSoftware = antiVirusSoftware;
            DefTime = defTime;
            ScanResultI = scanResultI;
            ScanTime = scanTime;
            ThreatFound = threatFound;
        }
    }
}
