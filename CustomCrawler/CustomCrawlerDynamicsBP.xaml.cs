/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using MasterDevs.ChromeDevTools;
using MasterDevs.ChromeDevTools.Protocol.Chrome.Debugger;
using MasterDevs.ChromeDevTools.Protocol.Chrome.Page;
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
    /// CustomCrawlerDynamicsBP.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomCrawlerDynamicsBP : Window
    {
        CustomCrawlerDynamics parent;
        public CustomCrawlerDynamicsBP(CustomCrawlerDynamics parent)
        {
            InitializeComponent();

            this.parent = parent;
            Loaded += CustomCrawlerDynamicsBP_Loaded;
        }

        static Dictionary<string, (string, Location[])> break_points;
        static int bp_count;
        static int paused_count;
        static List<PausedEvent> paused = new List<PausedEvent>();
        //static List<(ScriptParsedEvent, string)> anonymous_scripts = new List<(ScriptParsedEvent, string)>();

        private void CustomCrawlerDynamicsBP_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                process_install_bp().Wait();
            });
        }

        public static bool ignore_js(string url)
        {
            var filename = url.Split('?')[0].Split('/').Last();

            if (filename.StartsWith("jquery"))
                return true;

            return false;
        }
        
        private async Task process_install_bp()
        {
            var cc = parent.scripts.Where(x => !string.IsNullOrEmpty(x.Url) && !ignore_js(x.Url)).ToList();

            var tasks = new List<(int, Esprima.Location)>();
            var urls = cc.Select(x => x.Url).ToList();
            urls.Sort();
            break_points = new Dictionary<string, (string, Location[])>();


            for (int i = 0; i < cc.Count; i++)
            {
                var ss = cc[i];
                var funcs = JsManager.Instance.EnumerateFunctionEntries(ss.Url);

                foreach (var func in funcs)
                {
                    tasks.Add((i, func.Body.Location));
                }
            }

            await Application.Current.Dispatcher.BeginInvoke(new Action(
            delegate
            {
                S2.Content = $"Set Break Points: 0/{tasks.Count}";
                P2.Maximum = tasks.Count;
            }));

            var ii = 0;

            foreach (var yy in tasks)
            {
                var rr = await CustomCrawlerDynamics.ss.SendAsync(new SetBreakpointByUrlCommand
                {
                    LineNumber = yy.Item2.Start.Line - 1,
                    ColumnNumber = yy.Item2.Start.Column + 1,
                    Url = cc[yy.Item1].Url,
                });
                await Application.Current.Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    S2.Content = $"Set Break Points: {++ii}/{tasks.Count}";
                    P2.Value += 1;
                }));
                if (rr.Result != null)
                {
                    lock (break_points)
                        break_points.Add(rr.Result.BreakpointId, (cc[yy.Item1].Url, rr.Result.Locations));
                }
            }

            CustomCrawlerDynamics.ss.Subscribe<BreakpointResolvedEvent>(x =>
            {
                var y = x;
                bp_count++;
            });

            CustomCrawlerDynamics.ss.Subscribe<PausedEvent>(async x =>
            {
                paused.Add(x);
                paused_count++;
                await CustomCrawlerDynamics.ss.SendAsync<ResumeCommand>();
            });

            await CustomCrawlerDynamics.ss.SendAsync<SetBreakpointsActiveCommand>();
            await CustomCrawlerDynamics.ss.SendAsync<ReloadCommand>();
        }
    }
}
