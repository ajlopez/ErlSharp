namespace ErlSharp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ErlSharp.Language;

    public class SelfFunction : IFunction
    {
        public object Apply(Context context, IList<object> arguments)
        {
            return Process.Current;
        }
    }
}
