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
        Dictionary<string, Program> contents = new Dictionary<string, Program>();

        public void Register(string url, string js)
        {
            var parser = new JavaScriptParser(js);

            try
            {
                var program = parser.ParseProgram();
                contents.Add(url, program);
            }
            catch { }
        }

        public bool Contains(string url) => contents.ContainsKey(url);

        public List<INode> FindByLocation(string url, int line, int column)
        {
            throw new NotImplementedException();
        }
    }
}
