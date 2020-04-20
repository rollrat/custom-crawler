/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using Esprima;
using Esprima.Ast;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using Jsbeautifier;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Xml;

namespace CustomCrawler
{
    /// <summary>
    /// ScriptViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ScriptViewer : Window
    {
        static bool init = false;

        static void inits()
        {
            if (init) return;
            init = true;
            ICSharpCode.AvalonEdit.Highlighting.Xshd.XshdSyntaxDefinition xshd;

            using (var s = new StringReader(Properties.Resources.JavaScript_Mode))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    xshd = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.LoadXshd(reader);
                }
            }
            HighlightingManager.Instance.RegisterHighlighting("JavaScript1", new[] { ".js" }, ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd, HighlightingManager.Instance));
        }

        Script s1;
        Script s2;
        int l, c;
        bool no_pretty = true;
        string raw;
        string pretty;
        void init_with_code(string js_code, int line, int column, bool no_pretty)
        {
            inits();
            InitializeComponent();

            textEditor.Options.EnableHyperlinks = false;
            textEditor.Options.HighlightCurrentLine = true;
            textEditor.Options.ConvertTabsToSpaces = true;
            textEditor.PreviewMouseLeftButtonDown += TextEditor_PreviewMouseLeftButtonDown;

            var bb = new Beautifier(new BeautifierOptions { IndentWithTabs = false, IndentSize = 4 });
            raw = js_code;
            pretty = bb.Beautify(raw);

            try
            {
                var p1 = new JavaScriptParser(raw, new ParserOptions { Loc = true });
                var p2 = new JavaScriptParser(pretty, new ParserOptions { Loc = true });
                s1 = p1.ParseScript(true);
                s2 = p2.ParseScript(true);

                if (line == -1 || column == -1)
                    (l, c) = (1, 1);
                else
                    (l, c) = (line, column);
                this.no_pretty = no_pretty;

                Loaded += ScriptViewer_Loaded;
            }
            catch
            {
                Beautify.IsEnabled = false;
                textEditor.Text = raw;
            }
        }

        public ScriptViewer(string js_url, int line = -1, int column = -1, bool no_pretty = false)
        {
            init_with_code(NetCommon.DownloadString(js_url), line, column, no_pretty);
        }

        public ScriptViewer(bool foo, string js_code, int line = -1, int column = -1, bool no_pretty = false)
        {
            init_with_code(js_code, line, column, no_pretty);
        }

        private void ScriptViewer_Loaded(object sender, RoutedEventArgs e)
        {
            scroll2pretty(l, c);
        }

        private void scroll2pretty(int line, int column, bool inverse = false)
        {
            if (no_pretty)
                textEditor.Text = raw;
            else
                textEditor.Text = pretty;

            if (no_pretty)
            {
                if (!inverse)
                {
                    var route = new List<int>();
                    find_internal(ref route, s1.ChildNodes, line, column);

                    INode cur = s1;
                    route.ForEach(x => cur = cur.ChildNodes.ElementAt(x));

                    var loc = cur.Location;

                    var s = textEditor.TextArea.Document.GetOffset(new TextLocation(loc.Start.Line, loc.Start.Column));
                    var e = textEditor.TextArea.Document.GetOffset(new TextLocation(loc.End.Line, loc.End.Column));

                    textEditor.ScrollTo(loc.Start.Line, loc.Start.Column);
                    textEditor.SelectionStart = s;
                    textEditor.Select(s, e - s + 1);
                }
                else
                {
                    var route = new List<int>();
                    find_internal(ref route, s2.ChildNodes, line, column);

                    INode cur = s1;
                    route.ForEach(x => cur = cur.ChildNodes.ElementAt(x));

                    var loc = cur.Location;

                    var s = textEditor.TextArea.Document.GetOffset(new TextLocation(loc.Start.Line, loc.Start.Column));
                    var e = textEditor.TextArea.Document.GetOffset(new TextLocation(loc.End.Line, loc.End.Column));

                    textEditor.ScrollTo(loc.Start.Line, loc.Start.Column);
                    textEditor.SelectionStart = s;
                    textEditor.Select(s, e - s + 1);
                }
            }
            else
            {
                var route = new List<int>();
                find_internal(ref route, s1.ChildNodes, line, column);

                INode cur = s2;
                route.ForEach(x => cur = cur.ChildNodes.ElementAt(x));

                var loc = cur.Location;

                var s = textEditor.TextArea.Document.GetOffset(new TextLocation(loc.Start.Line, loc.Start.Column));
                var e = textEditor.TextArea.Document.GetOffset(new TextLocation(loc.End.Line, loc.End.Column));

                textEditor.ScrollTo(loc.Start.Line, loc.Start.Column);
                textEditor.SelectionStart = s;
                textEditor.Select(s, e - s + 1);
            }
        }

        private void TextEditor_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pos = textEditor.GetPositionFromPoint(e.GetPosition(textEditor));
            if (pos.HasValue)
            {
                LC.Text = $"Line:{pos.Value.Line}, Column:{pos.Value.Column}";
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            no_pretty = false;
            scroll2pretty(textEditor.TextArea.Caret.Line, textEditor.TextArea.Caret.Column);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            no_pretty = true;
            scroll2pretty(textEditor.TextArea.Caret.Line, textEditor.TextArea.Caret.Column, true);
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

        void find_internal(ref List<int> result, IEnumerable<INode> node, int line, int column)
        {
            if (node == null || node.Count() == 0)
                return;

            var nrr = node.ToList();
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

            if (z.Location.Start.Line > line || z.Location.End.Line < line)
                return;

            if (z.Location.Start.Line == z.Location.End.Line)
            {
                if (z.Location.Start.Column > column || z.Location.End.Column < column)
                    return;
            }

            result.Add(ii);

            find_internal(ref result, z.ChildNodes, line, column);
        }
    }
}
