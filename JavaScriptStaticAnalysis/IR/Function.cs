﻿// This source code is a part of Custom Copy Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaScriptStaticAnalysis.IR
{
    /// <summary>
    /// Contains about named functions, anonymous functions, and lambda functions.
    /// </summary>
    public class Function : Value
    {
        public List<Value> Arguments { get; set; }
        public List<Block> Blocks { get; set; }

        public Function(INode node)
            : base(node)
        {
            Arguments = new List<Value>();
            Blocks = new List<Block>();
        }
    }
}
