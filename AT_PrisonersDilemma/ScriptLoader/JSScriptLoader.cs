using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace AT_PrisonersDilemma.ScriptLoader
{
    public class JSScriptLoader
    {
        private Context? context = null;
        public bool IsLoaded { get => (context is not null); }

        public void LoadAssembly(string source)
        {
            context = new Context();
            context.Eval(source);
        }
        public object? ExecuteMethod(string name, object?[]? parameters)
        {
            if (!IsLoaded) throw new Exception("Assembly is not loaded");
            var functionDel = context!.GetVariable(name).As<Function>();
            return functionDel.Call(new Arguments() { parameters ?? System.Array.Empty<object>() });
        }
        public void Test()
        {
            LoadAssembly("var findSum = (a, b) => a + b;");
            var findSum = context!.GetVariable("findSum").As<Function>();
            Console.WriteLine("Result is " + findSum.Call(new Arguments { "100", "500" }));
        }
    }
}
