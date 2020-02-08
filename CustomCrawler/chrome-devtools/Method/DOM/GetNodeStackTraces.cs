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

namespace CustomCrawler.chrome_devtools.Method.DOM
{
    public class GetNodeStackTraces
    {
        public const string Method = "DOM.getNodeStackTraces";

        [JsonProperty(PropertyName = "nodeId")]
        public int NodeId { get; set; }
    }
}