using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppRaySDK.Utility;
using System.IO;
using System.Text.RegularExpressions;

namespace AppRaySDK
{
    public class Job
    {
        private static AppRayClient _client = new AppRayClient();

        public Guid? ID { get; private set; }

        internal Job(AppRayClient client, Guid? id)
        {
            _client = client;
            ID = id;
        }

        public JobDetails Get()
        {
            var endpoint = "jobs/" + ID;

            var response = _client.Request(endpoint, HttpMethod.Get);

            return ParseJobDetails(response);
        }

        public string Delete()
        {
            var endpoint = "jobs/" + ID;

            var response = _client.Request(endpoint, HttpMethod.Delete);

            return response;
        }

        public string DownloadReport()
        {
            var endpoint = "jobs/" + ID + "/details";

            var response = _client.Request(endpoint, HttpMethod.Get);

            return response;
        }

        public string DownloadScanArtifact()
        {
            var endpoint = "jobs/" + ID + "/result.zip";

            var response = _client.Request(endpoint, HttpMethod.Get);

            return response;
        }

        public List<Issue> GetIssues()
        {
            var result = new List<Issue>();

            var job = Get();
            var threatCategories = _client.GetThreatCategories();
            var threats = _client.GetThreats();

            foreach(var threatSummary in job.ThreatSummaries)
            {
                var matchingCategories = 
                    threatCategories.Where(x => x.ThreatIDs.Contains(threatSummary.ThreatID));

                var threat = threats.SingleOrDefault(x => x.ID == threatSummary.ThreatID);

                result.Add(new Issue(
                        matchingCategories.Select(x => x.Category),
                        threat.Confidence,
                        threatSummary.Description,
                        threatSummary.Explanation,
                        threat.ID,
                        threat.MoreAbout,
                        threat.Severity,
                        threat.Solution,
                        threat.Source
                    ));
            }

            return result;
        }

        public List<Communication> GetCommunications(bool onlyHttp = false)
        {
            var communications = new List<Communication>();

            var endpoint = "jobs/" + ID + "/details?path=/de.fhg.aisec.appray.detectors.UnmodifiedDynamicDetectorResult";

            var response = _client.Request(endpoint, HttpMethod.Get);

            JObject json = JObject.Parse(response);

            foreach(var comm in json["packets"])
            {
                long? contentLength;
                long contentLengthValue;

                contentLength = long.TryParse(comm["contentLength"].SafeValue<string>(), out contentLengthValue)
                    ? (long?)contentLengthValue : null;

                var contentType = comm["contentType"].SafeValue<string>();
                var destination = comm["destination"].SafeValue<string>();
                var destinationPort = comm["destinationPort"].SafeValue<int?>();
                var headers = comm["headers"].SafeValue<string>();
                var host = comm["host"].SafeValue<string>();
                var html = comm["html"].SafeValue<string>();
                var httpPacket = comm["httpPacket"].SafeValue<string>();
                var id = comm["id"].SafeValue<long?>();
                var informationType = "packets";
                var outgoing = comm["outgoing"].SafeValue<bool>();
                var payload = comm["payload"].SafeValue<string>();
                var referer = comm["referer"].SafeValue<string>();
                var request = comm["request"].SafeValue<bool>();
                var requestMethod = comm["requestMethod"].SafeValue<string>();
                var requestUrl = comm["requestURL"].SafeValue<string>();
                var responseValue = comm["response"].SafeValue<bool>();
                var server = comm["server"].SafeValue<string>();
                var source = comm["source"].SafeValue<string>();
                var sourcePort = comm["sourcePort"].SafeValue<int?>();

                var location = GetLocationFromIP(destination);

                var communication = new Communication(contentLength, contentType, destination, destinationPort, headers,
             host, html, httpPacket, id, informationType, outgoing, payload, referer,
             request, requestMethod, requestUrl, responseValue, server, source, sourcePort, location);

                communications.Add(communication);
            }

            foreach (var arrays in json["HTTP_flows"])
            {
                foreach (var comm in arrays)
                {
                    if(comm["destination"].SafeValue<string>() == json["emulator_ip"].SafeValue<string>())
                    {
                        comm["destination"] = comm["source"];
                        comm["destinationPort"] = comm["sourcePort"];
                        comm["requestURL"] = comm["requestURL"] + " response";
                    }

                    long? contentLength;
                    long contentLengthValue;

                    contentLength = long.TryParse(comm["contentLength"].SafeValue<string>(), out contentLengthValue)
                        ? (long?)contentLengthValue : null;

                    var contentType = comm["contentType"].SafeValue<string>();
                    var destination = comm["destination"].SafeValue<string>();
                    var destinationPort = comm["destinationPort"].SafeValue<int?>();
                    var headers = comm["headers"].SafeValue<string>();
                    var host = comm["host"].SafeValue<string>();
                    var html = comm["html"].SafeValue<string>();
                    var httpPacket = comm["httpPacket"].SafeValue<string>();
                    var id = comm["id"].SafeValue<long?>();
                    var informationType = "HTTP_flows";
                    var outgoing = comm["outgoing"].SafeValue<bool>();
                    var payload = comm["payload"].SafeValue<string>();
                    var referer = comm["referer"].SafeValue<string>();
                    var request = comm["request"].SafeValue<bool>();
                    var requestMethod = comm["requestMethod"].SafeValue<string>();
                    var requestUrl = comm["requestURL"].SafeValue<string>();
                    var responseValue = comm["response"].SafeValue<bool>();
                    var server = comm["server"].SafeValue<string>();
                    var source = comm["source"].SafeValue<string>();
                    var sourcePort = comm["sourcePort"].SafeValue<int?>();

                    var location = GetLocationFromIP(destination);

                    var communication = new Communication(contentLength, contentType, destination, destinationPort, headers,
                     host, html, httpPacket, id, informationType, outgoing, payload, referer,
                     request, requestMethod, requestUrl, responseValue, server, source, sourcePort, location);

                    communications.Add(communication);
                }
            }

            if(onlyHttp)
            {
                communications.RemoveAll(c => !c.Headers.Contains("Http"));
            }

            return communications;
        }

