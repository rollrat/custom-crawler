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

namespace CustomCrawler.chrome_devtools.Types.Runtime
{
    public class CallFrame
    {
        [JsonProperty(PropertyName = "functionName")]
        public string FunctionName { get; set; }
        [JsonProperty(PropertyName = "scriptId")]
        public string ScriptId { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "lineNumber")]
        public long LineNumber { get; set; }
        [JsonProperty(PropertyName = "columnNumber")]
        public long ColumnNumber { get; set; }
    }
}
