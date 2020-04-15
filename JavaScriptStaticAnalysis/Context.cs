// This source code is a part of Custom Copy Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using Esprima;
using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaScriptStaticAnalysis
{
    public class Context
    {
        Script script;

        public static Context CreateInstance(string js)
        {
            var cc = new Context();
            var parser = new JavaScriptParser(js, new ParserOptions { Loc = true });
            cc.script = parser.ParseScript(true);
            return cc;
        }

        /// <summary>
        /// Check References for Sepecific Node
        /// 
        /// If you enter `d` node in the following code,
        /// a = 30;
        /// a = 10;
        /// b = 20;
        /// d = a + b;
        /// then returns the nodes that were last used with values a and b needed to compute d.
        /// 
        /// </summary>
        /// <param name="node"></param>
        private void check_reference(INode node)
        {

        }
    }
}
