using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AppRaySDK.Utility;

namespace AppRaySDK
{
    /// <summary>
    /// Responsible for logging in and out of the AppRay application, storing login
    /// and session information and handling direct HTTP requests.
    /// </summary>
    public class AppRayClient
    {
        /// <summary>
        /// Inner HTTP client for handling different HTTP requests.
        /// </summary>
        private static HttpClient _httpClient = new HttpClient();

        private string _version = string.Empty;

        public string Version
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_version))
                {
                    Ping();
                }
                return _version;
            }
        }

        /// <summary>
        /// Gets the authorization token of the current session. Empty if logged out.
        /// </summary>
        public string Token { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the initial time (seconds) the current session will expire in.
        /// </summary>
        public long? LoginExpiresIn { get; private set; }

        /// <summary>
        /// Gets the date and time of the last login from the current user.
        /// </summary>
        public DateTime? LastLoginTime { get; private set; }

        /// <summary>
        /// Gets the IP address of the device from which the current user last logged in.
        /// </summary>
        public IPAddress LastLoginIP { get; private set; }

        /// <summary>
        /// Gets a value indicating the successfulness of the last login attempt.
        /// </summary>
        public bool LastLoginSuccessful { get; private set; }

        /// <summary>
        /// Gets the type of the authorization token for the current session.
        /// </summary>
        public string TokenType { get; private set; }

        public string ApiUrl
        {
            get
            {
                Uri host = new Uri(AppRaySDKConfiguration.HostUrl);
                return new Uri(host, string.Format("api/v{0}/", _version)).ToString();
            }
        }

        /// <summary>
        /// Gets the login API URL for login requests.
        /// </summary>
        public string LoginUrl
        {
            get
            {
                Uri api = new Uri(ApiUrl);
                return new Uri(api, "authentication").ToString();
            }
        }

        /// <summary>
        /// Gets the version API URL.
        /// </summary>
        public string VersionUrl
        {
            get
            {
                Uri host = new Uri(AppRaySDKConfiguration.HostUrl);
                return new Uri(host, "api/version").ToString();
            }
        }

        /// <summary>
        /// Gets the organization API URL.
        /// </summary>
        public string OrganizationUrl
        {
            get
            {
                Uri api = new Uri(ApiUrl);
                return new Uri(api, "organization").ToString();
            }
        }

        /// <summary>
        /// Gets the job API URL.
        /// </summary>
        public string JobsUrl
        {
            get
            {
                Uri api = new Uri(ApiUrl);
                return new Uri(api, "jobs").ToString();
            }
        }

        public AppRayClient()
        {
            Ping(); // Initialize the version number.
        }

        /// <summary>
        /// Performs a login using the authentication infomration
        /// provided in the configuration file.
        /// </summary>
        /// <exception cref="AppRayException">
        /// Indicates that something was wrong with the request (e.g. authentication failed).
        /// </exception>
        public void Login()
        {
            try
            {
                Dictionary<string, string> values = new Dictionary<string, string>()
                {
                    { "username", AppRaySDKConfiguration.Username },
                    { "password", AppRaySDKConfiguration.Password },
                    { "grant_type", "password"}
                };

                HttpContent content = new FormUrlEncodedContent(values);

                var response = _httpClient.PostAsync(LoginUrl, content).Result;

                var responseString = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = JObject.Parse(responseString);

                    Token = json["access_token"].SafeValue<string>();
                    TokenType = json["token_type"].SafeValue<string>();

                    LastLoginTime = json["last_login_time"].SafeValue<long?>().ToUnixDateTime();
                    LastLoginSuccessful = json["last_login_successful"].SafeValue<bool>();
                    LastLoginIP = IPAddress.Parse(json["last_login_ip"].SafeValue<string>());
                    LoginExpiresIn = json["expires_in"].SafeValue<long?>();
                }
                else
                {
                    throw new AppRayException("User authentication failed. Status code: {0}", response.StatusCode);
                }
            }
            catch (Exception exception)
            {
                throw new AppRayException("An error occured during login. See inner exception for details", exception);
            }
        }

        /// <summary>
        /// Performs a logout.
        /// </summary>
        public void Logout()
        {
            Token = string.Empty;
        }

        /// <summary>
        /// Pings the application server.
        /// </summary>
        /// <returns>
        /// The elapsed time between the request and the arrival of the response
        /// in milliseconds. A null value indicates that the server did not respond.
        /// </returns>
        public double? Ping()
        {
            double? result;

            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var response = _httpClient.GetAsync(VersionUrl).Result;
                sw.Stop();
                result = sw.ElapsedMilliseconds;

                var responseString = response.Content.ReadAsStringAsync().Result;

                JObject json = JObject.Parse(responseString);
                _version = json["api_version"].SafeValue<string>();
            }
            catch
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Performs a request to the specified endpoint of the application using the given
        /// HTTP method and passing the given HTTP content.
        /// </summary>
        /// <param name="endpoint">The endpoint of the application.</param>
        /// <param name="method">HTTP method used for the request.</param>
        /// <param name="content">HTTP content of the request if needed.</param>
        /// <returns>The response from the application server.</returns>
        public string Request(string endpoint, HttpMethod method, HttpContent content = null)
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                Login();
            }

            try
            {
                if(endpoint.StartsWith("/"))
                {
                    endpoint = endpoint.Substring(1);
                }

                Uri host = new Uri(ApiUrl);
                var url = new Uri(host, endpoint).ToString();

                HttpRequestMessage request = new HttpRequestMessage(method, url);
                request.Content = content;
                request.Headers.Add("Authorization",
                    string.Format("{0} {1}", TokenType, Token));

                var response = _httpClient.SendAsync(request).Result;

                if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Login();
                    response = _httpClient.SendAsync(request).Result;
                }

                if(!response.IsSuccessStatusCode)
                {
                    throw new AppRayException("HTTP Request unsuccessful. Status code: {0}", response.StatusCode);
                }

                var responseString = response.Content.ReadAsStringAsync().Result;

                return responseString;
            }
            catch (Exception exception)
            {
                throw new AppRayException("There was an error during the request. See inner exception for details.", exception);
            }
        }

        internal string ThirdPartyRequest(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = _httpClient.SendAsync(request).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new AppRayException("HTTP Request unsuccessful. Status code: {0}", response.StatusCode);
            }

            var responseString = response.Content.ReadAsStringAsync().Result;

            return responseString;
        }

        /// <summary>
        /// Instantiates an object that handles job related requests to the application server.
        /// </summary>
        /// <param name="id">The ID of the job.</param>
        /// <returns>The object handling job related requests.</returns>
        public Job Job(Guid? id)
        {
            return new Job(this, id);
        }

        /// <summary>
        /// Instantiates an object that handles organization related requests to the application server.
        /// </summary>
        /// <returns>The object handling organization related requests.</returns>
        public Organization Organization()
        {
            return new Organization(this);
        }

        public List<Threat> GetThreats()
        {
            var endpoint = "../threats.json";

            var response = Request(endpoint, HttpMethod.Get);

            JObject json = JObject.Parse(response);

            var result = new List<Threat>();

            foreach (var item in json)
            {
                result.Add(new Threat(
                        item.Value["id"].SafeValue<string>(),
                        item.Value["severity"].SafeValue<string>(),
                        item.Value["source"].SafeValue<string>(),
                        item.Value["confidence"].SafeValue<string>(),
                        item.Value["moreabout"].SafeValue<string>(),
                        item.Value["solution"].SafeValue<string>()
                    ));
            }

            return result;
        }

        public List<ThreatCategory> GetThreatCategories()
        {
            var endpoint = "../threat-categories.json";

            var response = Request(endpoint, HttpMethod.Get);

            JObject json = JObject.Parse(response);

            var result = new List<ThreatCategory>();

            foreach (var mainCategory in json)
            {
                foreach(var category in (JObject)mainCategory.Value)
                {
                        result.Add(new ThreatCategory(
                            mainCategory.Key,
                            category.Key,
                            category.Value.Values<string>().ToList()
                        ));
                }
            }

            return result;
        }
    }
}
