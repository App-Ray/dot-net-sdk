using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class Threat
    {
        public string ID { get; private set; }

        public string Severity { get; private set; }

        public string Source { get; private set; }

        public string Confidence { get; private set; }

        public string MoreAbout { get; private set; }

        public string Solution { get; private set; }

        public Threat(string id, string severity, string source, string confidence,
            string moreAbout, string solution)
        {
            ID = id;
            Severity = severity;
            Source = source;
            Confidence = confidence;
            MoreAbout = moreAbout;
            Solution = solution;
        }
    }
}
