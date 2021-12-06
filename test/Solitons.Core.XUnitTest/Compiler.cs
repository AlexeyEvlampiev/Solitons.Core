using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json.Serialization;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Solitons
{
    class Compiler
    {
        public static Assembly Compile(string assemblyName, string code)
        {
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp9);
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(DomainContext).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(XmlAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(JsonPropertyNameAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
            };

            Assembly
                .GetEntryAssembly()
                .GetReferencedAssemblies()
                .ForEach(a => references.Add(MetadataReference.CreateFromFile(Assembly.Load(a).Location)));

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(code, options);
            var compilation = CSharpCompilation.Create(assemblyName,
                new[] { parsedSyntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                    assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));
            using var memory = new MemoryStream();
            var result = compilation.Emit(memory);
            Assert.True(result.Success);
            memory.Seek(0, SeekOrigin.Begin);

            var assemblyLoadContext = new SimpleUnloadableAssemblyLoadContext();
            var assembly = assemblyLoadContext.LoadFromStream(memory);
            return assembly;
        }

        internal class SimpleUnloadableAssemblyLoadContext : AssemblyLoadContext
        {
            public SimpleUnloadableAssemblyLoadContext()
                : base(true)
            {
            }

            protected override Assembly Load(AssemblyName assemblyName)
            {
                return null;
            }
        }
    }
}
