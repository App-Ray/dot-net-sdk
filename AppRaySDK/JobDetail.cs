using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class JobDetails
    {
        public string AppHash { get; private set; }

        public string Label { get; private set; }

        public string Package { get; private set; }

        public string Platform { get; private set; }

        public int? ProgressFinished { get; private set; }

        public int? ProgressTotal { get; private set; }

        public string RiskGrade { get; private set; }

        public int? RiskScore { get; private set; }

        public long? ScanDuration { get; private set; }

        public Dictionary<string, string> ScanErrors { get; private set; }

        public Guid? ScanID { get; private set; }

        public string Status { get; private set; }

        public string Submitter { get; private set; }

        public ThreatCounts ThreatCounts { get; private set; }

        public List<ThreatLocation> ThreatLocations { get; private set; }

        public List<ThreatSummary> ThreatSummaries { get; private set; }

        public DateTime? ScanStart { get; private set; }

        public DateTime? ScanFinish { get; private set; }

        public DateTime? Upload { get; private set; }

        public Guid? Uuid { get; private set; }

        public string Version { get; private set; }

        internal JobDetails(string appHash, string label, string package, string platform,
            int? progressFinished, int? progressTotal, string riskGrade, int? riskScore, long? scanDuration,
            Dictionary<string, string> scanErrors, Guid? scanID, string status, string submitter,
            ThreatCounts threatCounts, List<ThreatLocation> threatLocations,
            List<ThreatSummary> threatSummaries, DateTime? scanStart, DateTime? scanFinish, DateTime? upload,
            Guid? uuid, string version)
        {
            AppHash = appHash;
            Label = label;
            Package = package;
            Platform = platform;
            ProgressFinished = progressFinished;
            ProgressTotal = progressTotal;
            RiskGrade = riskGrade;
            RiskScore = riskScore;
            ScanDuration = scanDuration;
            ScanErrors = scanErrors;
            ScanID = scanID;
            Status = status;
            Submitter = submitter;
            ThreatCounts = threatCounts;
            ThreatLocations = threatLocations;
            ThreatSummaries = threatSummaries;
            ScanStart = scanStart;
            ScanFinish = scanFinish;
            Upload = upload;
            Uuid = uuid;
            Version = version;
        }
    }
}
