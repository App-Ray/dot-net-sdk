using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class VirusScanResult
    {
        public string ScanAllResultA { get; set; }

        public int? ScanAllResultI { get; set; }

        public List<VirusScanDetails> DetailedResults { get; set; }

        public DateTime? StartTime { get; set; }

        public int? TotalAVs { get; set; }

        public int? TotalDetectingAVs { get; set; }

        public VirusScanResult(string scanAllResultA, int? scanAllResultI, List<VirusScanDetails> detailedResults,
            DateTime? startTime, int? totalAVs, int? totalDetectingAVs)
        {
            ScanAllResultA = scanAllResultA;
            ScanAllResultI = scanAllResultI;
            DetailedResults = detailedResults;
            StartTime = startTime;
            TotalAVs = totalAVs;
            TotalDetectingAVs = totalDetectingAVs;
        }
    }
}
