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
    public class Location
    {
        [JsonProperty(PropertyName = "scriptId")]
        public string ScriptId { get; set; }
        [JsonProperty(PropertyName = "lineNumber")]
        public int LineNumber { get; set; }
        [JsonProperty(PropertyName = "columnNumber")]
        public int ColumnNumber { get; set; }
    }
}
