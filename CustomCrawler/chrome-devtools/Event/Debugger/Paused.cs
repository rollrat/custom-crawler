/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using CustomCrawler.chrome_devtools.Types.Debugger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawler.chrome_devtools.Event.Debugger
{
    public class Paused
    {
        public const string Event = "Debugger.paused";

        [JsonProperty(PropertyName = "callFrames")]
        public CallFrame[] CallFrames { get; set; }
        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }
        [JsonProperty(PropertyName = "hitBreakpoints")]
        public string[] HitBreakpoints { get; set; }
        [JsonProperty(PropertyName = "asyncStackTrace")]
        public Types.Runtime.StackTrace AsyncStackTrace { get; set; }
        //[JsonProperty(PropertyName = "asyncStackTraceId")]
        //public string AsyncStackTraceId { get; set; }
        //[JsonProperty(PropertyName = "asyncCallStackTraceId")]
        //public string AsyncCallStackTraceId { get; set; }
    }
}
