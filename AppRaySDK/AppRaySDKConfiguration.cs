using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AppRaySDK
{
    public static class AppRaySDKConfiguration
    {
        private static string _username = string.Empty;

        private static string _password = string.Empty;

        private static string _hostUrl = string.Empty;
        
        public static string Username
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_username))
                {
                    var key = nameof(Username);
                    var setting = ConfigurationManager.AppSettings[key];

                    if (string.IsNullOrWhiteSpace(setting))
                    {
                        throw new AppRayException("Username not provided. Please provide a value or add the key {0} to the application configuration file.", key);
                    }

                    _username = setting;
                }

                return _username;
            }
            set
            {
                _username = value;
            }
        }

        public static string Password
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_password))
                {
                    var key = nameof(Password);
                    var setting = ConfigurationManager.AppSettings[key];

                    if (string.IsNullOrWhiteSpace(setting))
                    {
                        throw new AppRayException("Password not provided. Please provide a value or add the key {0} to the application configuration file.", key);
                    }

                    _password = setting;
                }

                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public static string HostUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_hostUrl))
                {
                    var key = nameof(HostUrl);
                    var setting = ConfigurationManager.AppSettings[key];

                    if (string.IsNullOrWhiteSpace(setting))
                    {
                        throw new AppRayException("Host URL not provided. Please provide a value or add the key {0} to the application configuration file.", key);
                    }

                    _hostUrl = setting;
                }

                return _hostUrl;
            }
            set
            {
                _hostUrl = value;
            }
        }
    }
}
