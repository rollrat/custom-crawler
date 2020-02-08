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
    public class GetDocument
    {
        public const string Method = "DOM.getDocument";

        [JsonProperty(PropertyName = "depth")]
        public int Depth { get; set; }
        [JsonProperty(PropertyName = "pierce")]
        public bool Pierce { get; set; }
    }
}
