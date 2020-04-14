/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using CefSharp.Wpf;
using MasterDevs.ChromeDevTools;
using MasterDevs.ChromeDevTools.Protocol.Chrome.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawler.chrome_devtools
{
    public class ChromeDevTools
    {
        public static int Port = 8086;

        public static void Settings(ref CefSettings settings)
        {
            settings.RemoteDebuggingPort = Port;
        }

        static IChromeProcess icp;

        public static async Task<IChromeSession> Create()
        {
            var chromeProcessFactory = new CefFactory();
            icp = chromeProcessFactory.Create(Port, true);

            var sessionInfo = (await icp.GetSessionInfo()).LastOrDefault();
            var chromeSessionFactory = new ChromeSessionFactory();
            var chromeSession = chromeSessionFactory.Create(sessionInfo.WebSocketDebuggerUrl);

            await chromeSession.SendAsync(new MasterDevs.ChromeDevTools.Protocol.Chrome.Network.EnableCommand
            {
                MaxPostDataSize = 65536
            });
            await chromeSession.SendAsync<MasterDevs.ChromeDevTools.Protocol.Chrome.Page.EnableCommand>();
            await chromeSession.SendAsync<MasterDevs.ChromeDevTools.Protocol.Chrome.DOM.EnableCommand>();
            await chromeSession.SendAsync( new MasterDevs.ChromeDevTools.Protocol.Chrome.Debugger.EnableCommand
            {
                MaxScriptsCacheSize = 10000000
            });
            await chromeSession.SendAsync( new MasterDevs.ChromeDevTools.Protocol.Chrome.Target.SetAutoAttachCommand
            {
                AutoAttach = true,
                WaitForDebuggerOnStart = true,
                Flatten = true,
            });
            await chromeSession.SendAsync<MasterDevs.ChromeDevTools.Protocol.Chrome.Runtime.EnableCommand>();
            await chromeSession.SendAsync<MasterDevs.ChromeDevTools.Protocol.Chrome.Runtime.RunIfWaitingForDebuggerCommand>();
            await chromeSession.SendAsync(new MasterDevs.ChromeDevTools.Protocol.Chrome.Debugger.SetAsyncCallStackDepthCommand
            {
                MaxDepth = 32
            });
            await chromeSession.SendAsync(new MasterDevs.ChromeDevTools.Protocol.Chrome.DOM.SetNodeStackTracesEnabledCommand 
            {
                Enable = true,
            });

            return chromeSession;
        }

        public static void Dispose()
        {
            if (icp != null)
                icp.Dispose();
        }
    }
}