        public List<FileAccess> GetFileAccesses()
        {
            var endpoint = "jobs/" + ID + "/details?path=/de.fhg.aisec.appray.detectors.UnmodifiedDynamicDetectorResult/accessed_file_properties";

            var response = _client.Request(endpoint, HttpMethod.Get);

            JArray json = JArray.Parse(response);

            var result = new List<FileAccess>();

            foreach (var item in json)
            {
                var fileAccess = new FileAccess(
                        item["name"].SafeValue<string>(),
                        item["mode"].SafeValue<string>(),
                        item["access"].SafeValue<string>(),
                        item["size"].SafeValue<long?>()
                    );

                result.Add(fileAccess);
            }

            return result;
        }

        public List<VersionData> GetVersions()
        {
            var result = new List<VersionData>();

            var finishedJobs = GetFinishedJobs();
            var currentJob = Get();

            foreach(var job in finishedJobs)
            {
                if(job.Package == currentJob.Package && job.Platform == currentJob.Platform)
                {
                    JobDetails details = _client.Job(job.Uuid).Get();

                    result.Add(new VersionData(details.Version, details));
                }
            }

            result = result.OrderBy(
                x => Version.Parse(Regex.Replace(x.Version, "[a-zA-Z ]", ""))
                ).ToList();

            for (int i = 0; i < result.Count; i++)
            {
                if(i == 0)
                {
                    result[i].NewThreats.AddRange(
                        result[i].App.ThreatSummaries.Select(x => x.ThreatID)
                        );
                }
                else
                {
                    result[i].NewThreats.AddRange(
                        result[i].App.ThreatSummaries
                            .Select(x => x.ThreatID)
                            .Except(result[i - 1].App.ThreatSummaries
                                .Select(x => x.ThreatID))
                        );

                    result[i].FixedThreats.AddRange(
                        result[i-1].App.ThreatSummaries
                            .Select(x => x.ThreatID)
                            .Except(result[i].App.ThreatSummaries
                                .Select(x => x.ThreatID))
                        );
                }
            }

            return result;
        }

        public void RequestVirusScan()
        {
            var endpoint = "jobs/" + ID + "/virus-scan-request";

            var response = _client.Request(endpoint, HttpMethod.Get);
        }

