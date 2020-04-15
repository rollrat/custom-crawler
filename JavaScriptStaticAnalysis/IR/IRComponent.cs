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
    public abstract class IRComponent
    {
        INode node;

        public IRComponent(INode node)
        {
            this.node = node;
        }

        /// <summary>
        /// Translate component to javascript code.
        /// </summary>
        /// <returns></returns>
        public abstract string Rewrite();
    }
}