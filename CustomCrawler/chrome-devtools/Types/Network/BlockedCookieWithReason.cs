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
    public class BlockedCookieWithReason
    {
        [JsonProperty(PropertyName = "blockedReasons")]
        public string[] BlockedReasons { get; set; }
        [JsonProperty(PropertyName = "cookie")]
        public Cookie Cookie { get; set; }
    }
}