        public VirusScanResult GetVirusScanResults()
        {
            var endpoint = "jobs/" + ID + "/virus-results";

            var response = _client.Request(endpoint, HttpMethod.Get);

            var json = JObject.Parse(response);

            var scanAllResultA = json["scan_all_result_a"].SafeValue<string>();
            var scanAllResultI = json["scan_all_result_i"].SafeValue<int?>();
            var startTime = json["start_time"].SafeValue<DateTime?>();
            var totalAVs =json["total_avs"].SafeValue<int?>();
            var totalDetectingAVs = json["total_detected_avs"].SafeValue<int?>();

            var detailedResults = new List<VirusScanDetails>();

            foreach(var item in json["scan_details"].Value<JObject>())
            {
                var antiVirusSoftware = item.Key;
                var defTime = item.Value["def_time"].SafeValue<DateTime?>();
                var scanResultI = item.Value["scan_result_i"].SafeValue<int?>();
                var scanTime = item.Value["scan_time"].SafeValue<int?>();
                var threatFound = item.Value["threat_found"].SafeValue<string>();

                var details = new VirusScanDetails(antiVirusSoftware, defTime, scanResultI, scanTime, threatFound);

                detailedResults.Add(details);
            }

            var result = new VirusScanResult(scanAllResultA, scanAllResultI, detailedResults,
                startTime, totalAVs, totalDetectingAVs);

            return result;
        }

        public static string Submit(string filePath)
        {
            var endpoint = "jobs";

            var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(new FileStream(filePath, FileMode.Open));

            content.Add(fileContent, "file", Path.GetFileName(filePath));

            var result = _client.Request(endpoint, HttpMethod.Post, content);

            return result;
        }

        public static List<JobResult> GetAllJobs()
        {
            var endpoint = "jobs?status=all";

            var response = _client.Request(endpoint, HttpMethod.Get);

            return ParseJobResults(response);
        }

        public static List<JobResult> GetFinishedJobs()
        {
            var endpoint = "jobs?status=finished";

            var response = _client.Request(endpoint, HttpMethod.Get);

            return ParseJobResults(response);
        }

        public static List<JobResult> GetQueuedJobs()
        {
            var endpoint = "jobs?status=queued";

            var response = _client.Request(endpoint, HttpMethod.Get);

            return ParseJobResults(response);
        }

        internal static List<JobResult> ParseJobResults(string json)
        {
            List<JobResult> result = new List<JobResult>();

            JArray jobs = JArray.Parse(json);

            foreach (var item in jobs)
            {
                var threatCounts = new ThreatCounts(
                        item["threat_counts"] != null
                            ? item["threat_counts"]["low"].SafeValue<int?>()
                            : null,
                        item["threat_counts"] != null
                            ? item["threat_counts"]["medium"].SafeValue<int?>()
                            : null,
                        item["threat_counts"] != null
                            ? item["threat_counts"]["high"].SafeValue<int?>()
                            : null
                    );

                var jobResult = new JobResult(
                        item["app_hash"].SafeValue<string>(),
                        item["label"].SafeValue<string>(),
                        item["package"].SafeValue<string>(),
                        item["platform"].SafeValue<string>(),
                        item["progress_finished"].SafeValue<int?>(),
                        item["progress_total"].SafeValue<int?>(),
                        item["risk_grade"].SafeValue<string>(),
                        item["risk_score"].SafeValue<int?>(),
                        item["scan_id"].SafeValue<string>() == null ? null : (Guid?)Guid.Parse(item["scan_id"].SafeValue<string>()),
                        item["status"].SafeValue<string>(),
                        item["submitter"].SafeValue<string>(),
                        threatCounts,
                        item["timestamp_scan_start"].SafeValue<long?>().ToUnixDateTime(),
                        item["timestamp_scan_finish"].SafeValue<long?>().ToUnixDateTime(),
                        item["timestamp_upload"].SafeValue<long?>().ToUnixDateTime(),
                        item["uuid"].SafeValue<string>() == null ? null : (Guid?)Guid.Parse(item["uuid"].SafeValue<string>()),
                        item["version"].SafeValue<string>()
                    );

                result.Add(jobResult);
            }

            return result;
        }

