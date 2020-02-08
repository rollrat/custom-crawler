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

namespace CustomCrawler.chrome_devtools.Types.Debugger
{
    public class Scope
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        //[JsonProperty(PropertyName = "object")]
        //public Runtime.RemoteObject Object { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "startLocation")]
        public Location StartLocation { get; set; }
        [JsonProperty(PropertyName = "endLocation")]
        public Location EndLocation { get; set; }
    }
}
