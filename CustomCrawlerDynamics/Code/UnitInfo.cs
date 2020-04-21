// This source code is a part of Custom Copy Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using MasterDevs.ChromeDevTools.Protocol.Chrome.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawlerDynamics.Code
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitInfo
    {
        public List<UnitInfo> Parent { get; set; } = new List<UnitInfo>();
        public string Id { get; set; }
        public string Url { get; protected set; }
        public List<RequestWillBeSentEvent> Request { get; set; } = new List<RequestWillBeSentEvent>();
        public List<ResponseReceivedEvent> Response { get; set; } = new List<ResponseReceivedEvent>();
    }
}
