/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using CefSharp;
using CefSharp.Wpf;
using CustomCrawler.chrome_devtools;
using CustomCrawler.chrome_devtools.Method.Debugger;
using CustomCrawler.chrome_devtools.Method.DOM;
using CustomCrawler.chrome_devtools.Method.DOMDebugger;
using CustomCrawler.chrome_devtools.Types.DOM;
using CustomCrawler.chrome_devtools.Types.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CustomCrawler
{
    /// <summary>
    /// CustomCrawlerDynamics.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomCrawlerDynamics : Window
    {
        ChromiumWebBrowser browser;

        public CustomCrawlerDynamics()
        {
            InitializeComponent();

            browser = new ChromiumWebBrowser(string.Empty);
            browserContainer.Content = browser;

            browser.LoadingStateChanged += Browser_LoadingStateChanged;

            Closed += CustomCrawlerDynamics_Closed;
        }

        private async void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading && env != null)
            {
                var doc = await env.Request(new GetDocument { Depth = -1 });
                var root_node = JsonConvert.DeserializeObject<Node>(JObject.Parse(doc.Result.ToString())["root"].ToString());
                env.PauseTimer();
                _ = find_source(root_node);
            }
        }

        private async Task find_source(Node nn)
        {
            if (nn.Children != null)
            {
                _ = Application.Current.Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    URLText.Text = nn.NodeId.ToString();
                }));
                try
                {
                    var st = await env.Request(new GetNodeStackTraces { NodeId = nn.NodeId });
                    if (st.Result != null)
                        ;
                    var ff = JsonConvert.DeserializeObject<StackTrace>(JObject.Parse(st.Result.ToString())["creation"].ToString());
                    MessageBox.Show(JObject.Parse(st.Result.ToString())["creation"].ToString());
                } catch (Exception ex) {
                    if (ex.Message.Contains("WebSocket"))
                        ;
                }
                foreach (var child in nn.Children)
                {
                    await find_source(child);
                }
            }
        }

        private void CustomCrawlerDynamics_Closed(object sender, EventArgs e)
        {
            if (env != null)
                env.Dispose();
        }

        ChromeDevtoolsEnvironment env;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (env == null)
            {
                var token = new Random().Next();
                browser.LoadHtml(token.ToString());

                var target = ChromeDevtoolsEnvironment.GetDebuggeeList().Where(x => x.Url == $"data:text/html,{token}");
                env = ChromeDevtoolsEnvironment.CreateInstance(target.First());
                new CustomCrawlerDynamicsRequest(env).Show();

                await env.Connect();
                await env.Option();

                _ = Task.Run(async () => { await env.Start(); });
            }

            browser.Load(URLText.Text);
        }
    }
}
