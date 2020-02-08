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

namespace CustomCrawler.chrome_devtools.Types.DOM
{
    public class BackendNode
    {
        [JsonProperty(PropertyName = "nodeType")]
        public int NodeType { get; set; }
        [JsonProperty(PropertyName = "nodeName")]
        public string NodeName { get; set; }
        [JsonProperty(PropertyName = "backendNodeId")]
        public int BackendNodeId { get; set; }
    }
}
