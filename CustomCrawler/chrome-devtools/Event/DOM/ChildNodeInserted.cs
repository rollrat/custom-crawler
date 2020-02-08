/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using CustomCrawler.chrome_devtools.Types.DOM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawler.chrome_devtools.Event.DOM
{
    public class ChildNodeInserted
    {
        public const string Event = "DOM.childNodeInserted";

        [JsonProperty(PropertyName = "parentNodeId")]
        public int ParentNodeId { get; set; }
        [JsonProperty(PropertyName = "previousNodeId")]
        public int PreviousNodeId { get; set; }
        [JsonProperty(PropertyName = "node")]
        public Node Node { get; set; }
    }
}
