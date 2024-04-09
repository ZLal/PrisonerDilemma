using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace AT_PrisonersDilemma.ScriptLoader
{
    public class PYScriptLoader
    {
        private readonly ScriptEngine engine = Python.CreateEngine();
        private ScriptScope? scope = null;
        public bool IsLoaded { get => (scope is not null); }

        public void LoadAssembly(string source)
        {
            scope = engine.CreateScope();
            engine.Execute(source, scope);
        }
        public object? ExecuteMethod(string name, object?[]? parameters)
        {
            if (!IsLoaded) throw new Exception("Assembly is not loaded");
            object function = scope!.GetVariable(name);
            ObjectOperations operations = engine.CreateOperations(scope);
            return operations.Invoke(function, parameters ?? Array.Empty<object>());
        }

        public void Test()
        {
            string source = @"
def findSum(a, b):
    res = a + b
    return res
            ";
            LoadAssembly(source);

            var result = ExecuteMethod("findSum", new object[] { 100, 500 });
            Console.WriteLine("Result is " + result);
        }
    }
}
