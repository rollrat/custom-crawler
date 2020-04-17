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

        private Stack<Block> latest_block = new Stack<Block>();

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
        private Block visit_block(Block bb)
        {
            var cur = bb;
            var once_created = false;

            latest_block.Push(cur);

            foreach (var node in bb.Node.ChildNodes)
            {
                var v = visit(node);

                if (v is Block)
                {
                    cur.ChildBlocks.Add(v as Block);

                    // The meaning of the null block is that 
                    // it is associated with the parent node.
                    cur = new Block(null);
                    bb.ChildBlocks.Add(cur);

                    if (once_created)
                        latest_block.Pop();
                    latest_block.Push(cur);

                    once_created = true;
                }
                else if (v != null)
                    cur.Childs.Add(v);
            }

            if (once_created)
                latest_block.Pop();
            latest_block.Pop();

            return bb;
        }

        private IRComponent visitOnStatement(Statement stmt)
        {

            switch (stmt.Type)
            {
                case Nodes.BlockStatement:
                    {
                        var result = new Block(stmt);
                        return visit_block(result);
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
                        var bs = stmt as BreakStatement;

                        if (string.IsNullOrEmpty(bs.Label.Name))
                            latest_block.Peek().ChildBlocks.Add(stmt_exit.Peek());
                        else
                            latest_block.Peek().ChildBlocks.Add(labeled_exit[bs.Label.Name]);
                    }
                    break;
                case Nodes.ContinueStatement:
                    {
                        var bs = stmt as ContinueStatement;

                        if (string.IsNullOrEmpty(bs.Label.Name))
                            latest_block.Peek().ChildBlocks.Add(stmt_cont.Peek());
                        else
                            latest_block.Peek().ChildBlocks.Add(labeled_cont[bs.Label.Name]);
                    }
                    break;

                // case Nodes.DebuggerStatement: // Nothing

                case Nodes.DoWhileStatement:
                    {
                        var result = new Block(stmt);
                        var dws = stmt as DoWhileStatement;

                        var i1 = new Block(null); // Test Block
                        i1.Childs.Add(visitOnExpression(dws.Test));

                        var i2 = new Block(null); // Body Block
                        var i3 = new Block(null); // Exit Block

                        result.ChildBlocks.Add(i2);

                        i1.ChildBlocks.Add(i2);
                        i1.ChildBlocks.Add(i3);

                        i2.ChildBlocks.Add(i1);

                        stmt_cont.Push(i2);
                        stmt_exit.Push(i3);

                        visit_block(result);

                        stmt_cont.Pop();
                        stmt_exit.Pop();

                        return result;
                    }

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
                    break;
            }

            return null;
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
