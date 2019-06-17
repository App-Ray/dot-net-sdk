using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class ScanStatistics
    {
        public int? AverageDuration { get; private set; }

        public int? AverageThreats { get; private set; }

        public int? CompletedScans { get; private set; }

        public int? RunningScans { get; private set; }

        public int? QueuedScans { get; private set; }

        public int? TotalScans
        {
            get
            {
                return CompletedScans + RunningScans + QueuedScans;
            }
        }

        public ScanStatistics(int? completedScans, int? runningScans, int? queuedScans, int? averageDuration, int? averageThreats)
        {
            CompletedScans = completedScans;
            RunningScans = runningScans;
            QueuedScans = queuedScans;
            AverageDuration = averageDuration;
            AverageThreats = averageThreats;
        }
    }
}
