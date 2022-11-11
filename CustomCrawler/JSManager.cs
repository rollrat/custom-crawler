/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using Esprima;
using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawler
{
    public class JsManager : ILazy<JsManager>
    {
        Dictionary<string, Script> contents = new Dictionary<string, Script>();

        public void Clear()
        {
            contents.Clear();
        }

        public void Register(string url, string js)
        {
            var parser = new JavaScriptParser(js, new ParserOptions { Loc = true });

            try
            {
                var script = parser.ParseScript(true);
                contents.Add(url, script);
            }
            catch { }
        }

        public bool Contains(string url) => contents.ContainsKey(url);

        public List<INode> FindByLocation(string url, int line, int column)
        {
            var result = new List<INode>();

            if (contents.ContainsKey(url))
            {
                var script = contents[url];
                find_internal(ref result, script.ChildNodes, line, column);
            }

            return result;
        }

        class bb : INode
        {
            public bb(int l, int c)
            {
                Location = new Location(new Position(l, c), new Position(l, c));
            }

            public Nodes Type => Nodes.ArrayExpression;

            public Range Range { get; set; }
            public Location Location { get; set; }

            public IEnumerable<INode> ChildNodes { get; set; }
        }

        void find_internal(ref List<INode> result, IEnumerable<INode> node, int line, int column)
        {
            if (node == null || node.Count() == 0)
                return;

            var nrr = node.Where(x => x != null).ToList();
            var ii = nrr.BinarySearch(new bb(line, column), Comparer<INode>.Create((x, y) =>
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

            if (z == null || z.Location.Start.Line > line || z.Location.End.Line < line)
                return;

            if (z.Location.Start.Line == z.Location.End.Line)
            {
                if (z.Location.Start.Column > column || z.Location.End.Column < column)
                    return;
            }

            result.Add(z);

            find_internal(ref result, z.ChildNodes, line, column);
        }

        public List<IFunction> EnumerateFunctionEntries(string url)
            => EnumerateNodes<IFunction>(url);

        public List<CallExpression> EnumerateCallExpressions(string url)
            => EnumerateNodes<CallExpression>(url);

        public List<T> EnumerateNodes<T>(string url)
            where T : INode
        {
            var result = new List<T>();

            if (contents.ContainsKey(url))
            {
                var script = contents[url];
                enumerate_internal(ref result, script.ChildNodes);
            }

            return result;
        }

        void enumerate_internal<T>(ref List<T> result, IEnumerable<INode> node)
            where T: INode
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
    }
}
