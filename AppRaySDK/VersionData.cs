using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class VersionData
    {
        public string Version { get; private set; }

        public JobDetails App { get; private set; }

        public List<string> NewThreats { get; private set; }

        public List<string> FixedThreats { get; private set; }

        internal VersionData(string version, JobDetails app)
        {
            Version = version;
            App = app;
            NewThreats = new List<string>();
            FixedThreats = new List<string>();
        }
    }
}