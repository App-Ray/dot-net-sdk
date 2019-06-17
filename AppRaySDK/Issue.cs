using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class Issue
    {
        public List<string> Categories { get; private set; }

        public string Confidence { get; private set; }

        public string Description { get; private set; }

        public string Explanation { get; private set; }

        public string ThreatID { get; private set; }

        public string MoreAbout { get; private set; }

        public string Severity { get; private set; }

        public string Solution { get; private set; }

        public string Source { get; private set; }

        public Issue(IEnumerable<string> categories, string confidence, string description, string explanation,
            string threatID, string moreAbout, string severity,
            string solution, string source)
        {
            Categories = categories.ToList();
            Confidence = confidence;
            Description = description;
            Explanation = explanation;
            ThreatID = threatID;
            MoreAbout = moreAbout;
            Severity = severity;
            Solution = solution;
            Source = source;
        }
    }
}