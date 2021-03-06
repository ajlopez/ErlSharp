﻿namespace ErlSharp.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ErlSharp.Compiler;
    using ErlSharp.Expressions;
    using ErlSharp.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MatchBodyTests
    {
        [TestMethod]
        public void MatchAtoms()
        {
            MatchBody match = new MatchBody(new Atom("a"), new ConstantExpression(1));

            var context = match.MakeContext(new Atom("a"), null);

            Assert.IsNotNull(context);

            var result = match.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void DontMatchDifferentAtoms()
        {
            MatchBody match = new MatchBody(new Atom("a"), new ConstantExpression(1));

            var context = match.MakeContext(new Atom("b"), null);

            Assert.IsNull(context);
        }

        [TestMethod]
        public void MatchVariableInteger()
        {
            MatchBody match = new MatchBody(new Variable("X"), new VariableExpression(new Variable("X")));

            var context = match.MakeContext(123, null);

            Assert.IsNotNull(context);

            var result = match.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.AreEqual(123, result);
        }
    }
}
