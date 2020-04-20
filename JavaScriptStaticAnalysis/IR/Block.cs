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
    public class Block : IRComponent
    {
        public List<Block> ChildBlocks { get; set; }
        public List<IRComponent> Childs { get; set; }

        public bool IsConditional { get; set; }

        public Block(INode node)
            : base(node)
        {
            ChildBlocks = new List<Block>();
            Childs = new List<IRComponent>();
        }
    }
}
