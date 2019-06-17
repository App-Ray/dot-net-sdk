using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{

    [Serializable]
    public class AppRayException : Exception
    {
        public AppRayException() { }

        public AppRayException(string message) : base(message) { }

        public AppRayException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public AppRayException(string message, Exception inner) : base(message, inner) { }

        protected AppRayException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
