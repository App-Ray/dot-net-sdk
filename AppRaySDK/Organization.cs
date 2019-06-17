using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AppRaySDK.Utility;

namespace AppRaySDK
{
    public class Organization
    {
        private AppRayClient _client = new AppRayClient();

        internal Organization(AppRayClient client)
        {
            _client = client;
        }

        public AccountInformation GetAccountInformation()
        {
            var endpoint = "user";

            var response = _client.Request(endpoint, HttpMethod.Get);

            var json = JObject.Parse(response);

            DateTime? created = json["created"].SafeValue<long?>().ToUnixDateTime();

            var result = new AccountInformation(
                    created,
                    json["email"].SafeValue<string>(),
                    json["name"].SafeValue<string>(),
                    json["role"].SafeValue<string>()
                );

            return result;
        }

        public CompanyInformation GetCompanyInformation()
        {
            var endpoint = "organization";

            var response = _client.Request(endpoint, HttpMethod.Get);

            var json = JObject.Parse(response);

            var created = json["created"].SafeValue<long?>().ToUnixDateTime();

            var employees = ((JArray)json["emails"]).Select(x => x.Value<string>());

            var result = new CompanyInformation(created, employees);

            return result;
        }

        public LicenceInformation GetLicenceInformation()
        {
            var endpoint = "organization";

            var response = _client.Request(endpoint, HttpMethod.Get);

            var json = JObject.Parse(response);

            var expiry = json["license_expiry"].SafeValue<long?>().ToUnixDateTime();

            var jsonLicences = (JArray)json["licenses"];
            var licences = new List<Licence>();

            foreach (var item in jsonLicences)
            {
                var timestamp = json["timestamp"].SafeValue<long?>().ToUnixDateTime();

                var licence = new Licence(
                        item["reference"].SafeValue<string>(),
                        item["scans"].SafeValue<int?>(),
                        timestamp,
                        item["validity_days"].SafeValue<int?>()
                    );

                licences.Add(licence);
            }

            var result = new LicenceInformation(
                    expiry,
                    json["license_total_scans"].SafeValue<int?>(),
                    json["license_used_scans"].SafeValue<int?>(),
                    licences
                );

            return result;
        }

        public ScanStatistics GetScanStatistics()
        {
            var endpoint = "jobs?status=all";

            var response = _client.Request(endpoint, HttpMethod.Get);

            var jobs = Job.ParseJobResults(response);

            var completed = jobs.Count(job => job.Status == "finished");
            var running = jobs.Count(job => job.Status == "processing");
            var queued = jobs.Count(job => job.Status == "queued");

            var averageDuration = jobs.Where(job => job.Status == "finished")
                .Average(job =>
                        TimeSpan.FromTicks(job.ScanFinish.Value.Ticks - job.ScanStart.Value.Ticks).TotalSeconds
                    );

            averageDuration = Math.Floor(averageDuration);

            var averageThreats = jobs.Where(job => job.Status == "finished")
                .Average(job =>
                        (double)(job.ThreatCounts.Total)
                    );

            averageThreats = Math.Floor(averageThreats);

            var result = new ScanStatistics(completed, running, queued,
                Convert.ToInt32(averageDuration), Convert.ToInt32(averageThreats));

            return result;
        }
    }
}
