/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using CustomCrawler.chrome_devtools;
using MasterDevs.ChromeDevTools;
using MasterDevs.ChromeDevTools.Protocol.Chrome.Debugger;
using MasterDevs.ChromeDevTools.Protocol.Chrome.DOM;
using MasterDevs.ChromeDevTools.Protocol.Chrome.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// CustomCrawlerDynamicsRequest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomCrawlerDynamicsRequest : Window
    {
        int index_count = 0;
        CustomCrawlerDynamics parent;

        public CustomCrawlerDynamicsRequest(IChromeSession env, CustomCrawlerDynamics parent)
        {
            InitializeComponent();

            this.parent = parent;
            RequestList.DataContext = new CustomCrawlerDynamicsRequestDataGridViewModel();
            RequestList.Sorting += new DataGridSortingEventHandler(new DataGridSorter<CustomCrawlerDynamicsRequestDataGridItemViewModel>(RequestList).SortHandler);

            env.Subscribe<RequestWillBeSentEvent>(x =>
            {
                Task.Run(() => parent.add_request_info(x));
                Application.Current.Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    (RequestList.DataContext as CustomCrawlerDynamicsRequestDataGridViewModel).Items.Add(new CustomCrawlerDynamicsRequestDataGridItemViewModel
                    {
                        Id = (++index_count).ToString(),
                        Type = "Request",
                        Url = x.Request.Url,
                        ContentType = x.Type.ToString(),
                        Request = x
                    });
                }));
            });

            env.Subscribe<ResponseReceivedEvent>(x =>
            {
                Task.Run(() => parent.add_response_info(x));
                Application.Current.Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    (RequestList.DataContext as CustomCrawlerDynamicsRequestDataGridViewModel).Items.Add(new CustomCrawlerDynamicsRequestDataGridItemViewModel
                    {
                        Id = (++index_count).ToString(),
                        Type = "Response",
                        Url = x.Response.Url,
                        ContentType = x.Type.ToString(),
                        Response = x
                    });
                }));
            });

            env.Subscribe<DocumentUpdatedEvent>(x =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    (RequestList.DataContext as CustomCrawlerDynamicsRequestDataGridViewModel).Items.Add(new CustomCrawlerDynamicsRequestDataGridItemViewModel
                    {
                        Id = (++index_count).ToString(),
                        Type = "DocumentUpdated",
                    });
                }));
            });

            env.Subscribe<ChildNodeInsertedEvent>(x =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    (RequestList.DataContext as CustomCrawlerDynamicsRequestDataGridViewModel).Items.Add(new CustomCrawlerDynamicsRequestDataGridItemViewModel
                    {
                        Id = (++index_count).ToString(),
                        Type = "ChildNodeInserted",
                        Url = $"{x.Node.NodeName}"
                    });
                }));
            });

            env.Subscribe<ScriptParsedEvent>(async x =>
            {
                _ = Task.Run(() => parent.add_script_info(x));
                if (x.Url == "")
                {
                    var pos = "";

                    if (x.StackTrace != null)
                    {
                        pos = $"{x.StackTrace.CallFrames[0].Url}:<{x.StackTrace.CallFrames[0].FunctionName}>:{x.StackTrace.CallFrames[0].LineNumber + 1}:{x.StackTrace.CallFrames[0].ColumnNumber + 1}";

                        // Cannot get the script code later.
                        var src_snippet = await CustomCrawlerDynamics.ss.SendAsync(new GetScriptSourceCommand { ScriptId = x.ScriptId });
                        var src_code = "";
                        if (src_snippet.Result != null)
                        {
                            src_code = $"/* this code is generated from {pos} */\r\n";
                            src_code += src_snippet.Result.ScriptSource;
                        }

                        await Application.Current.Dispatcher.BeginInvoke(new Action(
                        delegate
                        {
                            (RequestList.DataContext as CustomCrawlerDynamicsRequestDataGridViewModel).Items.Add(new CustomCrawlerDynamicsRequestDataGridItemViewModel
                            {
                                Id = (++index_count).ToString(),
                                Type = "AnonymouseParsed",
                                Url = pos,
                                AnonymouseCode = src_code,
                                AnonymouseSource = x.StackTrace.CallFrames[0]
                            });
                        }));
                    }
                }
            });

            //Closed += (s, e) =>
            //{
            //    env.Dispose();
            //};
        }

        private void RequestList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void RequestList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RequestList.SelectedItems.Count > 0)
            {
                var item = (RequestList.SelectedItems[0] as CustomCrawlerDynamicsRequestDataGridItemViewModel);

                RequestWillBeSentEvent request = item.Request;
                ResponseReceivedEvent response = item.Response;

                if (request == null && response == null)
                {
                    if (item.AnonymouseSource.Url != "")
                    {
                        new ScriptViewer(item.AnonymouseSource.Url, (int)item.AnonymouseSource.LineNumber + 1, (int)item.AnonymouseSource.ColumnNumber + 1, true).Show();
                    }
                    if (item.AnonymouseCode != "")
                    {
                        new ScriptViewer(false, item.AnonymouseCode).Show();
                    }
                    return;
                }

                if (request == null)
                {
                    lock (parent.requests)
                        if (parent.requests_id.ContainsKey(response.RequestId))
                            request = parent.requests_id[response.RequestId];
                }
                else if (response == null)
                {
                    lock (parent.response)
                        if (parent.response.ContainsKey(request.RequestId))
                            response = parent.response[request.RequestId];
                }

                if (request != null)
                    new CustomCrawlerDynamicsRequestInfo(request, response).Show();
            }
        }
    }
}
