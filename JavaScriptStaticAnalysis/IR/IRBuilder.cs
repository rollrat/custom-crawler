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
            switch (node.GetType().Name)
            {
                    // Expressions
                case "ArrayExpression":
                case "ArrowFunctionExpression":
                case "AssignmentExpression":
                case "AwaitExpression":
                case "BinaryExpression":
                case "CallExpression":
                case "ClassExpression":
                case "ComputedMemberExpression":
                case "ConditionalExpression":
                case "FunctionExpression":
                case "MemberExpression":
                case "NewExpression":
                case "ObjectExpression":
                case "SequenceExpression":
                case "StaticMemberExpression":
                case "TaggedTemplateExpression":
                case "ThisExpression":
                case "UnaryExpression":
                case "UpdateExpression":
                case "YieldExpression":
                    break;

                    // Statements
                case "BlockStatement":
                case "BreakStatement":
                case "ContinueStatement":
                case "DebuggerStatement":
                case "DoWhileStatement":
                case "EmptyStatement":
                case "ExpressionStatement":
                case "ForInStatement":
                case "ForOfStatement":
                case "ForStatement":
                case "IfStatement":
                case "LabeledStatement":
                case "ReturnStatement":
                case "SwitchStatement":
                case "ThrowStatement":
                case "TryStatement":
                case "WhileStatement":
                case "WithStatement":
                    break;

                    // Declaration
                case "ClassDeclaration":
                case "ExportAllDeclaration":
                case "ExportDeclaration":
                case "ExportDefaultDeclaration":
                case "ExportNamedDeclaration":
                case "FunctionDeclaration":
                case "IDeclaration":
                case "IFunctionDeclaration":
                case "ImportDeclaration":
                case "VariableDeclaration":

                    // Operator
                case "AssignmentOperator":
                case "BinaryOperator":
                case "UnaryOperator":
                    break;

                    // Interfaces or Abstract
                case "ArgumentListElement":
                case "ArrayExpressionElement":
                case "BindingIdentifier":
                case "BindingPattern":
                case "ExportSpecifier":
                case "Expression":
                case "IArrayPatternElement":
                case "IFunction":
                case "IFunctionParameter":
                case "ImportDeclarationSpecifier":
                case "INode":
                case "IStatementListItem":
                case "ObjectExpressionProperty":
                case "ObjectPatternProperty":
                case "Program":
                case "PropertyValue":
                case "Statement":
                    break;

                    // Others
                case "ArrayPattern":
                case "ArrowParameterPlaceHolder":
                case "AssignmentPattern":
                case "CatchClause":
                case "ClassBody":
                case "ClassProperty":
                case "Directive":
                case "Identifier":
                case "Import":
                case "ImportDefaultSpecifier":
                case "ImportNamespaceSpecifier":
                case "ImportSpecifier":
                case "Literal":
                case "MetaProperty":
                case "MethodDefinition":
                case "Module":
                case "Node":
                case "NodeExtensions":
                case "NodeList":
                case "Nodes":
                case "ObjectPattern":
                case "Property":
                case "PropertyKind":
                case "Range":
                case "RegexValue":
                case "RestElement":
                case "Script":
                case "SpreadElement":
                case "Super":
                case "SwitchCase":
                case "TemplateElement":
                case "TemplateLiteral":
                case "VariableDeclarationKind":
                case "VariableDeclarator":
                    break;
            }
        }
    }
}
