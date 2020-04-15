// This source code is a part of Custom Copy Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaScriptStaticAnalysis
{
    public class UnitAnalysis
    {
        /// <summary>
        /// (Interprocedural Check)
        /// Check whether a input parameter has influenced the function return value.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool ipc_relation_with_return_value(Function func, int param)
        {
            return false;
        }

        /// <summary>
        /// (Interprocedural Check)
        /// Check which function argument affect other argument.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        private bool ipc_relation_with_arguments(Function func, int arg1, int arg2)
        {
            return false;
        }
    }
}
