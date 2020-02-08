/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using CustomCrawler.chrome_devtools;
using CustomCrawler.chrome_devtools.Event.Debugger;
using CustomCrawler.chrome_devtools.Event.DOM;
using CustomCrawler.chrome_devtools.Event.Network;
using CustomCrawler.chrome_devtools.Method.Debugger;
using CustomCrawler.chrome_devtools.Method.DOM;
using CustomCrawler.chrome_devtools.Method.DOMDebugger;
using CustomCrawler.chrome_devtools.Types.DOM;
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

        public CustomCrawlerDynamicsRequest(ChromeDevtoolsEnvironment env)
        {
            InitializeComponent();

            RequestList.DataContext = new CustomCrawlerDynamicsRequestDataGridViewModel();
            RequestList.Sorting += new DataGridSortingEventHandler(new DataGridSorter<CustomCrawlerDynamicsRequestDataGridItemViewModel>(RequestList).SortHandler);

            env.Subscribe<RequestWillBeSent>(x =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    (RequestList.DataContext as CustomCrawlerDynamicsRequestDataGridViewModel).Items.Add(new CustomCrawlerDynamicsRequestDataGridItemViewModel
                    {
                        Id = (++index_count).ToString(),
                        Type = "Request",
                        Url = x.Request.Url,
                        ContentType = x.ResourceType,
                        Request = x
                    });
                }));
            });

            env.Subscribe<ResponseReceived>(x =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    (RequestList.DataContext as CustomCrawlerDynamicsRequestDataGridViewModel).Items.Add(new CustomCrawlerDynamicsRequestDataGridItemViewModel
                    {
                        Id = (++index_count).ToString(),
                        Type = "Response",
                        Url = x.Response.Url,
                        ContentType = x.ResourceType,
                        Response = x
                    });
                }));
            });

            env.Subscribe<DocumentUpdated>(x =>
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

            env.Subscribe<ChildNodeInserted>(x =>
            {
                var xx = "";
                if (x.Node == null || x.Node.Attributes == null)
                    xx = string.Join(",", x.Node.Attributes);
                Application.Current.Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    (RequestList.DataContext as CustomCrawlerDynamicsRequestDataGridViewModel).Items.Add(new CustomCrawlerDynamicsRequestDataGridItemViewModel
                    {
                        Id = (++index_count).ToString(),
                        Type = "ChildNodeInserted",
                        Url = $"{x.Node.NodeName} {xx}"
                    });
                }));
            });
           
            Closed += (s, e) =>
            {
                env.Dispose();
            };
        }

        private void RequestList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}
