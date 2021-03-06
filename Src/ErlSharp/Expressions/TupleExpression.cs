﻿namespace ErlSharp.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ErlSharp.Language;

    public class TupleExpression : IExpression
    {
        private IList<IExpression> expressions;

        public TupleExpression(IEnumerable<IExpression> expressions)
        {
            this.expressions = new System.Collections.Generic.List<IExpression>(expressions);
        }

        public IList<IExpression> Expressions { get { return this.expressions; } }

        public object Evaluate(Context context, bool withvars = false)
        {
            IList<object> elements = new List<object>();

            foreach (var expr in this.expressions)
                elements.Add(expr.Evaluate(context, withvars));

            return new Tuple(elements);
        }

        public bool HasVariable()
        {
            foreach (var expr in this.expressions)
                if (expr.HasVariable())
                    return true;

            return false;
        }
    }
}
