﻿namespace ErlSharp.Tests.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ErlSharp.Expressions;
    using ErlSharp.Forms;
    using ErlSharp.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FunctionFormTests
    {
        [TestMethod]
        public void DefineAndEvaluateFunction()
        {
            FunctionForm form = new FunctionForm("add", new IExpression[] { new VariableExpression(new Variable("X")), new VariableExpression(new Variable("Y")) }, new AddExpression(new VariableExpression(new Variable("X")), new VariableExpression(new Variable("Y"))));
            Context context = new Context();

            var result = form.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Function));

            var func = (Function)result;

            Assert.AreSame(func, context.GetValue("add/2"));

            var newcontext = func.MakeContext(new object[] { 1, 2 });

            Assert.IsNotNull(newcontext);
            Assert.AreEqual(1, newcontext.GetValue("X"));
            Assert.AreEqual(2, newcontext.GetValue("Y"));

            Assert.AreSame(func, newcontext.GetValue("add/2"));

            Assert.AreEqual(3, func.Evaluate(newcontext));
        }
    }
}
