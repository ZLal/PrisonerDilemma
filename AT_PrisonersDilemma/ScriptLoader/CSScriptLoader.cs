using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace AT_PrisonersDilemma.ScriptLoader
{
    public class CSScriptLoader
    {
        private Assembly? assembly = null;
        public bool IsLoaded { get => (assembly is not null); }

        public void LoadAssembly(string source)
        {
            var tree = SyntaxFactory.ParseSyntaxTree(source);
            string fileName = Path.GetRandomFileName() + ".dll";
            // Detect the file location for the library that defines the object type
            var systemRefLocation = typeof(object).GetTypeInfo().Assembly.Location;
            // Create a reference to the library
            var systemReference = MetadataReference.CreateFromFile(systemRefLocation);
            // A single, immutable invocation to the compiler to produce a library
            var compilation = CSharpCompilation.Create(fileName)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(systemReference)
                .AddSyntaxTrees(tree);
            string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            EmitResult compilationResult = compilation.Emit(path);
            if (compilationResult.Success)
            {
                // Load the assembly
                assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            }
            else
            {
                string issues = string.Empty;
                foreach (Diagnostic codeIssue in compilationResult.Diagnostics)
                {
                    issues = @$"ID: {codeIssue.Id},
                        Message: {codeIssue.GetMessage()},
                        Location: {codeIssue.Location.GetLineSpan()},
                        Severity: {codeIssue.Severity}
                        \r\n";
                }
                throw new Exception(issues);
            }
        }

        public object? ExecuteMethod(string name, object?[]? parameters)
        {
            string namespaceName, className, methodName;
            string[] splits = name.Split('.');
            if (splits.Length < 3) throw new Exception("Name is expected in format \"Namespace.Class.Method\"");
            namespaceName = string.Join('.', splits[0..^2]);
            className = splits[^2];
            methodName = splits[^1];
            if (!IsLoaded) throw new Exception("Assembly is not loaded");
            Type type = assembly!.GetType($"{namespaceName}.{className}") ?? throw new Exception("Failed to load class");
            MethodInfo method = type.GetMethod(methodName) ?? throw new Exception("Failed to load method");
            return method.Invoke(null, parameters);
        }

        public void Test()
        {
            const string code = @"
                using System;
                using System.IO;
                namespace RoslynCore
                {
                    public static class Helper
                    {
                        public static double CalculateCircleArea(double radius)
                        {
                            return radius * radius * Math.PI;
                        }
                    }
                }
            ";
            LoadAssembly(code);
            double radius = 10;
            object? result = ExecuteMethod("RoslynCore.Helper.CalculateCircleArea", new object[] { radius });
            Console.WriteLine($"Circle area with radius = {radius} is {result}");
        }
    }
}
