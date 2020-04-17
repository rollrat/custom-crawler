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
    /// <summary>
    /// Organize parse tree nodes in a form that makes static analysis easy.
    /// </summary>
    public class IRBuilder
    {
        Block entry_point;

        public IRBuilder(Script script)
        {
            visit(script.ChildNodes.First());
        }

        public static Type[] NodeTypes = new Type[] 
        {
            typeof(ArgumentListElement),
            typeof(ArrayExpression),
            typeof(ArrayExpressionElement),
            typeof(ArrayPattern),
            typeof(ArrowFunctionExpression),
            typeof(ArrowParameterPlaceHolder),
            typeof(AssignmentExpression),
            typeof(AssignmentOperator),
            typeof(AssignmentPattern),
            typeof(AwaitExpression),
            typeof(BinaryExpression),
            typeof(BinaryOperator),
            typeof(BindingIdentifier),
            typeof(BindingPattern),
            typeof(BlockStatement),
            typeof(BreakStatement),
            typeof(CallExpression),
            typeof(CatchClause),
            typeof(ClassBody),
            typeof(ClassDeclaration),
            typeof(ClassExpression),
            typeof(ClassProperty),
            typeof(ComputedMemberExpression),
            typeof(ConditionalExpression),
            typeof(ContinueStatement),
            typeof(DebuggerStatement),
            typeof(Directive),
            typeof(DoWhileStatement),
            typeof(EmptyStatement),
            typeof(ExportAllDeclaration),
            typeof(ExportDeclaration),
            typeof(ExportDefaultDeclaration),
            typeof(ExportNamedDeclaration),
            typeof(ExportSpecifier),
            typeof(Expression),
            typeof(ExpressionStatement),
            typeof(ForInStatement),
            typeof(ForOfStatement),
            typeof(ForStatement),
            typeof(FunctionDeclaration),
            typeof(FunctionExpression),
            typeof(IArrayPatternElement),
            typeof(IDeclaration),
            typeof(Identifier),
            typeof(IfStatement),
            typeof(IFunction),
            typeof(IFunctionDeclaration),
            typeof(IFunctionParameter),
            typeof(Import),
            typeof(ImportDeclaration),
            typeof(ImportDeclarationSpecifier),
            typeof(ImportDefaultSpecifier),
            typeof(ImportNamespaceSpecifier),
            typeof(ImportSpecifier),
            typeof(INode),
            typeof(IStatementListItem),
            typeof(LabeledStatement),
            typeof(Literal),
            typeof(MemberExpression),
            typeof(MetaProperty),
            typeof(MethodDefinition),
            typeof(Module),
            typeof(NewExpression),
            typeof(Node),
            typeof(NodeExtensions),
            typeof(NodeList),
            typeof(Nodes),
            typeof(ObjectExpression),
            typeof(ObjectExpressionProperty),
            typeof(ObjectPattern),
            typeof(ObjectPatternProperty),
            typeof(Program),
            typeof(Property),
            typeof(PropertyKind),
            typeof(PropertyValue),
            typeof(RegexValue),
            typeof(RestElement),
            typeof(ReturnStatement),
            typeof(Script),
            typeof(SequenceExpression),
            typeof(SpreadElement),
            typeof(Statement),
            typeof(StaticMemberExpression),
            typeof(Super),
            typeof(SwitchCase),
            typeof(SwitchStatement),
            typeof(TaggedTemplateExpression),
            typeof(TemplateElement),
            typeof(TemplateLiteral),
            typeof(ThisExpression),
            typeof(ThrowStatement),
            typeof(TryStatement),
            typeof(UnaryExpression),
            typeof(UnaryOperator),
            typeof(UpdateExpression),
            typeof(VariableDeclaration),
            typeof(VariableDeclarationKind),
            typeof(VariableDeclarator),
            typeof(WhileStatement),
            typeof(WithStatement),
            typeof(YieldExpression),
        };

        private IRComponent visit(INode node)
        {
            switch (node.Type)
            {
                    /* Expressions */
                case Nodes.ArrayExpression:
                case Nodes.ArrowFunctionExpression:
                case Nodes.AssignmentExpression:
                case Nodes.AwaitExpression:
                case Nodes.BinaryExpression:
                case Nodes.CallExpression:
                case Nodes.ClassExpression:
                case Nodes.ConditionalExpression:
                case Nodes.LogicalExpression:
                case Nodes.FunctionExpression:
                case Nodes.MemberExpression:
                case Nodes.NewExpression:
                case Nodes.ObjectExpression:
                case Nodes.SequenceExpression:
                case Nodes.TaggedTemplateExpression:
                case Nodes.ThisExpression:
                case Nodes.UnaryExpression:
                case Nodes.UpdateExpression:
                case Nodes.YieldExpression:
                    return visitOnExpression(node as Expression);

                    /* Statements */
                case Nodes.BlockStatement:
                case Nodes.BreakStatement:
                case Nodes.ContinueStatement:
                case Nodes.DebuggerStatement:
                case Nodes.DoWhileStatement:
                case Nodes.EmptyStatement:
                case Nodes.ExpressionStatement:
                case Nodes.ForInStatement:
                case Nodes.ForOfStatement:
                case Nodes.ForStatement:
                case Nodes.IfStatement:
                case Nodes.LabeledStatement:
                case Nodes.ReturnStatement:
                case Nodes.SwitchStatement:
                case Nodes.ThrowStatement:
                case Nodes.TryStatement:
                case Nodes.WhileStatement:
                case Nodes.WithStatement:
                    return visitOnStatement(node as Statement);

                    /* Declaration */
                case Nodes.ClassDeclaration:
                case Nodes.ExportAllDeclaration:
                case Nodes.ExportDefaultDeclaration:
                case Nodes.ExportNamedDeclaration:
                case Nodes.FunctionDeclaration:
                case Nodes.ImportDeclaration:
                case Nodes.VariableDeclaration:
                    return visitOnDeclaration(node as IDeclaration);

                    /* Specifier */
                case Nodes.ExportSpecifier:
                case Nodes.ImportDefaultSpecifier:
                case Nodes.ImportNamespaceSpecifier:
                case Nodes.ImportSpecifier:

                    /* Others */
                case Nodes.Program:
                case Nodes.ArrayPattern:
                case Nodes.ArrowParameterPlaceHolder:
                case Nodes.AssignmentPattern:
                case Nodes.CatchClause:
                case Nodes.ClassBody:
                case Nodes.Identifier:
                case Nodes.Import:
                case Nodes.Literal:
                case Nodes.MetaProperty:
                case Nodes.MethodDefinition:
                case Nodes.ObjectPattern:
                case Nodes.Property:
                case Nodes.RestElement:
                case Nodes.SpreadElement:
                case Nodes.Super:
                case Nodes.SwitchCase:
                case Nodes.TemplateElement:
                case Nodes.TemplateLiteral:
                case Nodes.VariableDeclarator:
                    return new Value(node);

                default:
                    throw new Exception("Untriggered type found!");
            }
        }

        private Value visitOnExpression(Expression expr)
        {
            switch (expr.Type)
            {
                case Nodes.ArrayExpression:
                case Nodes.ArrowFunctionExpression:
                case Nodes.AssignmentExpression:
                case Nodes.AwaitExpression:
                case Nodes.BinaryExpression:
                case Nodes.CallExpression:
                case Nodes.ClassExpression:
                case Nodes.ConditionalExpression:
                case Nodes.LogicalExpression:
                case Nodes.FunctionExpression:
                case Nodes.MemberExpression:
                case Nodes.NewExpression:
                case Nodes.ObjectExpression:
                case Nodes.SequenceExpression:
                case Nodes.TaggedTemplateExpression:
                case Nodes.ThisExpression:
                case Nodes.UnaryExpression:
                case Nodes.UpdateExpression:
                case Nodes.YieldExpression:
                    break;
            }

            // never triggered.
            throw new InvalidOperationException();
        }

        private Dictionary<string, Block> labeled_cont = new Dictionary<string, Block>();
        private Dictionary<string, Block> labeled_exit = new Dictionary<string, Block>();
        private Stack<Block> stmt_cont = new Stack<Block>();
        private Stack<Block> stmt_exit = new Stack<Block>();
        private Stack<Block> func_exit = new Stack<Block>();
        private Stack<Block>  try_exit = new Stack<Block>();

        /// <summary>
        /// Assuming you have the following example
        /// 
        /// {
        ///     a = 1;
        ///     {
        ///         b = 2;
        ///     }
        ///     c = 3;
        ///     d = 4;
        ///     {
        ///         e = 5;
        ///     }
        ///     f = 6;
        /// }
        /// 
        /// This code is expressed as follows:
        /// 
        /// block
        ///   - expr[a=1]  <= Put all statements in the parent block 
        ///   + block         before the block appears.
        ///     - expr[b=2]
        ///   + block
        ///     - expr[c=3]  <= If more than one block has been created,
        ///     - expr[d=4]     it is put into a new block. The reason 
        ///   + block           for this is due to the principle of 
        ///     - expr[e=5]     pre-execution.
        ///   + block
        ///     - expr[f=6]
        ///     
        /// The reason you can make it this way is because goto
        /// doesn't exist on javascript.
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        private Block visit_block(Block bb, Statement body)
        {
            var cur = bb;

            foreach (var node in body.ChildNodes)
            {
                var v = visit(node);

                if (v == null)
                    continue;

                v.Parent = bb;

                if (v is Block)
                {
                    cur.ChildBlocks.Add(v as Block);

                    // The meaning of the null block is that 
                    // it is associated with the parent node.
                    cur = new Block(null);
                    bb.ChildBlocks.Add(cur);
                }
                else
                    cur.Childs.Add(v);
            }

            return bb;
        }

        private IRComponent visitOnStatement(Statement stmt)
        {
            switch (stmt.Type)
            {
                case Nodes.BlockStatement:
                    {
                        var result = new Block(stmt);
                        return visit_block(result, stmt);
                    }

                /// There are several elements to handle.
                /// Check this.
                /// 
                /// {
                ///     z = 0;
                ///     for (...)   <================*
                ///     {                            |
                ///         a = 1;                   |
                ///         b = 2;                   |
                ///         {                        |
                ///             c = 1;               |
                ///             break; =========*    |
                ///             d = 3;          |    |
                ///             continue; ======+====*
                ///         }                   |
                ///         e = 4;              |
                ///     }                       |
                ///     k = 9;   <==============*
                /// }
                /// 
                /// To find the branch of a break, you need to make a temporary 
                /// block that will be executed after the statement when you visit 
                /// for or while statements that can use break.
                /// 
                case Nodes.BreakStatement:
                    {
                        var result = new Block(stmt);
                        var bs = stmt as BreakStatement;

                        if (string.IsNullOrEmpty(bs.Label.Name))
                            result.ChildBlocks.Add(stmt_exit.Peek());
                        else
                            result.ChildBlocks.Add(labeled_exit[bs.Label.Name]);

                        return result;
                    }
                case Nodes.ContinueStatement:
                    {
                        var result = new Block(stmt);
                        var bs = stmt as ContinueStatement;

                        if (string.IsNullOrEmpty(bs.Label.Name))
                            result.ChildBlocks.Add(stmt_exit.Peek());
                        else
                            result.ChildBlocks.Add(labeled_exit[bs.Label.Name]);

                        return result;
                    }

                // case Nodes.DebuggerStatement: // Nothing

                case Nodes.DoWhileStatement:
                    {
                        var result = new Block(stmt);
                        var dws = stmt as DoWhileStatement;

                        var i1 = new Block(dws.Test) { Parent = result, IsConditional = true }; // Test Block
                        i1.Childs.Add(visitOnExpression(dws.Test));

                        var i2 = new Block(null) { Parent = result }; // Body Block
                        var i3 = new Block(null) { Parent = result }; // Exit Block

                        result.ChildBlocks.Add(i2);

                        i1.ChildBlocks.Add(i2);
                        i1.ChildBlocks.Add(i3);

                        stmt_cont.Push(i2);
                        stmt_exit.Push(i3);

                        visit_block(i2, dws.Body);

                        stmt_cont.Pop();
                        stmt_exit.Pop();

                        i2.ChildBlocks.Add(i1);

                        return result;
                    }

                // case Nodes.EmptyStatement: // Nothing

                case Nodes.ExpressionStatement:
                    {
                        var expr = stmt as ExpressionStatement;
                        return new Expr(stmt) { Value = visitOnExpression(expr.Expression) };
                    }

                case Nodes.ForInStatement:
                    {
                        var result = new Block(stmt);
                        var fis = stmt as ForInStatement;

                        var i1 = new Block(null) { Parent = result }; // Next Block
                        var i2 = new Block(null) { Parent = result }; // Body Block
                        var i3 = new Block(null) { Parent = result }; // Exit Block

                        result.ChildBlocks.Add(i1);

                        i1.ChildBlocks.Add(i2);
                        i1.ChildBlocks.Add(i3);

                        stmt_cont.Push(i1);
                        stmt_exit.Push(i3);

                        visit_block(i2, fis.Body);

                        stmt_cont.Pop();
                        stmt_exit.Pop();

                        i2.ChildBlocks.Add(i1);

                        return result;
                    }

                case Nodes.ForOfStatement:
                    {
                        var result = new Block(stmt);
                        var fis = stmt as ForOfStatement;

                        var i1 = new Block(null) { Parent = result }; // Next Block
                        var i2 = new Block(null) { Parent = result }; // Body Block
                        var i3 = new Block(null) { Parent = result }; // Exit Block

                        result.ChildBlocks.Add(i1);

                        i1.ChildBlocks.Add(i2);
                        i1.ChildBlocks.Add(i3);

                        stmt_cont.Push(i1);
                        stmt_exit.Push(i3);

                        visit_block(i2, fis.Body);

                        stmt_cont.Pop();
                        stmt_exit.Pop();

                        i2.ChildBlocks.Add(i1);

                        return result;
                    }

                case Nodes.ForStatement:
                    {
                        var result = new Block(stmt);
                        var fs = stmt as ForStatement;

                        var i0 = new Block(fs.Init) { Parent = result }; // Init Block
                        i0.Childs.Add(visit(fs.Init));

                        var i1 = new Block(fs.Test) { Parent = result, IsConditional = true }; // Test Block
                        i1.Childs.Add(visitOnExpression(fs.Test));
                        i0.ChildBlocks.Add(i1);

                        var i2 = new Block(fs.Update) { Parent = result }; // Update Block
                        i2.Childs.Add(visitOnExpression(fs.Update));
                        i2.ChildBlocks.Add(i1);

                        var i3 = new Block(null) { Parent = result }; // Body Block
                        var i4 = new Block(null) { Parent = result }; // Exit Block
                        i1.ChildBlocks.Add(i3);
                        i1.ChildBlocks.Add(i4);

                        result.ChildBlocks.Add(i0);

                        stmt_cont.Push(i2);
                        stmt_exit.Push(i4);

                        visit_block(i3, fs.Body);

                        stmt_cont.Pop();
                        stmt_exit.Pop();

                        i3.ChildBlocks.Add(i2);

                        return result;
                    }

                case Nodes.IfStatement:
                    {
                        var result = new Block(stmt);
                        var ifs = stmt as IfStatement;

                        var i0 = new Block(ifs.Test) { Parent = result, IsConditional = true }; // Test Block
                        var i1 = new Block(null) { Parent = result }; // If Block
                        var i2 = new Block(null) { Parent = result }; // Else Block
                        var i3 = new Block(null) { Parent = result }; // Exit Block

                        i0.ChildBlocks.Add(i1);
                        i0.ChildBlocks.Add(i2);

                        i0.Childs.Add(visitOnExpression(ifs.Test));

                        result.ChildBlocks.Add(result);

                        visit_block(i1, ifs.Consequent);
                        visit_block(i2, ifs.Alternate);

                        i1.ChildBlocks.Add(i3);
                        i2.ChildBlocks.Add(i3);

                        return result;
                    }

                case Nodes.LabeledStatement:
                    {
                        var result = new Block(stmt);
                        var ls = stmt as LabeledStatement;

                        var i1 = new Block(null) { Parent = result }; // Enter Block
                        var i2 = new Block(null) { Parent = result }; // Exit Block

                        result.ChildBlocks.Add(i1);

                        labeled_cont.Add(ls.Label.Name, i1);
                        labeled_exit.Add(ls.Label.Name, i2);

                        visit_block(i1, ls.Body);

                        labeled_cont.Remove(ls.Label.Name);
                        labeled_exit.Remove(ls.Label.Name);

                        i1.ChildBlocks.Add(i2);

                        return result;
                    }

                case Nodes.ReturnStatement:
                    {
                        var result = new Block(stmt);
                        var rs = stmt as ReturnStatement;

                        result.Childs.Add(visitOnExpression(rs.Argument));
                        result.ChildBlocks.Add(func_exit.Peek());

                        return result;
                    }

                case Nodes.SwitchStatement:
                    {
                        var result = new Block(stmt);
                        var ss = stmt as SwitchStatement;

                        var i0 = new Block(ss.Discriminant) { Parent = result, IsConditional = true }; // Test Block
                        i0.Childs.Add(visitOnExpression(ss.Discriminant));

                        result.Childs.Add(i0);

                        foreach (var sc in ss.Cases)
                        {
                            var i1 = new Block(sc.Test) { Parent = result, IsConditional = true };
                            i1.Childs.Add(visitOnExpression(sc.Test));
                            foreach (var li in sc.Consequent)
                                i1.Childs.Add(visit(li));
                            i0.ChildBlocks.Add(i1);
                        }

                        return result;
                    }

                case Nodes.ThrowStatement:
                    {
                        var result = new Block(stmt);
                        var ts = stmt as ThrowStatement;

                        result.Childs.Add(visitOnExpression(ts.Argument));
                        result.ChildBlocks.Add(try_exit.Peek());

                        return result;
                    }

                case Nodes.TryStatement:
                    {
                        var result = new Block(stmt);
                        var ts = stmt as TryStatement;

                        var i1 = new Block(null) { Parent = result }; // Try Block
                        var i2 = new Block(ts.Handler) { Parent = result }; // Catch Block
                        var i3 = new Block(null) { Parent = result }; // Finalize Block
                        var i4 = new Block(null) { Parent = result }; // Exit Block

                        i1.ChildBlocks.Add(i3);
                        i2.ChildBlocks.Add(i3);
                        i3.ChildBlocks.Add(i4);

                        visit_block(i1, ts.Block);
                        visit_block(i2, ts.Handler.Body);
                        visit_block(i3, ts.Finalizer);

                        return result;
                    }

                case Nodes.WhileStatement:
                    {
                        var result = new Block(stmt);
                        var ws = stmt as WhileStatement;

                        var i1 = new Block(ws.Test) { Parent = result, IsConditional = true }; // Test Block
                        i1.Childs.Add(visitOnExpression(ws.Test));

                        var i2 = new Block(null) { Parent = result }; // Body Block
                        var i3 = new Block(null) { Parent = result }; // Exit Block

                        result.ChildBlocks.Add(i1);

                        i1.ChildBlocks.Add(i2);
                        i1.ChildBlocks.Add(i3);

                        stmt_cont.Push(i2);
                        stmt_exit.Push(i3);

                        visit_block(result, ws.Body);

                        stmt_cont.Pop();
                        stmt_exit.Pop();

                        i2.ChildBlocks.Add(i1);

                        return result;
                    }

                case Nodes.WithStatement:
                    {
                        var result = new Block(stmt);
                        var ws = stmt as WithStatement;

                        result.Childs.Add(visitOnExpression(ws.Object));

                        visit_block(result, ws.Body);

                        return result;
                    }
            }

            // never triggered.
            throw new InvalidOperationException();
        }

        private Value visitOnDeclaration(IDeclaration decl)
        {
            switch (decl.Type)
            {

                case Nodes.ClassDeclaration:
                case Nodes.ExportAllDeclaration:
                case Nodes.ExportDefaultDeclaration:
                case Nodes.ExportNamedDeclaration:
                case Nodes.FunctionDeclaration:
                case Nodes.ImportDeclaration:
                case Nodes.VariableDeclaration:
                    break;
            }

            // never triggered.
            throw new InvalidOperationException();
        }
    }
}
