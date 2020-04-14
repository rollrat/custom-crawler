/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using CefSharp.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomCrawler.chrome_devtools
{
    public class ChromeDevtoolsListElement
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "devtoolsFrontendUrl")]
        public string DevtoolsFrontendUrl { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "webSocketDebuggerUrl")]
        public string WebSocketDebuggerUrl { get; set; }
    }

    public class ChromeDevtoolsResponse
    {
        public string RawMessage { get; set; }

        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }
        [JsonProperty(PropertyName = "result")]
        public object Result { get; set; }
        [JsonProperty(PropertyName = "method")]
        public object Method { get; set; }
        [JsonProperty(PropertyName = "params")]
        public object Params { get; set; }
        [JsonProperty(PropertyName = "error")]
        public object Error { get; set; }
    }

    public class ChromeDevtoolsOptions
    {
        public const string Network = "{\"id\":1,\"method\":\"Network.enable\",\"params\":{\"maxPostDataSize\":65536}}";
        public const string Debugger = "{\"id\":2,\"method\":\"Debugger.enable\",\"params\":{\"maxScriptsCacheSize\":10000000}}";
        public const string DOM = "{\"id\":3,\"method\":\"DOM.enable\",\"params\":{}}";
        public const string Target = "{\"id\":4,\"method\":\"Target.setAutoAttach\",\"params\":{\"autoAttach\":true,\"waitForDebuggerOnStart\":true,\"flatten\":true}}";
        public const string Runtime = "{\"id\":5,\"method\":\"Runtime.enable\"}";
        public const string RuntimeIWFD = "{\"id\":6,\"method\":\"Runtime.runIfWaitingForDebugger\"}";
        public const string Page = "{\"id\":7,\"method\":\"Page.enable\"}";
        public const string DebuggerACSD = "{\"id\":8,\"method\":\"Debugger.setAsyncCallStackDepth\",\"params\":{\"maxDepth\":32}}";
        public const string DOMStackTrace = "{\"id\":9,\"method\":\"DOM.setNodeStackTracesEnabled\",\"params\":{\"enable\":true}}";
    }

    /// <summary>
    /// This class is chrome devtools protocol wrapper.
    /// </summary>
    public class ChromeDevtoolsEnvironment : IDisposable
    {
        public static int Port = 8087;
        Timer timer;

        public static void Settings(ref CefSettings settings)
        {
            settings.RemoteDebuggingPort = Port;
        }

        private static long GetTime()
        {
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
            return (long)(t.TotalMilliseconds + 0.5);
        }

        public static List<ChromeDevtoolsListElement> GetDebuggeeList()
        {
            var list = NetCommon.DownloadString($"http://localhost:{Port}/json/list?t=" + GetTime());
            return JsonConvert.DeserializeObject<List<ChromeDevtoolsListElement>>(list);
        }

        public static ChromeDevtoolsEnvironment CreateInstance(ChromeDevtoolsListElement element)
        {
            return new ChromeDevtoolsEnvironment(element);
        }

        ChromeDevtoolsListElement target;
        ClientWebSocket wss;
        int id_count = 9;

        public ChromeDevtoolsEnvironment(ChromeDevtoolsListElement target_info)
        {
            target = target_info;

            wss = new ClientWebSocket();
            timer = new Timer(timer_callback, null, 0, 500);
        }

        private void timer_callback(object obj)
        {
            Task.Run(async () => await send($"{{\"id\":{Interlocked.Increment(ref id_count)},\"method\":\"DOM.getDocument\"}}"));
        }

        public async Task Connect()
        {
            await wss.ConnectAsync(new Uri(target.WebSocketDebuggerUrl), CancellationToken.None);
        }

        public async Task Option()
        {
            await send(ChromeDevtoolsOptions.Network);
            await send(ChromeDevtoolsOptions.Debugger);
            await send(ChromeDevtoolsOptions.DOM);
            await send(ChromeDevtoolsOptions.Target);
            await send(ChromeDevtoolsOptions.Runtime);
            await send(ChromeDevtoolsOptions.RuntimeIWFD);
            await send(ChromeDevtoolsOptions.Page);
            await send(ChromeDevtoolsOptions.DebuggerACSD);
            await send(ChromeDevtoolsOptions.DOMStackTrace);
        }

        Dictionary<string, List<object>> events = new Dictionary<string, List<object>>();
        HashSet<int> trigger_event = new HashSet<int>();
        Dictionary<int, ChromeDevtoolsResponse> request_lists = new Dictionary<int, ChromeDevtoolsResponse>();

        public async Task Start()
        {
            await Task.Run(async () =>
            {
                var construct = new StringBuilder();
                byte[] buffer = new byte[65535];
                while (wss.State == WebSocketState.Open)
                {
                    var result = await wss.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await wss.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    }
                    else
                    {
                        var content = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        construct.Append(content);

                        try
                        {
                            var rr = construct.ToString();
                            var response = JsonConvert.DeserializeObject<ChromeDevtoolsResponse>(rr);
                            response.RawMessage = rr;
                            construct.Clear();

                            if (response.Id != null && trigger_event.Contains(Convert.ToInt32(response.Id.ToString())))
                            {
                                lock (request_lists)
                                    request_lists.Add(Convert.ToInt32(response.Id.ToString()), response);
                                continue;
                            }

                            if (response.Method != null)
                            {
                                raise_events_condition(response.Method.ToString(), response.Params.ToString());
                            }
                            
                            // ignore other events
                        }
                        catch { }
                    }
                }
            });
        }

        private void raise_events_condition(string method_name, string param)
        {
            switch(method_name)
            {
                case Paused.Event:
                    raise_event("Paused", JsonConvert.DeserializeObject<Paused>(param));
                    break;
                case ChildNodeInserted.Event:
                    raise_event("ChildNodeInserted", JsonConvert.DeserializeObject<ChildNodeInserted>(param));
                    break;
                case DocumentUpdated.Event:
                    raise_event("DocumentUpdated", JsonConvert.DeserializeObject<DocumentUpdated>(param));
                    break;
                case RequestWillBeSent.Event:
                    raise_event("RequestWillBeSent", JsonConvert.DeserializeObject<RequestWillBeSent>(param));
                    break;
                case RequestWillBeSentExtraInfo.Event:
                    raise_event("RequestWillBeSentExtraInfo", JsonConvert.DeserializeObject<RequestWillBeSentExtraInfo>(param));
                    break;
                case ResponseReceived.Event:
                    raise_event("ResponseReceived", JsonConvert.DeserializeObject<ResponseReceived>(param));
                    break;
                case ResponseReceivedExtraInfo.Event:
                    raise_event("ResponseReceivedExtraInfo", JsonConvert.DeserializeObject<ResponseReceivedExtraInfo>(param));
                    break;
            }
        }

        private void raise_event<T>(string what, T obj)
        {
            if (events.ContainsKey(what))
            {
                var ll = events[what];
                ll.ForEach(x => (x as Action<T>).Invoke(obj));
            }
        }

        string[] events_list = new[] { "Paused", "ChildNodeInserted", "DocumentUpdated", "RequestWillBeSent", "RequestWillBeSentExtraInfo", "ResponseReceived", "ResponseReceivedExtraInfo" };

        public void Subscribe<T>(Action<T> callback)
        {
            foreach (var event_name in events_list)
            {
                if (typeof(T).Name == event_name)
                {
                    if (!events.ContainsKey(event_name))
                        events.Add(event_name, new List<object>());
                    events[event_name].Add(callback);
                }
            }
        }

        static Dictionary<Type, string> method_lists = new Dictionary<Type, string>
        {
            { typeof(Resume), Resume.Method },
            { typeof(Method.DOM.Enable), Method.DOM.Enable.Method },
            { typeof(Method.DOM.GetDocument), Method.DOM.GetDocument.Method },
            { typeof(Method.DOM.GetNodeStackTraces), Method.DOM.GetNodeStackTraces.Method },
            { typeof(RemoveDOMBreakpoint), RemoveDOMBreakpoint.Method },
            { typeof(SetDOMBreakpoint), SetDOMBreakpoint.Method },
            { typeof(Method.Network.Enable), Method.Network.Enable.Method },
            { typeof(Method.Network.GetCookies), Method.Network.GetCookies.Method },
        };

        private async Task send(string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            await wss.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task Send<T>(T param)
        {
            if (method_lists.ContainsKey(typeof(T)))
            {
                await send($"{{\"id\":{Interlocked.Increment(ref id_count)},\"method\":\"{method_lists[typeof(T)]}\",\"params\":{JsonConvert.SerializeObject(param)}}}");
            }
        }

        public async Task<ChromeDevtoolsResponse> Request<T>()
        {
            if (method_lists.ContainsKey(typeof(T)))
            {
                var id = Interlocked.Increment(ref id_count);
                trigger_event.Add(id);
                await send($"{{\"id\":{id},\"method\":\"{method_lists[typeof(T)]}\",\"params\":{{}}}}");
                return await wait_request(id);
            }
            return null;
        }

        public async Task<ChromeDevtoolsResponse> Request<T>(T param)
        {
            if (method_lists.ContainsKey(typeof(T)))
            {
                var id = Interlocked.Increment(ref id_count);
                trigger_event.Add(id);
                await send($"{{\"id\":{id},\"method\":\"{method_lists[typeof(T)]}\",\"params\":{JsonConvert.SerializeObject(param)}}}");
                return await wait_request(id);
            }
            return null;
        }

        private async Task<ChromeDevtoolsResponse> wait_request(int id)
        {
            return await Task.Run(() =>
            {
                // polling
                while (true)
                {
                    lock (request_lists)
                    {
                        if (request_lists.ContainsKey(id))
                        {
                            return request_lists[id];
                        }
                    }
                    Thread.Sleep(100);
                }
            });
        }

        public void PauseTimer()
        {
            timer.Change(int.MaxValue, int.MaxValue);
        }

        public void ResumeTimer()
        {
            timer.Change(0, 500);
        }

        public void Dispose()
        {
            if (wss != null)
            {
                try
                {
                    timer.Dispose();
                    wss.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).Wait();
                    wss = null;
                }
                catch
                { }
            }
        }
    }
}
