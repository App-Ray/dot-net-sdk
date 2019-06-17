using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class GeoLocation
    {
        public string IP { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string RegionCode { get; set; }

        public string RegionName { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string TimeZone { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public int? MetroCode { get; set; }

        internal GeoLocation(string ip, string countryCode, string countryName, string regionCode,
            string regionName, string city, string zipCode, string timeZone, double? latitude,
            double? longitude, int? metroCode)
        {
            IP = ip;
            CountryCode = countryCode;
            CountryName = countryName;
            RegionCode = regionCode;
            RegionName = regionName;
            City = city;
            ZipCode = zipCode;
            TimeZone = timeZone;
            Latitude = latitude;
            Longitude = longitude;
            MetroCode = metroCode;
        }
    }
}
