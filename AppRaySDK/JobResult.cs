using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class JobResult
    {
        public string AppHash { get; private set; }

        public string Label { get; private set; }

        public string Package { get; private set; }

        public string Platform { get; private set; }

        public int? ProgressFinished { get; private set; }

        public int? ProgressTotal { get; private set; }

        public string RiskGrade { get; private set; }

        public int? RiskScore { get; private set; }

        public Guid? ScanID { get; private set; }

        public string Status { get; private set; }

        public string Submitter { get; private set; }

        public ThreatCounts ThreatCounts { get; private set; }

        public DateTime? ScanStart { get; private set; }

        public DateTime? ScanFinish { get; private set; }

        public DateTime? Upload { get; private set; }

        public Guid? Uuid { get; private set; }

        public string Version { get; private set; }

        internal JobResult(string appHash, string label, string package, string platform,
            int? progressFinished, int? progressTotal, string riskGrade, int? riskScore,
            Guid? scanID, string status, string submitter, ThreatCounts threatCounts,
            DateTime? scanStart, DateTime? scanFinish, DateTime? upload, Guid? uuid,
            string version)
        {
            AppHash = appHash;
            Label = label;
            Package = package;
            Platform = platform;
            ProgressFinished = progressFinished;
            ProgressTotal = progressTotal;
            RiskGrade = riskGrade;
            RiskScore = riskScore;
            ScanID = scanID;
            Status = status;
            Submitter = submitter;
            ThreatCounts = threatCounts;
            ScanStart = scanStart;
            ScanFinish = scanFinish;
            Upload = upload;
            Uuid = uuid;
            Version = version;
        }
    }
}
