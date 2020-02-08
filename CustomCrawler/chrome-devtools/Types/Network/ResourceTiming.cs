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
    public class ResourceTiming
    {
        [JsonProperty(PropertyName = "requestTime")]
        public double RequestTime { get; set; }
        [JsonProperty(PropertyName = "proxyStart")]
        public double ProxyStart { get; set; }
        [JsonProperty(PropertyName = "proxyEnd")]
        public double ProxyEnd { get; set; }
        [JsonProperty(PropertyName = "dnsStart")]
        public double DnsStart { get; set; }
        [JsonProperty(PropertyName = "dnsEnd")]
        public double DnsEnd { get; set; }
        [JsonProperty(PropertyName = "connectStart")]
        public double ConnectStart { get; set; }
        [JsonProperty(PropertyName = "connectEnd")]
        public double ConnectEnd { get; set; }
        [JsonProperty(PropertyName = "sslStart")]
        public double SslStart { get; set; }
        [JsonProperty(PropertyName = "sslEnd")]
        public double SslEnd { get; set; }
        [JsonProperty(PropertyName = "workerStart")]
        public double WorkerStart { get; set; }
        [JsonProperty(PropertyName = "workerReady")]
        public double WorkerReady { get; set; }
        [JsonProperty(PropertyName = "sendStart")]
        public double SendStart { get; set; }
        [JsonProperty(PropertyName = "sendEnd")]
        public double SendEnd { get; set; }
        [JsonProperty(PropertyName = "pushStart")]
        public double PushStart { get; set; }
        [JsonProperty(PropertyName = "pushEnd")]
        public double PushEnd { get; set; }
        [JsonProperty(PropertyName = "receiveHeadersEnd")]
        public double ReceiveHeadersEnd { get; set; }
    }
}
