/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CustomCrawler
{
    public class HtmlCAL
    {
        private static List<string> split_token(string str)
        {
            var result = new List<string>();
            for (int i = 0; i < str.Length; i++)
            {
                var builder = new StringBuilder();
                bool skip = false;
                for (; i < str.Length; i++)
                {
                    if (str[i] == ',' && skip == false)
                    {
                        result.Add(builder.ToString());
                        break;
                    }
                    if (str[i] == '[')
                        skip = true;
                    else if (str[i] == ']' && skip)
                        skip = false;
                    builder.Append(str[i]);
                }
                if (i == str.Length)
                    result.Add(builder.ToString());
            }
            return result;
        }

        private static int cal(string pp, int v)
        {
            // 1. i*b
            // 2. a+i*c
            if (pp.Contains('+'))
                return Convert.ToInt32(pp.Split('+')[0].Trim()) + v * Convert.ToInt32(pp.Split('+')[1].Split('*')[1].Trim());
            return v * Convert.ToInt32(pp.Split('*')[1].Trim());
        }

        public static List<string> Calculate(string pattern, HtmlNode root_node)
        {
            var result = new List<string>();
            var split = split_token(pattern);
            var xpath = split[0].Trim();
            var extend = split.Count > 1 ? split[1].Trim() : "";
            var snodes = new List<HtmlNode>();
            int ptr = 1;

            HtmlNode explore = null;

            if (!(xpath.Contains("{") && xpath.Contains("}")))
            {
                explore = root_node.SelectSingleNode(xpath);
                if (string.IsNullOrEmpty(extend) || extend == "#text")
                    result.Add(explore.InnerText);
                else if (extend == "#htext")
                    result.Add(string.Join("", explore.ChildNodes.Where(x => x.Name == "#text").Select(x => x.InnerText.Trim())));
                else if (extend == "#html")
                    result.Add(explore.InnerHtml);
                else if (extend == "#ohtml")
                    result.Add(explore.OuterHtml);
                else if (extend.StartsWith("#attr"))
                {
                    var val = extend.Split('[')[1].Split(']')[0].Trim();
                    result.Add(explore.GetAttributeValue(val, ""));
                }
            }
            else
            {
                var _split = xpath.Split('{').ToList();
                _split.RemoveAt(0);
                var split2 = _split.Select(x => x.Split('}')[0]);
                for (int i = 0; ; i++)
                {
                    var builder = new StringBuilder(xpath);
                    foreach (var sss in split2)
                    {
                        builder.Replace("{" + sss + "}", cal(sss, i).ToString());
                    }
                    var pattern2 = builder.ToString();
                    if (pattern2.Contains("#text"))
                    {
                        pattern2 = pattern2.Remove(pattern2.IndexOf("/#text"));
                    }
                    var node = root_node.SelectSingleNode(pattern2);
                    if (node == null)
                        break;
                    snodes.Add(node);
                }
            }

            if (result.Count > 0)
                ptr++;

            if (extend.StartsWith(".") || snodes.Count > 0)
            {
                List<HtmlNode> nodes;
                if (snodes.Count == 0 && explore != null)
                {
                    nodes = explore.SelectNodes(extend).ToList();
                    ptr++;
                }
                else
                    nodes = snodes;
                if (split.Count >= 3 || (snodes.Count > 0 && split.Count >= 2))
                {
                    if (split.Count >= 3 && snodes.Count == 0)
                        extend = split[2].Trim();
                    else
                        extend = split[1].Trim();
                    if (extend.StartsWith("#attr"))
                    {
                        // #attr[src] ...
                        var val = extend.Split('[')[1].Split(']')[0].Trim();
                        nodes.ToList().ForEach(x => result.Add(x.GetAttributeValue(val, "")));
                    }
                    else if (extend == "#text")
                        nodes.ToList().ForEach(x => result.Add(x.InnerText));
                    else if (extend == "#htext")
                        nodes.ToList().ForEach(y => result.Add(string.Join("", y.ChildNodes.Where(x => x.Name == "#text").Select(x => x.InnerText.Trim()))));
                    else if (extend == "#html")
                        nodes.ToList().ForEach(x => result.Add(x.InnerHtml));
                    else if (extend == "#ohtml")
                        nodes.ToList().ForEach(x => result.Add(x.OuterHtml));
                    ptr++;
                }
                else
                {
                    nodes.ToList().ForEach(x => result.Add(x.InnerText));
                }
            }

            while (ptr < split.Count)
            {
                var token = split[ptr].Trim();

                if (token.StartsWith("#split"))
                {
                    // #split[text, index]
                    var val = token.Split(new char[] { '[' }, 2)[1];
                    val = val.Remove(val.LastIndexOf(']'));
                    var text = val.Remove(val.LastIndexOf(',')).Trim();
                    int index = Convert.ToInt32(val.Split(',').Last().Trim());
                    var tmp = new List<string>();

                    if (index != -1)
                        result.ForEach(x => tmp.Add(x.Split(new string[] { text }, StringSplitOptions.None)[index]));
                    else
                        result.ForEach(x => tmp.Add(x.Split(new string[] { text }, StringSplitOptions.None).Last()));
                    result = tmp;
                }
                else if (token.StartsWith("#regex"))
                {
                    // #regex[pattern]
                    var val = token.Split(new char[] { '[' }, 2)[1];
                    val = val.Remove(val.LastIndexOf(']'));
                    var regex = new Regex(val);
                    var tmp = new List<string>();
                    result.ForEach(x => tmp.Add(regex.Match(x).Value));
                    result = tmp;
                }
                else if (token.StartsWith("#gregex"))
                {
                    // #gregex[pattern]
                    var val = token.Split(new char[] { '[' }, 2)[1];
                    val = val.Remove(val.LastIndexOf(']'));
                    var regex = new Regex(val);
                    var tmp = new List<string>();
                    result.ForEach(x => tmp.Add(regex.Match(x).Groups[1].Value));
                    result = tmp;
                }
                else if (token.StartsWith("#back"))
                {
                    var val = token.Split(new char[] { '[' }, 2)[1];
                    val = val.Remove(val.LastIndexOf(']'));
                    var tmp = new List<string>();
                    result.ForEach(x => tmp.Add(x + val));
                    result = tmp;
                }
                else if (token.StartsWith("#front"))
                {
                    var val = token.Split(new char[] { '[' }, 2)[1];
                    val = val.Remove(val.LastIndexOf(']'));
                    var tmp = new List<string>();
                    result.ForEach(x => tmp.Add(val + x));
                    result = tmp;
                }

                ptr++;
            }

            return result.Select(x => x.Trim()).ToList();
        }
    }
}
