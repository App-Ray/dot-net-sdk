using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class Licence
    {
        public string Reference { get; private set; }

        public int? Scans { get; private set; }

        public DateTime? Timestamp { get; private set; }

        public int? ValidityDays { get; private set; }

        internal Licence(string reference, int? scans, DateTime? timestamp, int? validityDays)
        {
            Reference = reference;
            Scans = scans;
            Timestamp = timestamp;
            ValidityDays = validityDays;
        }
    }
}