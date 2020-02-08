/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawler.chrome_devtools.Types.Network
{
    public class Response
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        [JsonProperty(PropertyName = "statusText")]
        public string StatusText { get; set; }
        [JsonProperty(PropertyName = "headers")]
        public Dictionary<string, string> Headers { get; set; }
        [JsonProperty(PropertyName = "headersText")]
        public string HeadersText { get; set; }
        [JsonProperty(PropertyName = "mimeType")]
        public string MimeType { get; set; }
        [JsonProperty(PropertyName = "requestHeaders")]
        public Dictionary<string, string> RequestHeaders { get; set; }
        [JsonProperty(PropertyName = "requestHeadersText")]
        public string RequestHeadersText { get; set; }
        [JsonProperty(PropertyName = "connectionReused")]
        public bool ConnectionReused { get; set; }
        [JsonProperty(PropertyName = "connectionId")]
        public long ConnectionId { get; set; }
        [JsonProperty(PropertyName = "remoteIPAddress")]
        public string RemoteIPAddress { get; set; }
        [JsonProperty(PropertyName = "remotePort")]
        public int RemotePort { get; set; }
        [JsonProperty(PropertyName = "fromDisckCache")]
        public bool FromDisckCache { get; set; }
        [JsonProperty(PropertyName = "fromServiceWorker")]
        public bool FromServiceWorker { get; set; }
        [JsonProperty(PropertyName = "fromPrefetchCache")]
        public bool FromPrefetchCache { get; set; }
        [JsonProperty(PropertyName = "encodedDataLength")]
        public long EncodedDataLength { get; set; }
        [JsonProperty(PropertyName = "timing")]
        public ResourceTiming Timing { get; set; }
        [JsonProperty(PropertyName = "protocol")]
        public string Protocol { get; set; }
        [JsonProperty(PropertyName = "securityState")]
        public string SecurityState { get; set; }
        [JsonProperty(PropertyName = "securityDetails")]
        public SecurityDetails SecurityDetails { get; set; }
    }
}
