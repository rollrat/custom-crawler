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

        private void visit(INode node)
        {
            switch (node.Type)
            {
                    // Expressions
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

                    // Statements
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
                    break;

                    // Declaration
                case Nodes.ClassDeclaration:
                case Nodes.ExportAllDeclaration:
                case Nodes.ExportDefaultDeclaration:
                case Nodes.ExportNamedDeclaration:
                case Nodes.FunctionDeclaration:
                case Nodes.ImportDeclaration:
                case Nodes.VariableDeclaration:

                    // Specifier
                case Nodes.ExportSpecifier:
                case Nodes.ImportDefaultSpecifier:
                case Nodes.ImportNamespaceSpecifier:
                case Nodes.ImportSpecifier:

                // Interfaces or Abstract
                case Nodes.Program:
                    break;

                    // Others
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
                    break;
            }
        }
    }
}
