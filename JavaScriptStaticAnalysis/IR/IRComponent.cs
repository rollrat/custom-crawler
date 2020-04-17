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
    public abstract class IRComponent
    {
        public INode Node { get; set; }
        public IRComponent Parent { get; set; }
        public List<IRComponent> Parents { get; set; }

        public IRComponent(INode node)
        {
            Node = node;
            Parents = new List<IRComponent>();
        }
    }
}
