using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class FileAccess
    {
        public string FileName { get; private set; }

        public string AccessMode { get; private set; }

        public string Permissions { get; private set; }

        public long? FileSize { get; private set; }

        internal FileAccess(string fileName, string accessMode, string permissions, long? fileSize)
        {
            FileName = fileName;
            AccessMode = accessMode;
            Permissions = permissions;
            FileSize = fileSize;
        }
    }
}
