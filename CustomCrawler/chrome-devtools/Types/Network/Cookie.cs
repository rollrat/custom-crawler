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
    public class Cookie
    {
        [JsonProperty(PropertyName = "name")]
        public string RequestTime { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
        [JsonProperty(PropertyName = "expires")]
        public double Expires { get; set; }
        [JsonProperty(PropertyName = "size")]
        public int Size { get; set; }
        [JsonProperty(PropertyName = "httpOnly")]
        public bool HttpOnly { get; set; }
        [JsonProperty(PropertyName = "secure")]
        public bool Secure { get; set; }
        [JsonProperty(PropertyName = "session")]
        public bool Session { get; set; }
        [JsonProperty(PropertyName = "sameSite")]
        public string SameSite { get; set; }
        [JsonProperty(PropertyName = "priority")]
        public string Priority { get; set; }
    }
}
