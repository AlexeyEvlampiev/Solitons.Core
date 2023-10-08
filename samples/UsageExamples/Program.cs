using Solitons.Common;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Reflection;
using Solitons;
using Solitons.Text;

namespace UsageExamples;

public sealed class Program : ProgramBase
{
    private readonly Option<int> _examplesCount = new Option<int>("--take", () => 1);
    private readonly Argument<string> _exampleSelector = new(() => String.Empty);
    private readonly RootCommand _rootCommand = new RootCommand();

    [DebuggerStepThrough]
    public static Task<int> Main(string[] args) => new Program(args).RunAsync();


    public Program(string[] args) : base(args)
    {
        
        _rootCommand.Description = "Solitons  Usage Examples";
        

        _exampleSelector.Arity = ArgumentArity.ZeroOrOne;
        _examplesCount.AddAlias("--do");
        _examplesCount.AddAlias("--limit");
        _examplesCount.AddAlias("--peek");

        _rootCommand.AddOption(_examplesCount);
        _rootCommand.AddArgument(_exampleSelector);
        _rootCommand.SetHandler(RunAsync);
    }



    public override Task<int> RunAsync(CancellationToken cancellation = default) => _rootCommand
        .InvokeAsync(Arguments.ToArray());



    private async Task RunAsync(InvocationContext context)
    {
        var selector = context.ParseResult.GetValueForArgument(_exampleSelector);
        var count = context.ParseResult.GetValueForOption(_examplesCount);
        var trigram = new TrigramStringComparer();
        var types = GetType().Assembly
            .GetTypes()
            .Where(type => Attribute.IsDefined(type, typeof(ExampleAttribute)))
            .OrderBy(type => TrigramStringComparer.CalculateSimilarity(selector, type.FullName ?? String.Empty))
            .Take(count)
            .ToList();

        foreach (var type in types)
        {
            var methods = type.GetMethods(
                BindingFlags.Instance |
                BindingFlags.Public | 
                BindingFlags.Static | 
                BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                object? result = default;
                if (method.IsStatic)
                {
                    result = method.Invoke(null, Array.Empty<object>());
                }
                else
                {
                    var example = ThrowIf.NullReference(Activator.CreateInstance(type));
                    result = method.Invoke(example, Array.Empty<object>());
                }

                if (result is Task task)
                {
                    await task;
                }
            }
        }
    }
}