        internal static JobDetails ParseJobDetails(string json)
        {
            JobDetails result;

            JObject job = JObject.Parse(json);

            var threatCounts = new ThreatCounts(
                        job["threat_counts"] != null
                            ? job["threat_counts"]["low"].SafeValue<int?>()
                            : null,
                        job["threat_counts"] != null
                            ? job["threat_counts"]["medium"].SafeValue<int?>()
                            : null,
                        job["threat_counts"] != null
                            ? job["threat_counts"]["high"].SafeValue<int?>()
                            : null
                );

            List<ThreatLocation> threatLocations = new List<ThreatLocation>();
            List<ThreatSummary> threatSummaries = new List<ThreatSummary>();

            foreach (var item in job["threat_locations"].Value<JObject>())
            {
                List<string> locations = new List<string>();

                foreach(var location in item.Value)
                {
                    locations.Add(location.Value<string>());
                }

                threatLocations.Add(new ThreatLocation(item.Key, locations));
            }

            foreach(var item in job["threat_summaries"])
            {
                threatSummaries.Add(new ThreatSummary(
                        item["id"].SafeValue<string>(),
                        item["description"].SafeValue<string>(),
                        item["explanation"].SafeValue<string>()
                    ));
            }

            var scanErrors = new Dictionary<string, string>();

            foreach (var item in job["scan_errors"].Value<JObject>())
            {
                var key = item.Key;
                var value = item.Value.SafeValue<string>();

                scanErrors.Add(key, value);
            }

            result = new JobDetails(
                    job["app_hash"].SafeValue<string>(),
                    job["label"].SafeValue<string>(),
                    job["package"].SafeValue<string>(),
                    job["platform"].SafeValue<string>(),
                    job["progress_finished"].SafeValue<int?>(),
                    job["progress_total"].SafeValue<int?>(),
                    job["risk_grade"].SafeValue<string>(),
                    job["risk_score"].SafeValue<int?>(),
                    job["scan_duration"].SafeValue<long?>(),
                    scanErrors,
                    job["scan_id"].SafeValue<string>() == null ? null : (Guid?)Guid.Parse(job["scan_id"].SafeValue<string>()),
                    job["status"].SafeValue<string>(),
                    job["submitter"].SafeValue<string>(),
                    threatCounts,
                    threatLocations,
                    threatSummaries,
                    job["timestamp_scan_start"].SafeValue<long?>().ToUnixDateTime(),
                    job["timestamp_scan_finish"].SafeValue<long?>().ToUnixDateTime(),
                    job["timestamp_upload"].SafeValue<long?>().ToUnixDateTime(),
                    job["uuid"].SafeValue<string>() == null ? null : (Guid?)Guid.Parse(job["uuid"].SafeValue<string>()),
                    job["version"].SafeValue<string>());

            return result;
        }

        private GeoLocation GetLocationFromIP(string ip)
        {
            GeoLocation location;

            try
            {
                var geoResponse = _client.ThirdPartyRequest("https://freegeoip.net/json/" + ip);

                JObject geoJson = JObject.Parse(geoResponse);

                var ipResult = geoJson["ip"].SafeValue<string>();
                var countryCode = geoJson["country_code"].SafeValue<string>();
                var countryName = geoJson["country_name"].SafeValue<string>();
                var regionCode = geoJson["region_code"].SafeValue<string>();
                var regionName = geoJson["region_name"].SafeValue<string>();
                var city = geoJson["city"].SafeValue<string>();
                var zipCode = geoJson["zip_code"].SafeValue<string>();
                var timeZone = geoJson["time_zone"].SafeValue<string>();
                var latitude = geoJson["latitude"].SafeValue<double?>();
                var longitude = geoJson["longitude"].SafeValue<double?>();
                var metroCode = geoJson["metro_code"].SafeValue<int?>();

                location = new GeoLocation(ipResult, countryCode, countryName, regionCode,
                    regionName, city, zipCode, timeZone, latitude,
                    longitude, metroCode);
            }
            catch
            {
                // If calling 3rd party fails, just return null
                location = null;
            }

            return null;
        }
    }
}
