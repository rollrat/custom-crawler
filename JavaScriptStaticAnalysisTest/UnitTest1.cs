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
            Context ctx = Context.CreateInstance(@"a=0;");
            IRBuilder bb = new IRBuilder(ctx.Script);
        }
    }
}
