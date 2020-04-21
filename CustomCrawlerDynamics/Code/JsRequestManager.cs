// This source code is a part of Custom Copy Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using CustomCrawlerDynamics.Utils;
using Esprima;
using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawlerDynamics.Code
{
    /// <summary>
    /// This class only contains JavaScript files downloaded as web requests.
    /// </summary>
    public class JsRequestManager : ILazy<JsRequestManager>
    {
        Dictionary<string, JsScript> contents = new Dictionary<string, JsScript>();

        public void Clear()
        {
            contents.Clear();
        }

        public bool Register(string url, string js)
        {
            try
            {
                var script = new JsScript(js, url);
                contents.Add(url, script);
                return true;
            }
            catch
            {
            }

            return false;
        }

        public bool Contains(string url) => contents.ContainsKey(url);
        public JsScript this[string key]
        {
            get { return contents[key]; }
        }
    }
}
