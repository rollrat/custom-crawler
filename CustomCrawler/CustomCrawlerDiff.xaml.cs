/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// CustomCrawlerDiff.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomCrawlerDiff : Window
    {
        ChromiumWebBrowser browser;

        public CustomCrawlerDiff()
        {
            InitializeComponent();

            browser = new ChromiumWebBrowser(string.Empty);
            browserContainer.Content = browser;

            DiffList.DataContext = new CustomCrawlerDiffDataGridViewModel();
            DiffList.Sorting += new DataGridSortingEventHandler(new DataGridSorter<CustomCrawlerDiffDataGridItemViewModel>(DiffList).SortHandler);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var html1 = NetCommon.DownloadString(URL1Text.Text);
            var html2 = NetCommon.DownloadString(URL2Text.Text);

            var tree1 = new HtmlTree(html1);
            var tree2 = new HtmlTree(html2);

            tree1.BuildTree();
            tree2.BuildTree();

            var diff = tree1.Diff(tree2);

            if (diff.Item1)
            {
                MessageBox.Show("The structure of the downloaded data is exactly the same and no differences can be found.", "Diff", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var rr = new List<CustomCrawlerDiffDataGridItemViewModel>();
            var index = 0;

            foreach (var node in diff.Item2)
            {
                string info;
                if (node.Item1.Name != node.Item2.Name)
                {
                    info = $"Tag-diff: {node.Item1.Name} <=> {node.Item2.Name}";
                }
                else if (node.Item1.ChildNodes.Count != node.Item2.ChildNodes.Count)
                {
                    info = $"Childcount-diff: {node.Item1.XPath}";
                }
                else if(node.Item1.Name == "#text")
                {
                    info = $"Text-diff: {node.Item1.InnerText.Trim()} <=> {node.Item2.InnerText.Trim()}";
                }
                else if (!HtmlTree.IsEqual(node.Item1.Attributes, node.Item2.Attributes))
                {
                    info = $"Attributes-diff: {node.Item1.XPath}";
                }
                else
                {
                    info = "";
                }

                rr.Add(new CustomCrawlerDiffDataGridItemViewModel
                {
                    Index = (++index).ToString(),
                    Info = info,
                    Location = node.Item1,
                });
            }

            marking(tree1);

            browser.LoadHtml(tree1[0][0].OuterHtml, URL1Text.Text);
            Thread.Sleep(500);
            DiffList.DataContext  = new CustomCrawlerDiffDataGridViewModel(rr);
        }

        private void marking(HtmlTree tree)
        {
            for (int i = 0; i <= tree.Height; i++)
            {
                for (int j = 0; j < tree[i].Count; j++)
                {
                    if (tree[i][j].Name != "#comment" && tree[i][j].Name != "#text")
                    {
                        tree[i][j].SetAttributeValue("ccw_tag", $"ccw_{i}_{j}");
                        tree[i][j].SetAttributeValue("onmouseenter", $"ccw.hoverelem('ccw_{i}_{j}')");
                        tree[i][j].SetAttributeValue("onmouseleave", $"ccw.hoverelem('ccw_{i}_{j}')");
                    }
                }
            }
        }

        string before;

        private void DiffList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DiffList.SelectedItems.Count > 0 && browser.IsLoaded)
            {
                var node = (DiffList.SelectedItems[0] as CustomCrawlerDiffDataGridItemViewModel).Location;

                if (node.Name == "#text")
                    node = node.ParentNode;

                browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '0em';").Wait();
                before = $"ccw_tag={node.GetAttributeValue("ccw_tag", "")}";
                browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '1em solid #FDFF47';").Wait();
                browser.EvaluateScriptAsync($"document.querySelector('[{before}]').scrollIntoView(true);").Wait();
            }
        }
    }
}
