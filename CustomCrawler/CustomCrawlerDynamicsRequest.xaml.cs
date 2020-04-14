/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using CustomCrawler.chrome_devtools;
using MasterDevs.ChromeDevTools;
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

        public CustomCrawlerDynamicsRequest(IChromeSession env, CustomCrawlerDynamics parent)
        {
            InitializeComponent();

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
                var xx = "";
                //if (x.Node == null || x.Node.Attributes == null)
                //    xx = string.Join(",", x.Node.Attributes);
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

            //Closed += (s, e) =>
            //{
            //    env.Dispose();
            //};
        }

        private void RequestList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}
