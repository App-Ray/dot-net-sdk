using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class Communication
    {
        public long? ContentLength { get; set; }

        public string ContentType { get; set; }

        public string Destination { get; set; }

        public int? DestinationPort { get; set; }

        public string Headers { get; set; }

        public string Host { get; set; }

        public string Html { get; set; }

        public string HttpPacket { get; set; }

        public long? ID { get; set; }

        public string InformationType { get; set; }

        public bool Outgoing { get; set; }

        public string Payload { get; set; }

        public string Referer { get; set; }

        public bool Request { get; set; }

        public string RequestMethod { get; set; }

        public string RequestUrl { get; set; }

        public bool Response { get; set; }

        public string Server { get; set; }

        public string Source { get; set; }

        public int? SourcePort { get; set; }

        public GeoLocation Location { get; set; }

        internal Communication(long? contentLength, string contentType, string destination, int? destinationPort, string headers,
            string host, string html, string httpPacket, long? id, string informationType, bool outgoing, string payload, string referer,
            bool request, string requestMethod, string requestUrl, bool response, string server, string source, int? sourcePort, GeoLocation location)
        {
            ContentLength = contentLength;
            ContentType = contentType;
            Destination = destination;
            DestinationPort = destinationPort;
            Headers = headers;
            Host = host;
            Html = html;
            HttpPacket = httpPacket;
            ID = id;
            InformationType = informationType;
            Outgoing = outgoing;
            Payload = payload;
            Referer = referer;
            Request = request;
            RequestMethod = requestMethod;
            RequestUrl = requestUrl;
            Response = response;
            Server = server;
            Source = source;
            SourcePort = sourcePort;
        }
    }
}
