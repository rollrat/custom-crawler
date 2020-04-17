// This source code is a part of Custom Copy Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaScriptStaticAnalysis.IR
{
    public class CallExpression : Value
    {
        public Function Callee { get; set; }

        public CallExpression(INode node)
            : base(node)
        {
        }
    }
}
