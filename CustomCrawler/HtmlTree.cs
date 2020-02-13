/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawler
{
    /// <summary>
    /// Truncates HTML by depth.
    /// </summary>
    public class HtmlTree
    {
        HtmlNode root_node;
        List<List<HtmlNode>> depth_map;
        Dictionary<HtmlNode, int> depth_ref;
        Dictionary<HtmlNode, (int, int)> unref;

        public HtmlTree(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            root_node = document.DocumentNode;
        }

        public HtmlNode RootNode { get { return root_node; } }

        /// <summary>
        /// Gets the maximum depth of the HTML tree.
        /// </summary>
        public int Height { get { return depth_map.Count - 1; } }

        /// <summary>
        /// Gets all nodes at a certain depth.
        /// </summary>
        /// <param name="i">Depth</param>
        /// <returns></returns>
        public List<HtmlNode> this[int i]
        {
            get { return depth_map[i]; }
        }

        /// <summary>
        /// Get node depth.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int this[HtmlNode node]
        {
            get { return depth_ref[node]; }
        }

        public (int, int) UnRef(HtmlNode node)
        {
            return unref[node];
        }

        /// <summary>
        /// Visit all nodes in the HTML tree and generate a depth map.
        /// </summary>
        /// <param name="lower_bound"></param>
        /// <param name="upper_bound"></param>
        public void BuildTree(int lower_bound = 0, int upper_bound = int.MaxValue)
        {
            depth_map = new List<List<HtmlNode>>();
            depth_ref = new Dictionary<HtmlNode, int>();
            unref = new Dictionary<HtmlNode, (int, int)>();

            var queue = new Queue<Tuple<int, HtmlNode>>();
            var nodes = new List<HtmlNode>();
            int latest_depth = 0;

            queue.Enqueue(Tuple.Create(0, root_node));

            while (queue.Count > 0)
            {
                var e = queue.Dequeue();

                if (e.Item1 != latest_depth)
                {
                    depth_map.Add(nodes);
                    nodes = new List<HtmlNode>();
                    latest_depth = e.Item1;
                }

                if (lower_bound <= latest_depth)
                    nodes.Add(e.Item2);

                if (latest_depth < upper_bound && e.Item2.HasChildNodes)
                {
                    foreach (var node in e.Item2.ChildNodes)
                    {
                        queue.Enqueue(Tuple.Create(e.Item1 + 1, node));
                    }
                }
            }

            if (nodes.Count > 0)
                depth_map.Add(nodes);

            for (int i = 0; i < depth_map.Count; i++)
            {
                depth_map[i].ForEach(x => depth_ref.Add(x, i));
                for (int j = 0; j < depth_map[i].Count; j++)
                    unref.Add(depth_map[i][j], (i, j));
            }
        }

        /// <summary>
        /// Calculate Lowest Common Ancestor
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public HtmlNode GetLCANode(List<Tuple<int, HtmlNode>> list)
        {
            var min_depth = list.Min(x => x.Item1);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Item1 > min_depth)
                {
                    var node = list[i].Item2;
                    for (int j = 0; j < list[i].Item1 - min_depth; j++)
                    {
                        node = node.ParentNode;
                    }
                    list[i] = Tuple.Create(min_depth, node);
                }
            }

            for (int i = min_depth; i > 0; i--)
            {
                var pp = list[0].Item2;
                if (list.TrueForAll(x => x.Item2 == pp))
                    break;
                for (int j = 0; j < list.Count; j++)
                {
                    var p_index = this[i - 1].FindIndex(x => list[j].Item2.ParentNode == x);
                    list[j] = Tuple.Create(i - 1, list[j].Item2.ParentNode);
                }
            }

            return list[0].Item2;
        }

        /// <summary>
        /// Get distance of specific two node.
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        public int Distance((int, HtmlNode) node1, (int, HtmlNode) node2)
        {
            if (node1.Item2 == node2.Item2)
                return 0;
            var lca_depth = depth_ref[GetLCANode(new List<Tuple<int, HtmlNode>> { node1.ToTuple(), node2.ToTuple() })];

            return node1.Item1 + node2.Item1 - lca_depth * 2;
        }

        public int Distance(HtmlNode node1, HtmlNode node2)
        {
            return Distance((depth_ref[node1], node1), (depth_ref[node2], node2));
        }

        #region Cluster Supports

        /// <summary>
        /// Gets a list that passes two-dimensional linear clustered items according to a given values.
        /// </summary>
        /// <param name="min_child_count"></param>
        /// <param name="min_diff_rate"></param>
        /// <returns></returns>
        public List<(int, double, HtmlNode, List<HtmlNode>)> LinearClustering(int min_child_count = 2, double min_diff_rate = 0.6)
        {
            var result = new List<(int, double, HtmlNode, List<HtmlNode>)>();
            foreach (var nn in depth_map)
                foreach (var node in nn)
                {
                    if (node.ChildNodes.Count >= min_child_count)
                    {
                        var ff = filtering_child_node(node);
                        var h2 = get_child_node_hashs_2nd(ff);

                        if (h2.Count == 0)
                            continue;

                        var diff = estimate_diff_node_hashs(h2);

                        if (diff >= min_diff_rate)
                            result.Add((ff.Count, diff, node, ff));
                    }
                }

            return result;
        }

        private string get_child_node_hash(HtmlNode node)
        {
            return node.Name + "/" + string.Join("/", node.ChildNodes.Select(x => x.Name));
        }

        private List<HtmlNode> filtering_child_node(HtmlNode node)
        {
            var childs = node.ChildNodes.ToList();
            childs.RemoveAll(x => x.Name == "#comment");
            childs.RemoveAll(x => x.Name == "script");
            childs.RemoveAll(x => x.Name == "#text");
            childs.RemoveAll(x => x.Name == "meta");
            childs.RemoveAll(x => x.Name == "link");
            childs.RemoveAll(x => x.Name == "title");
            childs.RemoveAll(x => x.Name == "head");
            childs.RemoveAll(x => x.Name == "style");
            if (node.Name == "tbody" || node.Name == "table")
                return childs.Where(x => x.Name == "tr").ToList();
            return childs;
        }

        private List<string> get_child_node_hashs_2nd(List<HtmlNode> child_nodes)
        {
            return child_nodes.Select(x => get_child_node_hash(x)).ToList();
        }

        private double estimate_diff_node_hashs(List<string> hashs)
        {
            var hash = new Dictionary<string, int>();

            hashs.ForEach(x =>
            {
                if (!hash.ContainsKey(x))
                    hash.Add(x, 0);
                hash[x] += 1;
            });

            return hash.Select(x => x.Value).Max() / (double)hashs.Count;
        }

        #endregion

        #region Diff

        public bool IsEqualStructure(HtmlTree tree)
        {
            return equal_internal(root_node, tree.root_node);
        }

        List<(HtmlNode, HtmlNode)> diff_node;
        private bool equal_internal(HtmlNode lhs, HtmlNode rhs)
        {
            if (lhs.Name != rhs.Name)
                return false;

            if (diff_node != null && (!lhs.Attributes.SequenceEqual(rhs.Attributes) || (lhs.Name == "#text" && lhs.InnerText != rhs.InnerText)))
                diff_node.Add((lhs, rhs));

            if (lhs.ChildNodes.Count != rhs.ChildNodes.Count)
                return false;

            for (int i = 0; i < lhs.ChildNodes.Count; i++)
                if (!equal_internal(lhs.ChildNodes[i], rhs.ChildNodes[i]))
                    return false;

            return true;
        }

        public (bool, List<(HtmlNode, HtmlNode)>) Diff(HtmlTree tree)
        {
            diff_node = new List<(HtmlNode, HtmlNode)>();
            return (equal_internal(root_node, tree.root_node), diff_node);
        }

        #endregion
    }
}
