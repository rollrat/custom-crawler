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

namespace CustomCrawler.chrome_devtools.Method.DOMDebugger
{
    public class SetDOMBreakpoint
    {
        public const string Method = "DOMDebugger.setDOMBreakpoint";

        [JsonProperty(PropertyName = "nodeId")]
        public int NodeId { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } // subtree-modified, attribute-modified, node-removed
    }
}
