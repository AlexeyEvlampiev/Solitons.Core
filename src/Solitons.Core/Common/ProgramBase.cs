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
/// Serves as the foundational abstract class for application programs, providing key infrastructure for argument processing and execution control.
/// </summary>
public abstract class ProgramBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramBase"/> class using the default argument manipulation callback.
    /// </summary>
    /// <param name="args">The collection of command-line arguments passed to the program.</param>
    [DebuggerStepThrough]
    protected ProgramBase(string[] args)
        : this(args, new DefaultCallback())
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramBase"/> class using a custom argument manipulation callback.
    /// </summary>
    /// <param name="args">The collection of command-line arguments passed to the program.</param>
    /// <param name="callback">A custom callback responsible for the manipulation and processing of the program arguments.</param>
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
    /// Executes the program's logic asynchronously.
    /// </summary>
    /// <param name="cancellation">An optional <see cref="CancellationToken"/> to enable task cancellation.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result encapsulates the program's exit code.</returns>
    public abstract Task<int> RunAsync(CancellationToken cancellation = default);


    /// <summary>
    /// Executes a factory method to instantiate a derived class and initiates its asynchronous execution.
    /// </summary>
    /// <param name="factory">A factory method to instantiate a derived <see cref="ProgramBase"/> class.</param>
    /// <param name="onError">An optional delegate for custom exception handling. Defaults to console-based error output.</param>
    /// <param name="cancellation">An optional <see cref="CancellationToken"/> to enable task cancellation.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the outcome of the asynchronous operation. A return code of '1' indicates an exception occurred.</returns>
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
    /// Gets the UTC date and time at which this instance of the program was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Gets the command line string, formulated based on the processed arguments.
    /// </summary>
    public string CommandLineText { get; }

    /// <summary>
    /// Gets an immutable array of processed command-line arguments.
    /// </summary>
    public ImmutableArray<string> Arguments { get; }

    /// <summary>
    /// Defines the contract for a callback interface, responsible for the manipulation and processing of the program arguments.
    /// </summary>
    protected interface IConstructorCallback
    {
        /// <summary>
        /// Processes the initial collection of command-line arguments.
        /// </summary>
        /// <param name="args">The original collection of command-line arguments.</param>
        /// <returns>An array of processed command-line arguments.</returns>
        string[] ProcessArguments(string[] args);

        /// <summary>
        /// Gets the current UTC date and time.
        /// </summary>
        DateTimeOffset UtcNow { get; }

        /// <summary>
        /// Constructs a command line string based on the processed arguments.
        /// </summary>
        /// <param name="args">An enumerable collection of processed command-line arguments.</param>
        /// <returns>A command line string.</returns>
        string BuildCommandLineText(IEnumerable<string> args);
    }

    /// <summary>
    /// Represents the default implementation of the <see cref="IConstructorCallback"/> interface.
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
            var text = args
                .Select(Environment.ExpandEnvironmentVariables)
                .Join(" ");
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