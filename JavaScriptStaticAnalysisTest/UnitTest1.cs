using System;
using Esprima.Ast;
using JavaScriptStaticAnalysis;
using JavaScriptStaticAnalysis.IR;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JavaScriptStaticAnalysisTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Context ctx = Context.CreateInstance(@"
for (i = 0; i < 10; i++)
    a += i;");
            IRBuilder bb = new IRBuilder(ctx.Script);
        }
    }
}
