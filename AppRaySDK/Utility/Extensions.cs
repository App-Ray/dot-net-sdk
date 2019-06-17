using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK.Utility
{
    public static class Extensions
    {
        public static DateTime? ToUnixDateTime(this long? seconds)
        {
            if(seconds.HasValue)
            {
                return DateTimeOffset
                        .FromUnixTimeSeconds(seconds.Value)
                        .DateTime;
            }

            return null;
        }

        public static T SafeValue<T>(this JValue jValue)
        {
            return jValue?.Value<string>() == null
                ? default(T) : jValue.Value<T>();
        }

        public static T SafeValue<T>(this JToken jToken)
        {
            return jToken?.Value<string>() == null
                ? default(T) : jToken.Value<T>();
        }

        public static T SafeValue<T>(this JObject jObject)
        {
            return jObject?.Value<string>() == null
                ? default(T) : jObject.Value<T>();
        }
    }
}
