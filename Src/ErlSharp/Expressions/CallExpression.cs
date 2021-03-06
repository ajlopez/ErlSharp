﻿namespace ErlSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ErlSharp.Language;

    public class CallExpression : IExpression
    {
        private IExpression nameexpression;
        private IList<IExpression> argumentexpressions;

        public CallExpression(IExpression nameexpression, IList<IExpression> argumentexpressions)
        {
            this.nameexpression = nameexpression;
            this.argumentexpressions = argumentexpressions;
        }

        public IExpression NameExpression { get { return this.nameexpression; } }

        public IList<IExpression> ArgumentExpressions { get { return this.argumentexpressions; } }

        public object Evaluate(Context context, bool withvars = false)
        {
            object namevalue = this.nameexpression.Evaluate(context, withvars);

            IList<object> arguments = new List<object>();

            foreach (var argexpr in this.argumentexpressions)
                arguments.Add(Machine.ExpandDelayedCall(argexpr.Evaluate(context, withvars)));

            if (namevalue is Atom)
                namevalue = context.GetValue(string.Format("{0}/{1}", ((Atom)namevalue).Name, this.argumentexpressions.Count));

            IFunction func = (IFunction)namevalue;

            return func.Apply(context, arguments);
        }

        public bool HasVariable()
        {
            if (this.nameexpression.HasVariable())
                return true;

            foreach (var expr in this.argumentexpressions)
                if (expr.HasVariable())
                    return true;

            return false;
        }

        public DelayedCallExpression ToDelayedCallExpression()
        {
            return new DelayedCallExpression(this.nameexpression, this.argumentexpressions);
        }
    }
}
