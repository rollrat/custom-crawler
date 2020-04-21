// This source code is a part of Custom Copy Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using Esprima;
using Esprima.Ast;
using Jsbeautifier;
using MasterDevs.ChromeDevTools.Protocol.Chrome.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawlerDynamics.Code
{
    /// <summary>
    /// Script Wrapper
    /// </summary>
    public class JsScript : UnitInfo
    {
        public string RawCode { get; private set; }
        public Script Script { get; private set; }
        public bool IsScriptAvailable { get; private set; }
        public bool IsCreatedFromEval { get { return string.IsNullOrEmpty(Url); } }
        public bool IsEmbeddedInHtml { get; private set; }
        public int EmbeddedLine { get; private set; }
        public int EmbeddedColumn { get; private set; }
        public bool IsFormatted { get; private set; }
        public ScriptParsedEvent ScriptParsed { get; set; }

        public JsScript(string code, string url = "", bool embedded_html = false, int line = 0, int column = 0)
        {
            RawCode = code;
            Url = url;
            IsEmbeddedInHtml = embedded_html;
            EmbeddedLine = line;
            EmbeddedColumn = column;

            var parser = new JavaScriptParser(code, new ParserOptions { Loc = true });

            try
            {
                Script = parser.ParseScript(true);
                IsScriptAvailable = true;
            }
            catch
            {
                IsScriptAvailable = false;
            }
        }

        #region Find By Location

        /// <summary>
        /// Find node from line, column number
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public List<INode> FindByLocation(int line, int column)
        {
            var result = new List<INode>();
            find_node_by_location_internal(ref result, Script.ChildNodes, line, column);
            return result;
        }

        class for_binsearch : INode
        {
            public for_binsearch(int l, int c)
            {
                Location = new Location(new Position(l, c), new Position(l, c));
            }

            public Nodes Type => Nodes.ArrayExpression;

            public Range Range { get; set; }
            public Location Location { get; set; }

            public IEnumerable<INode> ChildNodes { get; set; }
        }

        void find_node_by_location_internal(ref List<INode> result, IEnumerable<INode> node, int line, int column)
        {
            if (node == null || node.Count() == 0)
                return;

            var nrr = node.Where(x => x != null).ToList();
            var ii = nrr.BinarySearch(new for_binsearch(line, column), Comparer<INode>.Create((x, y) =>
            {
                if (x.Location.Start.Line != y.Location.Start.Line)
                    return x.Location.Start.Line.CompareTo(y.Location.Start.Line);
                if (x.Location.Start.Column != y.Location.Start.Column)
                    return x.Location.Start.Column.CompareTo(y.Location.Start.Column);
                return 0;
            }));

            if (node.Count() == 1)
                ii = 0;

            if (ii < 0)
                ii = ~ii - 1;

            if (ii < 0 || ii >= node.Count())
                return;

            var z = node.ElementAt(ii);

            if (z.Location.Start.Line > line || z.Location.End.Line < line)
                return;

            if (z.Location.Start.Line == z.Location.End.Line)
            {
                if (z.Location.Start.Column > column || z.Location.End.Column < column)
                    return;
            }

            result.Add(z);

            find_node_by_location_internal(ref result, z.ChildNodes, line, column);
        }

        #endregion

        #region Indexing By Location

        /// <summary>
        /// Indexing nodes from line, column number
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public List<int> IndexingByLocation(int line, int column)
        {
            var result = new List<int>();
            indexing_by_location_internal(ref result, Script.ChildNodes, line, column);
            return result;
        }

        void indexing_by_location_internal(ref List<int> result, IEnumerable<INode> node, int line, int column)
        {
            if (node == null || node.Count() == 0)
                return;

            var nrr = node.ToList();
            var ii = nrr.BinarySearch(new for_binsearch(line, column), Comparer<INode>.Create((x, y) =>
            {
                if (x.Location.Start.Line != y.Location.Start.Line)
                    return x.Location.Start.Line.CompareTo(y.Location.Start.Line);
                if (x.Location.Start.Column != y.Location.Start.Column)
                    return x.Location.Start.Column.CompareTo(y.Location.Start.Column);
                return 0;
            }));

            if (node.Count() == 1)
                ii = 0;

            if (ii < 0)
                ii = ~ii - 1;

            if (ii < 0 || ii >= node.Count())
                return;

            var z = node.ElementAt(ii);

            if (z.Location.Start.Line > line || z.Location.End.Line < line)
                return;

            if (z.Location.Start.Line == z.Location.End.Line)
            {
                if (z.Location.Start.Column > column || z.Location.End.Column < column)
                    return;
            }

            result.Add(ii);

            indexing_by_location_internal(ref result, z.ChildNodes, line, column);
        }

        #endregion

        /// <summary>
        /// Outputs the script converted into a beautiful form.
        /// </summary>
        /// <returns></returns>
        public JsScript GetFormattedScript()
        {
            if (IsFormatted)
                return this;

            var tool = new Beautifier(new BeautifierOptions { IndentWithTabs = false, IndentSize = 4 });
            var pretty = tool.Beautify(RawCode);

            return new JsScript(pretty, Url, IsEmbeddedInHtml, EmbeddedLine, EmbeddedColumn) { IsFormatted = true };
        }

        #region Enumerate

        public List<IFunction> EnumerateFunctionEntries()
            => EnumerateNodes<IFunction>();

        public List<CallExpression> EnumerateCallExpressions()
            => EnumerateNodes<CallExpression>();

        /// <summary>
        /// List all items in parse tree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> EnumerateNodes<T>()
            where T : INode
        {
            var result = new List<T>();
            enumerate_internal(ref result, Script.ChildNodes);
            return result;
        }

        void enumerate_internal<T>(ref List<T> result, IEnumerable<INode> node)
            where T : INode
        {
            if (node == null || node.Count() == 0)
                return;

            var nrr = node.Where(x => x != null).ToList();
            foreach (var nn in nrr)
            {
                if (nn is T)
                    result.Add((T)nn);
                enumerate_internal(ref result, nn.ChildNodes);
            }
        }

        #endregion

        #region Child Scripts

        public List<(JsScript, int, int, string)> Childs { get; private set; }

        /// <summary>
        /// Register the contents of this script.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <param name="function"></param>
        public void AppendChilds(JsScript script, int line, int column, string function)
        {
            Childs.Add((script, line, column, function));
        }

        /// <summary>
        /// Return all scripts created at input location.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public List<(JsScript, int, int, string)> FindChildsByLocation(int line, int column)
        {
            var target = FindByLocation(line, column);
            var result = new List<(JsScript, int, int, string)>();

            foreach (var child in Childs)
            {
                var src = FindByLocation(child.Item2, child.Item3);
                var lca = lca_nodes(target, src);

                // Check if the target terminal node overlaps
                if (lca == target.Count)
                    result.Add(child);
            }

            return result;
        }

        #endregion

        #region Common Internal Utils

        /// <summary>
        /// Get index of least common ancestor.
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        private static int lca_nodes(List<INode> n1, List<INode> n2)
        {
            var index = 0;
            var count = Math.Min(n1.Count, n2.Count);

            for (; index < count; index++)
            {
                if (n1[index].Type != n2[index].Type)
                    break;
                if (n1[index].Location != n2[index].Location)
                    break;
            }

            return index - 1;
        }

        #endregion
    }
}
