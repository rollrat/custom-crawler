// This source code is a part of Custom Copy Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using CefSharp.Wpf;
using MasterDevs.ChromeDevTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawlerDynamics.cefw
{
    public class Session
    {
        public static int Port;

        public static void Settings(ref CefSettings settings)
        {
            Port = FindFreePort();
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
            await chromeSession.SendAsync(new MasterDevs.ChromeDevTools.Protocol.Chrome.Debugger.EnableCommand
            {
                MaxScriptsCacheSize = 10000000
            });
            await chromeSession.SendAsync(new MasterDevs.ChromeDevTools.Protocol.Chrome.Target.SetAutoAttachCommand
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

        /// <summary>
        /// Source Code: https://github.com/SeleniumHQ/selenium/blob/master/dotnet/src/webdriver/Internal/PortUtilities.cs#L33
        /// 
        /// Finds a random, free port to be listened on.
        /// </summary>
        /// <returns>A random, free port to be listened on.</returns>
        public static int FindFreePort()
        {
            // Locate a free port on the local machine by binding a socket to
            // an IPEndPoint using IPAddress.Any and port 0. The socket will
            // select a free port.
            int listeningPort = 0;
            Socket portSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPEndPoint socketEndPoint = new IPEndPoint(IPAddress.Any, 0);
                portSocket.Bind(socketEndPoint);
                socketEndPoint = (IPEndPoint)portSocket.LocalEndPoint;
                listeningPort = socketEndPoint.Port;
            }
            finally
            {
                portSocket.Close();
            }

            return listeningPort;
        }

        public static void Dispose()
        {
            if (icp != null)
                icp.Dispose();
        }
    }
}
