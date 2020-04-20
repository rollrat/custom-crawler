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
    public class Expr : Value
    {
        public Value Value { get; set; }
        public bool IsStatement { get; set; }
        public string Id { get; set; }

        public List<IRComponent> Childs { get; set; } // Uses
        public IRComponent Body { get; set; } // If is lambda function
        public List<IRComponent> Defs { get; set; }
        public List<IRComponent> Args { get; set; }

        // Class Property
        public bool IsClass { get; set; }
        public IRComponent Super { get; set; }
        public ClassBody ClassBody { get; set; }

        // a.b
        public bool IsMember { get; set; }
        public IRComponent Member { get; set; }

        // new
        public bool IsNew { get; set; }

        public Expr(INode node)
            : base(node)
        {
            Childs = new List<IRComponent>();
            Defs = new List<IRComponent>();
            Args = new List<IRComponent>();
        }
    }
}
