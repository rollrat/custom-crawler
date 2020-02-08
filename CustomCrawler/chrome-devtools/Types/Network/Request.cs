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
    public class Request
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "urlFragment")]
        public string UrlFragment { get; set; }
        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }
        [JsonProperty(PropertyName = "headers")]
        public Dictionary<string, string> Headers { get; set; }
        [JsonProperty(PropertyName = "postData")]
        public string PostData { get; set; }
        [JsonProperty(PropertyName = "hasPostData")]
        public bool HasPostData { get; set; }
        [JsonProperty(PropertyName = "mixedContentType")]
        public string MixedContentType { get; set; }
        [JsonProperty(PropertyName = "initialPriority")]
        public string ResourcePriority { get; set; }
        [JsonProperty(PropertyName = "referrerPolicy")]
        public string ReferrerPolicy { get; set; }
        [JsonProperty(PropertyName = "isLinkPreload")]
        public bool IsLinkPreload { get; set; }
    }
}
