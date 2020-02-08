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
    public class CallFrame
    {
        [JsonProperty(PropertyName = "callFrameId")]
        public string CallFrameId { get; set; }
        [JsonProperty(PropertyName = "functionName")]
        public string FunctionName { get; set; }
        [JsonProperty(PropertyName = "functionLocation")]
        public Location FunctionLocation { get; set; }
        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Uurl { get; set; }
        [JsonProperty(PropertyName = "scopeChain")]
        public Scope[] ScopeChain { get; set; }
        //[JsonProperty(PropertyName = "this")]
        //public Runtime.RemoteObject This { get; set; }
        //[JsonProperty(PropertyName = "returnValue")]
        //public Runtime.RemoteObject ReturnValue { get; set; }
    }
}
