/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using CustomCrawler.chrome_devtools.Types.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawler.chrome_devtools.Event.Network
{
    class ResponseReceivedExtraInfo
    {
        public const string Event = "Network.responseReceivedExtraInfo";

        [JsonProperty(PropertyName = "requestId")]
        public string RequestId { get; set; }
        [JsonProperty(PropertyName = "blockedCookies")]
        public BlockedCookieWithReason[] BlockedCookies { get; set; }
        [JsonProperty(PropertyName = "headers")]
        public Dictionary<string, string> Headers { get; set; }
        [JsonProperty(PropertyName = "headersText")]
        public string HeadersText { get; set; }
    }
}
