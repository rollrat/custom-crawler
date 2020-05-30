/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using MasterDevs.ChromeDevTools;
using MasterDevs.ChromeDevTools.Protocol.Chrome.DOM;
using MasterDevs.ChromeDevTools.Protocol.Chrome.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CustomCrawler
{
    /// <summary>
    /// CustomCrawlerDynamicsHover.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomCrawlerDynamicsHover : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        CustomCrawlerDynamics parent;
        public CustomCrawlerDynamicsHover(CustomCrawlerDynamics parent)
        {
            InitializeComponent();

            this.parent = parent;

            Loaded += CustomCrawlerDynamicsHover_Loaded;
            Closed += CustomCrawlerDynamicsHover_Closed;
        }

        private void CustomCrawlerDynamicsHover_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void CustomCrawlerDynamicsHover_Closed(object sender, EventArgs e)
        {
            childs.Where(x => x.IsLoaded).ToList().ForEach(x => x.Close());
        }

        public async Task Update(long nodeid)
        {
            var dom = await CustomCrawlerDynamics.ss.SendAsync(new GetDocumentCommand());
            var nn = await CustomCrawlerDynamics.ss.SendAsync(new PushNodesByBackendIdsToFrontendCommand { BackendNodeIds = new long[] { nodeid } });
            var mm = await CustomCrawlerDynamics.ss.SendAsync(new GetNodeStackTracesCommand { NodeId = nn.Result.NodeIds[0] });
            var stack = mm.Result.Creation;
            if (stack != null)
            {
                var depth = 0;

                var paragraph = new Paragraph();

                while (stack != null)
                {
                    if (!string.IsNullOrEmpty(stack.Description))
                        paragraph.Inlines.Add("Description: " + stack.Description + "\r\n");

                    if (depth < 5)
                    {
                        var frames = stack.CallFrames.ToList().Where(frame => !CustomCrawlerDynamics.ignore_js(frame.Url)).ToList();
                        var picks = new List<(CallFrame, int, int, int)>[frames.Count];

                        await Task.Run(() => Parallel.For(0, frames.Count, i =>
                        {
                             var frame = frames[i];
                             var node = JsManager.Instance.FindByLocation(frame.Url, (int)frame.LineNumber + 1, (int)frame.ColumnNumber + 1);
                             picks[i] = parent.pick_candidate(frame.Url, node, frame.FunctionName, (int)frame.LineNumber + 1, (int)frame.ColumnNumber + 1);
                        }));

                        foreach (var frame in stack.CallFrames)
                        {
                            if (CustomCrawlerDynamics.ignore_js(frame.Url))
                                continue;

                            if (!string.IsNullOrEmpty(frame.Url))
                            {
                                var hy1 = new Hyperlink();
                                //hy1.NavigateUri = new Uri(frame.Url);
                                hy1.DataContext = (frame.Url, frame.LineNumber + 1, frame.ColumnNumber + 1);
                                hy1.Inlines.Add($"{frame.Url}");
                                paragraph.Inlines.Add(hy1);
                            }

                            paragraph.Inlines.Add($":<{frame.FunctionName}>:{frame.LineNumber + 1}:{frame.ColumnNumber + 1}\r\n");

                            // Currently not support html built-in script
                            var count = 0;

                            int index = 0;
                            foreach (var pick in picks[index])
                            {
                                paragraph.Inlines.Add("  => ");
                                var hy2 = new Hyperlink();
                                hy2.DataContext = pick.Item2;
                                hy2.Inlines.Add($"{parent.requests[pick.Item2].Request.Url}");
                                paragraph.Inlines.Add(hy2);

                                if (pick.Item1.FunctionName != frame.FunctionName || pick.Item1.LineNumber != frame.LineNumber || pick.Item1.ColumnNumber != frame.ColumnNumber)
                                    paragraph.Inlines.Add($":<{pick.Item1.FunctionName}>:{pick.Item1.LineNumber + 1}:{pick.Item1.ColumnNumber + 1}");
                                paragraph.Inlines.Add(new LineBreak());

                                if (count++ > 10)
                                    break;
                                index++;
                            }
                        }
                    }

                    depth++;
                    stack = stack.Parent;
                }

                Info.Document.Blocks.Clear();
                Info.Document.Blocks.Add(paragraph);
            }
            else
            {
                Info.Document.Blocks.Clear();
                Info.Document.Blocks.Add(new Paragraph(new Run("Static Node")));
            }
        }

        List<Window> childs = new List<Window>();
        private void Hyperlink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var hyperlink = (Hyperlink)sender;
            if (!(hyperlink.DataContext is int))
            {
                var tt = hyperlink.DataContext as (string, long, long)?;
                new ScriptViewer(tt.Value.Item1, (int)tt.Value.Item2, (int)tt.Value.Item3, true).Show();
            }
            else
            {
                var child = new CustomCrawlerDynamicsRequestInfo(parent.requests[(hyperlink.DataContext as int?).Value],
                    parent.response[parent.requests[(hyperlink.DataContext as int?).Value].RequestId]);
                child.Show();
                childs.Add(child);
            }
        }
    }
}
