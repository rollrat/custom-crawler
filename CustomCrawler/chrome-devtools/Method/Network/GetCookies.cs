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

namespace CustomCrawler.chrome_devtools.Method.Network
{
    public class GetCookies
    {
        public const string Method = "Network.getCookies";

        [JsonProperty(PropertyName = "urls")]
        public string[] Urls;
        [JsonProperty(PropertyName = "cookies")]
        public Cookie[] Cookies;
    }
}
