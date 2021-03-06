﻿namespace ErlSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using ErlSharp.Compiler;
    using ErlSharp.Functions;
    using ErlSharp.Language;
    using ErlSharp.Modules;
    using System.Globalization;

    public class Machine
    {
        private Context rootcontext;

        public Machine()
        {
            this.rootcontext = new Context();
            this.rootcontext.SetValue("c/1", new CompileModuleFunction(this));
            this.rootcontext.SetValue("spawn/1", new SpawnFunction());
            this.rootcontext.SetValue("self/0", new SelfFunction());

            this.TextWriter = System.Console.Out;

            Module lists = new ListsModule(this.rootcontext);
            this.rootcontext.SetValue(lists.Name, lists);

            Module io = new IoModule(this);
            this.rootcontext.SetValue(io.Name, io);
        }

        public Context RootContext { get { return this.rootcontext; } }

        public TextWriter TextWriter { get; set; }

        public static object ExpandDelayedCall(object value)
        {
            while (value is DelayedCall)
                value = ((DelayedCall)value).Evaluate();

            return value;
        }

        public static string ToString(object value)
        {
            if (value == null)
                return "null";
            if (value.Equals(false))
                return "false";
            if (value.Equals(true))
                return "true";

            if (value is float)
                return ((float)value).ToString(CultureInfo.InvariantCulture);
            if (value is double)
                return ((double)value).ToString(CultureInfo.InvariantCulture);

            return value.ToString();
        }

        public static bool AreEqual(object obj1, object obj2)
        {
            if (obj1 == null)
                return obj2 == null;

            return obj1.Equals(obj2);
        }

        public static int GetHashCode(object obj)
        {
            if (obj == null)
                return 0;

            return obj.GetHashCode();
        }

        public Module LoadModule(string modname)
        {
            Module module = new Module(this.rootcontext);
            StreamReader reader = File.OpenText(modname + ".erl");
            Parser parser = new Parser(reader);

            for (var form = parser.ParseForm(); form != null; form = parser.ParseForm())
                form.Evaluate(module.Context);

            reader.Close();

            return module;
        }
    }
}
