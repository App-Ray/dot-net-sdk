using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class LicenceInformation
    {
        public DateTime? Expiry { get; private set; }

        public int? TotalScans { get; private set; }

        public int? UsedScans { get; private set; }

        public List<Licence> Licences { get; private set; }

        internal LicenceInformation(DateTime? expiry, int? totalScans, int? usedScans, List<Licence> licences)
        {
            Expiry = expiry;
            TotalScans = totalScans;
            UsedScans = usedScans;
            Licences = licences ?? throw new ArgumentNullException(nameof(licences));
        }
    }
}