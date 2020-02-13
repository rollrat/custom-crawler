/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using CefSharp;
using CefSharp.Wpf;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
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
    /// CustomCrawlerCluster.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomCrawlerCluster : Window
    {
        ChromiumWebBrowser browser;
        string url;
        HtmlTree tree;
        CallbackCCW cbccw;

        public CustomCrawlerCluster(string url, HtmlTree tree)
        {
            InitializeComponent();

            browser = new ChromiumWebBrowser(string.Empty);
            browserContainer.Content = browser;
            browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;

            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            browser.JavascriptObjectRepository.Register("ccw", cbccw = new CallbackCCW(this), isAsync: true);

            this.url = url;
            this.tree = tree;

            ResultList.DataContext = new CustomCrawlerClusterDataGridViewModel();
            ResultList.Sorting += new DataGridSortingEventHandler(new DataGridSorter<CustomCrawlerClusterDataGridItemViewModel>(ResultList).SortHandler);
            CaptureList.DataContext = new CustomCrawlerClusterCaptureDataGridViewModel();
            CaptureList.Sorting += new DataGridSortingEventHandler(new DataGridSorter<CustomCrawlerClusterCaptureDataGridItemViewModel>(CaptureList).SortHandler);
            PatternList.DataContext = new CustomCrawlerClusterPatternDataGridViewModel();
            PatternList.Sorting += new DataGridSortingEventHandler(new DataGridSorter<CustomCrawlerClusterPatternDataGridItemViewModel>(PatternList).SortHandler);

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

            KeyDown += CustomCrawlerCluster_KeyDown;
        }

        private void Browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                refresh();
            }
            catch { }
        }

        private void CustomCrawlerCluster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                if (locking)
                {
                    F2.Text = "F2: Lock";
                    locking = false;
                }
                else
                {
                    F2.Text = "F2: UnLock";
                    locking = true;
                }
            }
            else if (e.Key == Key.F3)
            {
                depth--;
                if (depth < 0)
                    depth = 0;
                Depth.Text = $"Depth={depth}";
                cbccw.adjust();
            }
            else if (e.Key == Key.F4)
            {
                depth++;
                Depth.Text = $"Depth={depth}";
                cbccw.adjust();
            }
            else if (e.Key == Key.F5)
            {
                new CustomCrawlerTree(cbccw.selected_node, new List<HtmlNode> { cbccw.selected_node }, this).Show();
            }
            else if (e.Key == Key.F6)
            {
                if (cbccw.selected_node != null)
                {
                    AppendCapture(cbccw.selected_node.XPath);
                }
            }
            else if (e.Key == Key.F7)
            {
                if (cbccw.selected_node != null)
                {
                    var locking = this.locking;
                    this.locking = true;
                    (new CustomCrawlerClusterCapture() { Owner = this }).ShowDialog();
                    this.locking = locking;
                }
            }
            else if (e.Key == Key.Add)
            {
                if (browser.ZoomLevel <= 3.0)
                {
                    browser.ZoomInCommand.Execute(null);
                }
            }
            else if (e.Key == Key.Subtract)
            {
                if (browser.ZoomLevel >= -3.0)
                {
                    browser.ZoomOutCommand.Execute(null);
                }
            }
            else if (e.Key == Key.F8)
            {
                if (cbccw.selected_node != null)
                {
                    var tar = cbccw.selected_node.GetAttributeValue("ccw_tag", "");
                    if (Marking.Contains(tar))
                    {
                        Marking.Remove(tar);
                        cbccw.before_border = "";
                        browser.EvaluateScriptAsync($"document.querySelector('[ccw_tag={tar}]').style.border = '';").Wait();
                    }
                    else
                    {
                        Marking.Add(cbccw.selected_node.GetAttributeValue("ccw_tag", ""));
                        refresh_marking();
                    }
                }
            }
            else if (e.Key == Key.F12)
            {
                connect_devtools();
            }
        }

        private void refresh()
        {
            browser.LoadHtml(tree[0][0].OuterHtml, url);
            Thread.Sleep(100);
            refresh_marking();
        }

        public List<string> Marking = new List<string>();
        private void refresh_marking()
        {
            var builder = new StringBuilder();
            foreach (var mm in Marking)
                builder.Append($"document.querySelector('[ccw_tag={mm}]').style.border = '0.2em solid orange';");
            browser.EvaluateScriptAsync(builder.ToString()).Wait();
        }

        #region Tree, Capture, Pattern

        public void AppendCapture(string info)
        {
            var index = CaptureList.Items.Count + 1;
            (CaptureList.DataContext as CustomCrawlerClusterCaptureDataGridViewModel).Items.Add(new CustomCrawlerClusterCaptureDataGridItemViewModel
            {
                Index = index.ToString(),
                Info = info,
                DateTime = DateTime.Now.ToString("h:mm ss"),
                Node = cbccw.selected_node,
            });
        }

        public void SelectNode(HtmlNode node)
        {
            F2.Text = "F2: UnLock";
            locking = true;

            var ij = tree.UnRef(node);
            var dd = depth;
            depth = 0;
            cbccw.hoverelem($"ccw_{ij.Item1}_{ij.Item2}", true);
            depth = dd;
            browser.EvaluateScriptAsync($"document.querySelector('[ccw_tag=ccw_{ij.Item1}_{ij.Item2}]').scrollIntoView(true);").Wait();
        }

        private void CaptureList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (CaptureList.SelectedItems.Count > 0)
                {
                    if (MessageBox.Show($"Are you sure you want to delete {CaptureList.SelectedItems.Count} items?", "Cluster", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        CaptureList.SelectedItems.Cast<object>().ToList().ForEach(x => (CaptureList.DataContext as CustomCrawlerClusterCaptureDataGridViewModel).Items.Remove(x as CustomCrawlerClusterCaptureDataGridItemViewModel));
                    }
                }
            }
        }

        private void ExtractPatterns_Click(object sender, RoutedEventArgs e)
        {
            if (CaptureList.SelectedItems.Count <= 1)
            {
                MessageBox.Show("Select two and more items!", "Cluster", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selected = CaptureList.SelectedItems.OfType<CustomCrawlerClusterCaptureDataGridItemViewModel>();
            var lca = tree.GetLCANode(selected.Select(x => (tree[x.Node], x.Node).ToTuple()).ToList());

            try
            {
                var pattern = new Pattern
                {
                    LCA = lca,
                    Nodes = selected.Select(x => (x.Info, x.Node)).ToList(),
                    SubPatternsString = selected.Select(x => make_string(x.Node)).ToList(),
                    Content = make_string(lca),
                    Info = make_string(lca, selected.ToDictionary(x => x.Node, x => "@" + x.Info))
                };

                (PatternList.DataContext as CustomCrawlerClusterPatternDataGridViewModel).Items.Add(new CustomCrawlerClusterPatternDataGridItemViewModel
                {
                    Index = (PatternList.Items.Count + 1).ToString(),
                    Pattern = pattern.Info,
                    Patterns = pattern
                });
            }
            catch
            {
                MessageBox.Show("Do not select same elements!", "Cluster", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        Dictionary<HtmlNode, string> msdp = new Dictionary<HtmlNode, string>();
        private string make_string(HtmlNode node)
        {
            if (node.ChildNodes.Count == 0)
            {
                if (node.Name == "#text")
                    return "#";
                return $"({node.Name})";
            }
            if (msdp.ContainsKey(node))
                return msdp[node];
            var ms = $"({node.Name}{string.Join("", node.ChildNodes.ToList().Where(x => x.Name != "#comment").Select(x => make_string(x)))})";
            msdp.Add(node, ms);
            return ms;
        }

        private string make_string(HtmlNode node, Dictionary<HtmlNode, string> snodes)
        {
            if (snodes != null && snodes.ContainsKey(node))
                return snodes[node];
            if (node.ChildNodes.Count == 0)
            {
                if (node.Name == "#text")
                    return "#";
                return $"({node.Name})";
            }
            return $"({node.Name}{string.Join("", node.ChildNodes.ToList().Where(x => x.Name != "#comment").Select(x => make_string(x, snodes)))})";
        }

        public class Pattern
        {
            public HtmlNode LCA { get; set; }
            public List<(string, HtmlNode)> Nodes { get; set; }
            public List<string> SubPatternsString { get; set; }
            public string Content { get; set; }
            public string Info { get; set; }
        }

        private void CaptureList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (CaptureList.SelectedItems.Count > 0)
            {
                var node = (CaptureList.SelectedItems[0] as CustomCrawlerClusterCaptureDataGridItemViewModel).Node;

                if (section)
                {
                    refresh();
                    section = false;
                }

                browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '0em';").Wait();
                before = $"ccw_tag={node.GetAttributeValue("ccw_tag", "")}";
                browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '1em solid #FDFF47';").Wait();
                browser.EvaluateScriptAsync($"document.querySelector('[{before}]').scrollIntoView(true);").Wait();
            }
        }

        string before_find = "";
        List<(HtmlNode, string)> candidate;
        Pattern latest_pattern;
        private void FindPatternsOnPage_Click(object sender, RoutedEventArgs e)
        {
            if (PatternList.SelectedItems.Count != 1)
            {
                MessageBox.Show("Selects only one item!", "Cluster", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Build nodes
            make_string(tree.RootNode);

            var pattern = PatternList.SelectedItems[0] as CustomCrawlerClusterPatternDataGridItemViewModel;
            candidate = new List<(HtmlNode, string)>();
            latest_pattern = pattern.Patterns;

            if (!AllowRoughly.IsChecked.Value)
            {
                foreach (var pp in msdp)
                    if (pp.Value == pattern.Patterns.Content)
                        candidate.Add((pp.Key, "100.0%"));
            }
            else
            {
                var value = AccuracyPattern.Value;
                Parallel.ForEach(msdp, pp =>
                {
                    var distance = Strings.ComputeLevenshteinDistance(pp.Value, pattern.Patterns.Content);
                    var per = 100.0 - distance / (double)Math.Max(pp.Value.Length, pattern.Patterns.Content.Length) * 100.0;
                    if (per >= value)
                    {
                        lock(candidate)
                            candidate.Add((pp.Key, per.ToString("0.0") + "%"));
                    }
                });
            }

            var builder = new StringBuilder();

            browser.EvaluateScriptAsync(before_find).Wait();
            candidate.ForEach(x => builder.Append($"document.querySelector('[ccw_tag={x.Item1.GetAttributeValue("ccw_tag", "")}]').style.border = '0.2em solid #FDFF47';"));
            browser.EvaluateScriptAsync(builder.ToString()).Wait();
            browser.EvaluateScriptAsync($"document.querySelector('[ccw_tag={candidate[0].Item1.GetAttributeValue("ccw_tag", "")}]').scrollIntoView(true);").Wait();
            before_find = string.Join("", candidate.Select(x => $"document.querySelector('[ccw_tag={x.Item1.GetAttributeValue("ccw_tag", "")}]').style.border = '';"));

            if (!AllowLive.IsChecked.Value)
                MessageBox.Show($"Found {candidate.Count} identical patterns!\r\n" + string.Join("\r\n", candidate.Select(x => $"({x.Item2}) {x.Item1.XPath}")), "Cluster", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (PatternAccuracy != null)
            {
                PatternAccuracy.Text = (sender as Slider).Value.ToString("0.0") + "%";
                if (AllowLive.IsChecked.Value)
                {
                    FindPatternsOnPage_Click(null, null);
                }
            }
        }

        private void TestFoundElements_Click(object sender, RoutedEventArgs e)
        {
            if (candidate == null || candidate.Count <= 1)
            {
                MessageBox.Show("There must be at least two candidates. Please run FindPatternsOnPage first or adjust the accuracy.", "Cluster", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("This method may differ from what is displayed in the browser because it uses a logical comparison based on the selected candidates. Do you want to continue?", "Cluster", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var candidate_lca = tree.GetLCANode(candidate.Select(x => (tree[x.Item1], x.Item1).ToTuple()).ToList());

                Func<HtmlNode, int> distance = (HtmlNode x) =>
                {
                    int parent_distance = 0;
                    for (var nn = x; nn == candidate_lca; nn = nn.ParentNode, parent_distance++) ;
                    return parent_distance;
                };

                var sdist = distance(candidate[0].Item1);

                if (candidate.Any(x => distance(x.Item1) != sdist))
                {
                    MessageBox.Show("LCA distance is so far. Please adjust the similarity higher to reduce the range.", "Cluster", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var builder = new StringBuilder();

                var pattern = latest_pattern;

                builder.Append($"Report for auto-crawling - {DateTime.Now.ToString()}\r\n");
                builder.Append("This file is automatically created by Custom Crawler(CC) Clustering Tool\r\n");
                builder.Append("Copyright (C) 2020. rollrat. All Rights Reserved.\r\n");
                builder.Append("\r\n");
                builder.Append("-- Captures Info remove LCA Prefix --\r\n");
                pattern.Nodes.ForEach(x => builder.Append($"@{x.Item1} = {x.Item2.XPath.Replace(pattern.LCA.XPath, "")}\r\n"));
                builder.Append("\r\n");
                builder.Append("-- Captures Info Origin --\r\n");
                pattern.Nodes.ForEach(x => builder.Append($"@{x.Item1} = {x.Item2.XPath}\r\n"));
                builder.Append("\r\n");
                builder.Append("-- Available Captures Info --\r\n");

                var avc = new Dictionary<HtmlNode, int>();
                foreach (var cd in candidate)
                {
                    foreach (var node in pattern.Nodes)
                    {
                        var postfix = node.Item2.XPath.Replace(pattern.LCA.XPath, "");
                        var nn = cd.Item1.XPath + postfix;

                        var snode = tree.RootNode.SelectSingleNode(nn);
                        if (snode != null)
                        {
                            if (!avc.ContainsKey(node.Item2))
                                avc.Add(node.Item2, 0);
                            avc[node.Item2]++;
                        }
                    }
                }

                var hh = avc.ToList().Where(x => x.Value == candidate.Count).Select(x => x.Key).ToHashSet();
                pattern.Nodes.Where(x => hh.Contains(x.Item2)).ToList().ForEach(x => builder.Append($"@{x.Item1} = {x.Item2.XPath}\r\n"));

                builder.Append("\r\n");
                builder.Append("-- Pattern Info --\r\n");
                builder.Append("P-Summary: " + pattern.Info + "\r\n");
                builder.Append("P-LCA: " + pattern.LCA.XPath + "\r\n");
                builder.Append("LCA: " + candidate_lca.XPath + "\r\n");
                builder.Append("\r\n");
                builder.Append("-- Capture result from Page --\r\n");
                builder.Append("Count: " + candidate.Count + "\r\n");

                int tc = 0;
                foreach (var cd in candidate)
                {
                    builder.Append("-------------------------\r\n");
                    builder.Append($"test-case: #{++tc}\r\n");
                    builder.Append($"tc-lca: {cd.Item1.XPath}\r\n");
                    foreach (var node in pattern.Nodes)
                    {
                        var postfix = node.Item2.XPath.Replace(pattern.LCA.XPath, "");
                        var nn = cd.Item1.XPath + postfix;

                        var snode = tree.RootNode.SelectSingleNode(nn);
                        if (snode != null)
                        {
                            builder.Append($"@{node.Item1} = ");
                            var tt = snode.InnerText.Trim();
                            if (tt.Length > 0)
                                builder.Append($"{snode.InnerText.Trim()}\r\n");
                            else
                            {
                                builder.Append($"/{snode.Name}:");
                                if (snode.Name == "img")
                                    builder.Append($"{snode.GetAttributeValue("src", "") + snode.GetAttributeValue("data-src", "")}");
                                else if (snode.Name == "a")
                                    builder.Append($"{snode.GetAttributeValue("href", "")}");
                                else
                                    builder.Append($"{{{string.Join(" ", snode.Attributes.Select(x => $"{x.Name}=\"{x.Value}\""))}}}");
                                builder.Append("\r\n");
                            }
                        }
                    }
                    builder.Append("\r\n");
                }

                builder.Append("-- Test-case Patterns --\r\n");
                var tcpattern = extract_pattern(candidate.Select(x => x.Item1.XPath).ToList());
                builder.Append("Pattern: " + tcpattern + "\r\n");
                builder.Append("public class Pattern\r\n");
                builder.Append("{\r\n");
                pattern.Nodes.ForEach(x => builder.Append($"    public string {x.Item1};\r\n"));
                builder.Append("}\r\n");
                builder.Append("\r\n");
                builder.Append("public List<Pattern> Extract(string html)\r\n");
                builder.Append("{\r\n");
                builder.Append("    HtmlDocument document = new HtmlDocument();\r\n");
                builder.Append("    document.LoadHtml(html);\r\n");
                builder.Append("    var result = new List<Pattern>();\r\n");
                builder.Append("    var root_node = document.DocumentNode;\r\n");

                if (tcpattern[0] == '/')
                {
                    builder.Append($"    for (int i = 1; ; i++)\r\n");
                    builder.Append("    {\r\n");
                    builder.Append($"        var node = root_node.SelectSingleNode($\"{tcpattern}\");\r\n");
                    builder.Append($"        if (node == null) break;\r\n");
                    builder.Append($"        var pattern = new Pattern();\r\n");
                    foreach (var pp in pattern.Nodes)
                    {
                        var postfix = pp.Item2.XPath.Replace(pattern.LCA.XPath, "");
                        if (hh.Contains(pp.Item2))
                        {
                            builder.Append($"        pattern.{pp.Item1} = node.SelectSingleNode(\".{postfix}\").InnerText;\r\n");
                        }
                        else
                        {
                            builder.Append($"        if (node.SelectSingleNode(\".{postfix}\") != null)\r\n");
                            builder.Append($"            pattern.{pp.Item1} = node.SelectSingleNode(\".{postfix}\").InnerText;\r\n");
                        }
                    }
                    builder.Append($"        result.Add(pattern);\r\n");
                    builder.Append("    }\r\n");
                }
                else
                {

                }

                builder.Append($"    return result;\r\n");
                builder.Append("}\r\n");


                var fn = $"ccpcccct-{DateTime.Now.Ticks}.txt";
                File.WriteAllText(fn, builder.ToString());
                Process.Start(fn);
            }
        }

        private List<string> parse_pattern_string(string pp)
        {
            var tokens = new List<string>();

            for (int i = 0; i < pp.Length; i++)
            {
                var builder = new StringBuilder();
                builder.Append(pp[i]);
                if (char.IsNumber(pp[i]))
                {
                    while (i < pp.Length - 1 && char.IsNumber(pp[i + 1]))
                        builder.Append(pp[++i]);
                }
                else
                {
                    while (i < pp.Length - 1 && !char.IsNumber(pp[i + 1]))
                        builder.Append(pp[++i]);
                }
                tokens.Add(builder.ToString());
            }

            return tokens;
        }

        private List<bool> classify_tokens(List<string> tokens)
        {
            var isnum = new List<bool>(tokens.Count);
            tokens.ForEach(x =>
            {
                int nn;
                if (int.TryParse(x, out nn))
                    isnum.Add(true);
                else
                    isnum.Add(false);
            });
            return isnum;
        }

        private string extract_pattern(List<string> list)
        {
            if (list.Count < 3) return "At least 3 inputs are required.";
            list.Sort(new Strings.NaturalComparer());
            var std = parse_pattern_string(list[0]);
            var fix = classify_tokens(std);
            var tokens = list.Select(x => parse_pattern_string(x)).ToList();
            var classes = tokens.Select(x => classify_tokens(x)).ToList();

            for (int i = 0; i < std.Count; i++)
            {
                if (!classes[0][i])
                {
                    for (int j = 0; j < classes.Count; j++)
                        if (classes[j].Count != classes[0].Count || tokens[j][i] != tokens[0][i])
                            return "Pattern Not Found.";
                }
            }

            var numbers = new List<List<int>>();
            for (int i = 0; i < fix.Count; i++)
            {
                if (fix[i])
                {
                    if (tokens[0][i] == tokens[1][i])
                    {
                        fix[i] = false;
                        continue;
                    }

                    numbers.Add(tokens.Select(x => Convert.ToInt32(x[i])).ToList());
                }
            }

            var pattern = new List<string>();
            for (int i = 0; i < numbers.Count; i++)
            {
                int a = numbers[i][0];
                int b = numbers[i][1];
                int c = numbers[i][2];

                if (c - b == b - a)
                {
                    if (a == 0)
                        pattern.Add($"i*{c - b}");
                    else
                        pattern.Add($"{a}+i*{c - b}");
                }
                else
                {
                    return "Patterns must be linear increment functions.";
                }
            }

            var builder = new StringBuilder();
            for (int i = 0, j = 0; i < fix.Count; i++)
            {
                if (fix[i])
                    builder.Append("{" + pattern[j++] + "}");
                else
                    builder.Append(std[i]);
            }

            return builder.ToString();
        }

        #endregion

        #region Cef Callback

        bool locking = false;
        int depth = 0;

        public class CallbackCCW
        {
            CustomCrawlerCluster instance;
            string before = "";
            public string before_border = "";
            string latest_elem = "";
            public HtmlNode selected_node;
            public CallbackCCW(CustomCrawlerCluster instance)
            {
                this.instance = instance;
            }
            public void hoverelem(string elem, bool adjust = false)
            {
                if (instance.locking && !adjust)
                    return;
                latest_elem = elem;
                var i = Convert.ToInt32(elem.Split('_')[1]);
                var j = Convert.ToInt32(elem.Split('_')[2]);
                for (int k = 0; k < instance.depth; k++)
                {
                    if (instance.tree[i][j].ParentNode == instance.tree.RootNode)
                        break;
                    var rr = instance.tree.UnRef(instance.tree[i][j].ParentNode);
                    (i, j) = rr;
                }
                selected_node = instance.tree[i][j];
                Application.Current.Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    try
                    {
                        instance.refresh_marking();
                        instance.hover_item.Text = instance.tree[i][j].XPath;
                        instance.browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '{before_border}';").Wait();
                        before = $"ccw_tag=ccw_{i}_{j}";
                        before_border = instance.browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border").Result.Result.ToString();
                        instance.browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '0.2em solid red';").Wait();
                        instance.CurrentXPath.Text = selected_node.XPath;

                        var builder = new StringBuilder();
                        builder.Append("public HtmlNode Extract(string html)\r\n");
                        builder.Append("{\r\n");
                        builder.Append("    HtmlDocument document = new HtmlDocument();\r\n");
                        builder.Append("    document.LoadHtml(html);\r\n");
                        builder.Append($"    return document.DocumentNode.SelectSingleNode(\"{selected_node.XPath}\");\r\n");
                        builder.Append("}\r\n");

                        instance.CurrentCode.Text = builder.ToString();
                    }
                    catch { }
                }));
            }
            public void adjust()
            {
                hoverelem(latest_elem, true);
            }
        }

        #endregion

        private async void RunButton_Click(object sender, RoutedEventArgs e)
        {
            var list = new List<CustomCrawlerClusterDataGridItemViewModel>();
            if ((Functions.SelectedItem as ComboBoxItem).Content.ToString() == "LinearClustering")
            {
                var rr = tree.LinearClustering();

                for (int i = 0; i < rr.Count; i++)
                {
                    list.Add(new CustomCrawlerClusterDataGridItemViewModel
                    {
                        Index = (i + 1).ToString(),
                        Count = rr[i].Item1.ToString("#,#"),
                        Accuracy = rr[i].Item2.ToString(),
                        Header = rr[i].Item3.Name + "+" + string.Join("/", rr[i].Item4.Select(x => x.Name)),
                        Node = rr[i].Item3
                    });
                }

                C2.Header = "Count";
                C3.Header = "Accuracy";
                C4.Header = "Header";
            }
            else if ((Functions.SelectedItem as ComboBoxItem).Content.ToString() == "StylistClustering")
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(
                    delegate
                    {
                        refresh();
                    }));
                    Thread.Sleep(500);
                });

                stylist_clustering(ref list);

                C2.Header = "Count"; // count of element
                C3.Header = "Use(%)"; // use space
                C4.Header = "Area"; // count of range
            }
            ResultList.DataContext = new CustomCrawlerClusterDataGridViewModel(list);
        }

        string before = "";
        bool section = false;

        private void ResultList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (ResultList.SelectedItems.Count > 0)
            {
                if ((Functions.SelectedItem as ComboBoxItem).Content.ToString() == "LinearClustering")
                {
                    var node = (ResultList.SelectedItems[0] as CustomCrawlerClusterDataGridItemViewModel).Node;

                    if (section)
                    {
                        refresh();
                        section = false;
                    }

                    browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '0em';").Wait();
                    before = $"ccw_tag={node.GetAttributeValue("ccw_tag", "")}";
                    browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '1em solid #FDFF47';").Wait();
                    browser.EvaluateScriptAsync($"document.querySelector('[{before}]').scrollIntoView(true);").Wait();
                }
                else if ((Functions.SelectedItem as ComboBoxItem).Content.ToString() == "StylistClustering")
                {
                    var node = (ResultList.SelectedItems[0] as CustomCrawlerClusterDataGridItemViewModel).Node;

                    if (section)
                    {
                        refresh();
                        section = false;
                    }

                    browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '0em';").Wait();
                    before = $"ccw_tag={node.GetAttributeValue("ccw_tag", "")}";
                    browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '1em solid #FDFF47';").Wait();
                    browser.EvaluateScriptAsync($"document.querySelector('[{before}]').scrollIntoView(true);").Wait();
                }
            }
        }

        private void ResultList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ResultList.SelectedItems.Count > 0)
            {
                if ((Functions.SelectedItem as ComboBoxItem).Content.ToString() == "LinearClustering")
                {
                    var node = (ResultList.SelectedItems[0] as CustomCrawlerClusterDataGridItemViewModel).Node;

                    if (node.Name == "tbody")
                        browser.LoadHtml($"<table>{node.OuterHtml}</table>", url);
                    else
                        browser.LoadHtml(node.OuterHtml, url);

                    section = true;
                }
            }
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            //browser.
        }

        #region Stylist Clustering

        private void stylist_clustering(ref List<CustomCrawlerClusterDataGridItemViewModel> result)
        {
            var pps = new List<List<(int?, int?, HtmlNode)>>();
            var ppsd = new Dictionary<HtmlNode, (int, int)>();
            for (int i = 0; i <= tree.Height; i++)
            {
                var pp = new List<(int?, int?, HtmlNode)>();
                for (int j = 0; j < tree[i].Count; j++)
                {
                    var w = browser.EvaluateScriptAsync($"document.querySelector('[ccw_tag=ccw_{i}_{j}]').clientWidth").Result.Result;
                    var h = browser.EvaluateScriptAsync($"document.querySelector('[ccw_tag=ccw_{i}_{j}]').clientHeight").Result.Result;

                    pp.Add((w as int?, h as int?, tree[i][j]));
                    ppsd.Add(tree[i][j], (i, j));
                }
                pps.Add(pp);
            }

            // area, use, use%, count
            var rr = new List<(int, int, double, int, HtmlNode)>();
            var max_area = 0;

            for (int i = 0; i <= tree.Height; i++)
                for (int j = 0; j < tree[i].Count; j++)
                {
                    if (!pps[i][j].Item1.HasValue)
                        continue;
                    int area = pps[i][j].Item1.Value * pps[i][j].Item2.Value;
                    int cnt = 0;
                    int use = 0;
                    foreach (var child in tree[i][j].ChildNodes)
                    {
                        var ij = ppsd[child];
                        if (!pps[ij.Item1][ij.Item2].Item1.HasValue)
                            continue;
                        cnt++;
                        use += pps[ij.Item1][ij.Item2].Item1.Value * pps[ij.Item1][ij.Item2].Item2.Value;
                    }
                    if (use == 0)
                        continue;
                    max_area = Math.Max(max_area, area);

                    rr.Add((area, use, use / (double)area * 100.0, cnt, tree[i][j]));
                }

            for (int i = 0; i < rr.Count; i++)
            {
                result.Add(new CustomCrawlerClusterDataGridItemViewModel
                {
                    Index = (i + 1).ToString(),
                    Count = rr[i].Item4.ToString(),
                    Accuracy = $"{rr[i].Item2.ToString("#,0")} ({rr[i].Item3.ToString("#0.0")} %)",
                    Header = $"{rr[i].Item1.ToString("#,0")} ({(rr[i].Item1 / (double)max_area * 100.0).ToString("#0.0")} %)",
                    Node = rr[i].Item5
                });
            }
        }

        #endregion

        #region WebSocket Connector

        private long GetTime()
        {
            long retval = 0;
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
            retval = (long)(t.TotalMilliseconds + 0.5);
            return retval;
        }

        private void connect_devtools()
        {
            var list = NetCommon.DownloadString("http://localhost:8088/json/list?t=" + GetTime());
            var ws = JArray.Parse(list)[0]["webSocketDebuggerUrl"];

            Task.Run(async () =>
            {
                var wss = new ClientWebSocket();
                await wss.ConnectAsync(new Uri(ws.ToString()), CancellationToken.None);
                await Task.WhenAll(Send(wss), Receive(wss));
            });

        }

        private static async Task Send(ClientWebSocket webSocket)
        {
            byte[] buffer = Encoding.UTF8.GetBytes("{\"id\":1,\"method\":\"Network.enable\",\"params\":{\"maxPostDataSize\":65536}}");
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private static async Task Receive(ClientWebSocket webSocket)
        {
            byte[] buffer = new byte[65535];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                {
                    var content = Encoding.UTF8.GetString(buffer);
                    //LogStatus(true, buffer, result.Count);
                }
            }
        }

        #endregion
    }
}
