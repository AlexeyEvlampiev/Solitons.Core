using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Common;

/// <summary>
/// Represents a base class for programs.
/// </summary>
public abstract class ProgramBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramBase"/> class using the default callback.
    /// </summary>
    /// <param name="args">The program arguments.</param>
    [DebuggerStepThrough]
    protected ProgramBase(string[] args)
        : this(args, new DefaultCallback())
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramBase"/> class.
    /// </summary>
    /// <param name="args">The program arguments.</param>
    /// <param name="callback">The callback for processing and manipulating the program arguments.</param>
    protected ProgramBase(string[] args, IConstructorCallback callback)
    {
        Arguments = args
            .EmptyIfNull()
            .Convert(callback.ProcessArguments)
            .ToImmutableArray();
        CreatedAt = callback.UtcNow;
        CommandLineText = callback.BuildCommandLineText(Arguments);
    }

    /// <summary>
    /// Runs the program asynchronously.
    /// </summary>
    /// <param name="cancellation">The optional cancellation token to allow cancellation of the program execution.</param>
    /// <returns>A task representing the asynchronous operation. The task result represents the program's exit code.</returns>
    public abstract Task<int> RunAsync(CancellationToken cancellation = default);

   
    protected static async Task<int> RunAsync(
        Func<ProgramBase> factory, 
        Action<Exception>? onError = null,
        CancellationToken cancellation = default)
    {
        onError ??= [DebuggerNonUserCode](exception) => ConsoleColor.Red.AsForegroundColor(() => 
            Console.WriteLine(exception.Message));
        try
        {
            var program = factory.Invoke();
            return await program.RunAsync(cancellation);
        }
        catch (Exception e)
        {
            onError.Invoke(e);
            return 1;
        }
    }


    /// <summary>
    /// Gets the creation date and time of the program.
    /// </summary>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Gets the command line text constructed from the program arguments.
    /// </summary>
    public string CommandLineText { get; }

    /// <summary>
    /// Gets the program arguments.
    /// </summary>
    public ImmutableArray<string> Arguments { get; }

    /// <summary>
    /// Represents the callback interface for processing and manipulating the program arguments.
    /// </summary>
    protected interface IConstructorCallback
    {
        /// <summary>
        /// Processes the program arguments.
        /// </summary>
        /// <param name="args">The program arguments.</param>
        /// <returns>The processed program arguments.</returns>
        string[] ProcessArguments(string[] args);

        /// <summary>
        /// Gets the current UTC date and time.
        /// </summary>
        DateTimeOffset UtcNow { get; }

        /// <summary>
        /// Builds the command line text from the program arguments.
        /// </summary>
        /// <param name="args">The program arguments.</param>
        /// <returns>The command line text.</returns>
        string BuildCommandLineText(IEnumerable<string> args);
    }

    /// <summary>
    /// Represents the default implementation of the callback interface.
    /// </summary>
    protected class DefaultCallback : IConstructorCallback
    {
        /// <inheritdoc/>
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

        /// <inheritdoc/>
        public virtual string[] ProcessArguments(string[] args) => args;

        /// <inheritdoc/>
        public virtual string BuildCommandLineText(IEnumerable<string> args)
        {
            var text = args.Join(" ");
            var regex = new Regex(@"(?i)%(?<var>[a-z_][a-z0-9_]{0,254})%");
            var errors = new HashSet<string>(StringComparer.Ordinal);
            foreach (Match match in regex.Matches(text))
            {
                var varName = match.Groups["var"].Value;
                errors.Add($"'{varName}' environment variable could not be found.");
            }

            if (errors.Count > 0)
            {
                throw new ArgumentException(errors.Join("; "));
            }
            return text;
        }

    }

}