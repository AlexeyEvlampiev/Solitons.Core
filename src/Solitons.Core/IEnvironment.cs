using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;

namespace Solitons;

/// <summary>
/// Defines a contract for interacting with environment-specific information and operations,
/// serving as an abstraction over the system's native Environment static class.
/// </summary>
/// <remarks>
/// The IEnvironment interface is designed to provide a decoupled and testable approach to
/// access environment variables, system properties, and perform environment-specific operations.
/// Implementations of this interface can override default system behavior, thereby allowing
/// for greater flexibility and testability in applications.
///
/// This interface is particularly useful for:
/// - Unit testing components that rely on environment-specific settings.
/// - Extending or modifying the behavior of environment-related operations.
/// - Facilitating dependency injection by providing a swappable implementation.
///
/// <example>
/// The following example demonstrates how the IEnvironment interface can be used to check
/// the operating system platform before executing platform-dependent code:
/// <code>
/// <![CDATA[
/// // Defines a Program class that utilizes an IEnvironment interface
/// // to get information about the environment it's running on.
/// public sealed class Program
/// {
///     // A read-only field to hold an instance of IEnvironment.
///     private readonly IEnvironment _env;
///
///     // A constructor that accepts an IEnvironment implementation.
///     // This constructor allows dependency injection of custom IEnvironment
///     // implementations, which can be useful for unit testing.
///     internal Program(IEnvironment env)
///     {
///         _env = env ?? throw new ArgumentNullException(nameof(env));
///     }
///
///     // A default constructor that passes the system's environment
///     // information to the internal constructor.
///     // It utilizes a default implementation of IEnvironment.
///     public Program() : this(IEnvironment.System) { }
///
///     // The Run method checks if the current operating system platform is
///     // supported, and if not, throws a NotSupportedException.
///     public void Run()
///     {
///         // Define an array of supported platforms.
///         var supportedPlatforms = new[] { PlatformID.MacOSX, PlatformID.Unix };
///
///         // Check if the current platform is in the list of supported platforms.
///         if (!supportedPlatforms.Contains(_env.OSVersion.Platform))
///         {
///             // Throw an exception if the current platform is not supported.
///             throw new NotSupportedException($"{_env.OSVersion} is not supported");
///         }
///         
///         // The rest of the code goes here.
///     }
/// }
/// ]]>
/// </code>
/// </example>
/// </remarks>
/// <seealso cref="Environment"/>
public partial interface IEnvironment
{
    /// <summary>
    /// Default (System) implementation
    /// </summary>
    sealed class SystemEnvironment : IEnvironment
    {
        /// <summary>
        /// Default (System) implementation
        /// </summary>
        public static SystemEnvironment Instance = new();

        private SystemEnvironment()
        {
        }
    }

    /// <summary>
    /// Gets the singleton instance of the default system environment implementation.
    /// </summary>
    /// <value>
    /// The singleton instance of the <see cref="SystemEnvironment"/> class, which serves as the default implementation of the <see cref="IEnvironment"/> interface.
    /// </value>
    /// <remarks>
    /// This property provides a convenient way to access environment-specific information and operations using the default system implementation.
    /// It is marked with the <see cref="DebuggerNonUserCodeAttribute"/> to indicate that it is not a user code and should be skipped while debugging.
    /// </remarks>
    /// <example>
    /// Here is how you can use the System property to get an environment variable:
    /// <code>
    /// string path = IEnvironment.System.GetEnvironmentVariable("PATH");
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static IEnvironment System => SystemEnvironment.Instance;



    /// <summary>
    /// Gets the context line for this process.
    /// </summary>
    /// <returns> A string containing context-line arguments. </returns>
    public string CommandLine => Environment.CommandLine;


    /// <summary>
    /// Gets or sets the fully qualified path of the current working directory.
    /// </summary>
    /// <returns>The directory path.</returns>
    /// <exception cref="ArgumentNullException">Attempted to set to an empty string ("").</exception>
    /// <exception cref="ArgumentNullException">An I/O error occurred.</exception>
    /// <exception cref="IOException">An I/O error occurred.</exception>
    /// <exception cref="DirectoryNotFoundException">Attempted to set a local path that cannot be found.</exception>
    /// <exception cref="SecurityException">The caller does not have the appropriate permission.</exception>
    public string CurrentDirectory
    {
        get => Environment.CurrentDirectory;
        set => Environment.CurrentDirectory = value;
    }


    /// <summary>
    /// Retrieves a unique identifier associated with the currently executing managed thread within the application domain.
    /// </summary>
    /// <value>
    /// An integer representing the unique identifier for the current managed thread. This identifier is intended for informational purposes and may be subject to change between different execution contexts.
    /// </value>
    /// <remarks>
    /// Utilizing this property can be instrumental in debugging, logging, and profiling scenarios where tracking the execution flow across various threads is required.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the unique identifier for the current managed thread:
    /// <code>
    /// int uniqueThreadId = IEnvironment.System.CurrentManagedThreadId;
    /// </code>
    /// </example>
    public int CurrentManagedThreadId => Environment.CurrentManagedThreadId;

    /// <summary>
    /// Gets or sets the exit code for the current process, providing a mechanism for conveying termination status to the operating system.
    /// </summary>
    /// <value>
    /// A 32-bit signed integer representing the exit code of the process. The default value is 0, signifying successful completion of the process.
    /// </value>
    /// <remarks>
    /// The exit code is a conventional means by which a process communicates its termination status to the operating system. A zero value typically indicates successful completion, while a non-zero value usually signifies an error or exceptional condition. This property is instrumental in scripting and automation scenarios where the exit status of a process is used for conditional logic.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to set and retrieve the exit code for the current process:
    /// <code>
    /// IEnvironment env = IEnvironment.System;
    /// env.ExitCode = 1;  // Set an error exit code
    /// int currentExitCode = env.ExitCode;  // Retrieve the current exit code
    /// </code>
    /// </example>
    public int ExitCode
    {
        get => Environment.ExitCode;
        set => Environment.ExitCode = value;
    }

    /// <summary>
    /// Retrieves a boolean value indicating whether the current application domain is in the process of being unloaded or if the Common Language Runtime (CLR) is in the process of shutting down.
    /// </summary>
    /// <value>
    /// Returns true if the current application domain is being unloaded or the CLR is undergoing termination; otherwise, returns false.
    /// </value>
    /// <remarks>
    /// This property serves as an important indicator for resource cleanup and graceful termination procedures. It is particularly useful in scenarios where it is critical to know the state of the application domain or the CLR, such as in finalizers or application shutdown hooks.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to check if the application domain or CLR is shutting down:
    /// <code>
    /// bool isShuttingDown = IEnvironment.System.HasShutdownStarted;
    /// if (isShuttingDown)
    /// {
    ///     // Perform cleanup or logging activities
    /// }
    /// </code>
    /// </example>
    public bool HasShutdownStarted => Environment.HasShutdownStarted;

    /// <summary>
    /// Retrieves a boolean value indicating whether the operating system under which the application is running is a 64-bit operating system.
    /// </summary>
    /// <value>
    /// Returns true if the underlying operating system is 64-bit; otherwise, returns false.
    /// </value>
    /// <remarks>
    /// This property is essential for making runtime decisions based on the architecture of the operating system. It can be particularly useful for determining the compatibility of certain libraries, making API calls, or optimizing performance for 64-bit systems.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to check if the operating system is 64-bit:
    /// <code>
    /// bool is64BitOS = IEnvironment.System.Is64BitOperatingSystem;
    /// if (is64BitOS)
    /// {
    ///     // Perform operations specific to 64-bit systems
    /// }
    /// </code>
    /// </example>
    public bool Is64BitOperatingSystem => Environment.Is64BitOperatingSystem;

    /// <summary>
    /// Retrieves a boolean value indicating whether the current process is executing under a 64-bit process architecture.
    /// </summary>
    /// <value>
    /// Returns true if the current process is a 64-bit process; otherwise, returns false.
    /// </value>
    /// <remarks>
    /// This property is crucial for making architectural-specific decisions at runtime. It is particularly relevant for scenarios involving dynamic loading of assemblies, inter-process communication, or when leveraging native libraries that are architecture-dependent.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to determine if the current process is 64-bit:
    /// <code>
    /// bool is64BitProcess = IEnvironment.System.Is64BitProcess;
    /// if (is64BitProcess)
    /// {
    ///     // Execute logic specific to 64-bit processes
    /// }
    /// </code>
    /// </example>
    public bool Is64BitProcess => Environment.Is64BitProcess;


    /// <summary>
    /// Retrieves the NetBIOS name of the local computer on which the application is running.
    /// </summary>
    /// <value>
    /// A string representing the NetBIOS name of the local computer.
    /// </value>
    /// <remarks>
    /// This property is useful for identifying the local computer in networked environments. It can be employed in scenarios such as logging, auditing, or for generating machine-specific configurations.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the NetBIOS name of the local computer:
    /// <code>
    /// string localMachineName = IEnvironment.System.MachineName;
    /// </code>
    /// </example>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the NetBIOS name of the local computer cannot be obtained.
    /// </exception>
    public string MachineName => Environment.MachineName;

    /// <summary>
    /// Retrieves the newline string defined for the current environment, serving as the standard delimiter between lines for this environment.
    /// </summary>
    /// <value>
    /// A string representing the newline delimiter. The value is "\r\n" for non-Unix platforms and "\n" for Unix platforms.
    /// </value>
    /// <remarks>
    /// This property is essential for text manipulation and formatting tasks that are platform-agnostic. It ensures that the application behaves consistently across different operating systems when dealing with text-based resources.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to use the newline string to concatenate lines of text:
    /// <code>
    /// string text = "Line 1" + IEnvironment.System.NewLine + "Line 2";
    /// </code>
    /// </example>
    public string NewLine => Environment.NewLine;

    /// <summary>
    /// Retrieves the identifier and version number of the operating system under which the application is running.
    /// </summary>
    /// <value>
    /// An <see cref="OperatingSystem"/> object that encapsulates the platform identifier and version number.
    /// </value>
    /// <remarks>
    /// This property is invaluable for making runtime decisions based on the operating system's characteristics. It can be particularly useful for ensuring compatibility, optimizing performance, or enabling platform-specific features.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the operating system version:
    /// <code>
    /// OperatingSystem osVersion = IEnvironment.System.OSVersion;
    /// </code>
    /// </example>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the system version cannot be obtained, or the obtained platform identifier is not a member of <see cref="PlatformID"/>.
    /// </exception>
    public OperatingSystem OSVersion => Environment.OSVersion;

    /// <summary>
    /// Retrieves the fully qualified path of the executable that initiated the currently executing process.
    /// </summary>
    /// <value>
    /// A nullable string representing the path of the executable. Returns null if the path is not available.
    /// </value>
    /// <remarks>
    /// This property is useful for scenarios where it is necessary to know the origin of the executing process, such as in logging, auditing, or for security validations.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the path of the executable that started the current process:
    /// <code>
    /// string? executablePath = IEnvironment.System.ProcessPath;
    /// if (executablePath != null)
    /// {
    ///     // Perform operations using the executable path
    /// }
    /// </code>
    /// </example>
    public string? ProcessPath => Environment.ProcessPath;

    /// <summary>
    /// Retrieves the unique identifier (PID) assigned to the currently executing process.
    /// </summary>
    /// <value>
    /// A 32-bit integer representing the unique identifier for the current process.
    /// </value>
    /// <remarks>
    /// This property is crucial for distinguishing between different instances of the same or different processes running on the system. It can be used for logging, monitoring, or inter-process communication (IPC) scenarios.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the unique identifier for the current process:
    /// <code>
    /// int processId = IEnvironment.System.ProcessId;
    /// </code>
    /// </example>
    public int ProcessId => Environment.ProcessId;

    /// <summary>
    /// Retrieves the number of logical processors available to the currently executing process.
    /// </summary>
    /// <value>
    /// A 32-bit signed integer specifying the number of available logical processors.
    /// </value>
    /// <remarks>
    /// This property is essential for optimizing parallel computing tasks and load distribution. It provides valuable information that can be used to tune the performance of multi-threaded or parallel applications.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the number of available processors:
    /// <code>
    /// int availableProcessors = IEnvironment.System.ProcessorCount;
    /// </code>
    /// </example>
    public int ProcessorCount => Environment.ProcessorCount;

    /// <summary>
    /// Retrieves the current stack trace information for the executing thread.
    /// </summary>
    /// <value>
    /// A string containing the stack trace information. The value can be <see cref="string.Empty"/> if the stack trace is unavailable.
    /// </value>
    /// <remarks>
    /// This property is invaluable for debugging and logging purposes, as it provides a snapshot of the call stack for the current thread. It can be used to trace the sequence of method calls that led to the current execution point.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the current stack trace information:
    /// <code>
    /// string currentStackTrace = IEnvironment.System.StackTrace;
    /// </code>
    /// </example>
    public string StackTrace => Environment.StackTrace;

    /// <summary>
    /// Retrieves the fully qualified path of the system directory.
    /// </summary>
    /// <value>
    /// A string representing the fully qualified path to the system directory.
    /// </value>
    /// <remarks>
    /// This property is essential for locating system-level resources or executing system-related operations. It provides a reliable way to access the system directory irrespective of the underlying operating system's configuration.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the fully qualified path of the system directory:
    /// <code>
    /// string systemDirectoryPath = IEnvironment.System.SystemDirectory;
    /// </code>
    /// </example>
    public string SystemDirectory => Environment.SystemDirectory;

    /// <summary>
    /// Retrieves the size, in bytes, of a memory page in the current operating system.
    /// </summary>
    /// <value>
    /// An integer representing the number of bytes in a single memory page.
    /// </value>
    /// <remarks>
    /// This property is crucial for understanding the memory architecture of the operating system. It can be used for optimizing memory-intensive operations and for aligning memory in low-level programming scenarios.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the size of a system memory page:
    /// <code>
    /// int pageSize = IEnvironment.System.SystemPageSize;
    /// </code>
    /// </example>
    public int SystemPageSize => Environment.SystemPageSize;

    /// <summary>
    /// Retrieves the number of milliseconds that have elapsed since the operating system was started.
    /// </summary>
    /// <value>
    /// A 32-bit signed integer representing the elapsed time in milliseconds since the operating system was initialized.
    /// </value>
    /// <remarks>
    /// This property is essential for performance measurement, timing operations, and calculating time intervals. It provides a high-resolution time measurement that is consistent across all hardware and operating systems.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the elapsed time since the system was started:
    /// <code>
    /// int elapsedMilliseconds = IEnvironment.System.TickCount;
    /// </code>
    /// </example>
    public int TickCount => Environment.TickCount;

    /// <summary>
    /// Retrieves the number of milliseconds that have elapsed since the operating system was started.
    /// </summary>
    /// <value>
    /// A 32-bit signed integer representing the elapsed time in milliseconds since the operating system was initialized.
    /// </value>
    /// <remarks>
    /// This property is essential for performance measurement, timing operations, and calculating time intervals. It provides a high-resolution time measurement that is consistent across all hardware and operating systems.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the elapsed time since the system was started:
    /// <code>
    /// int elapsedMilliseconds = IEnvironment.System.TickCount;
    /// </code>
    /// </example>
    public long TickCount64 => Environment.TickCount64;

    /// <summary>
    /// Retrieves the number of milliseconds that have elapsed since the operating system was initialized.
    /// </summary>
    /// <value>
    /// A 64-bit signed integer representing the elapsed time in milliseconds since the operating system was initialized.
    /// </value>
    /// <remarks>
    /// This property is crucial for performance measurement, timing operations, and calculating long time intervals. Unlike the 32-bit <see cref="TickCount"/>, this property provides a 64-bit value, offering a much larger range and eliminating the need for handling overflow.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the elapsed time since the system was started using a 64-bit integer:
    /// <code>
    /// long elapsedMilliseconds = IEnvironment.System.TickCount64;
    /// </code>
    /// </example>
    public string UserDomainName => Environment.UserDomainName;

    /// <summary>
    /// Retrieves the network domain name associated with the currently authenticated user.
    /// </summary>
    /// <value>
    /// A string representing the network domain name of the current user.
    /// </value>
    /// <remarks>
    /// This property is essential for identifying the network context in which the current user is operating. It can be used for security, auditing, and multi-domain scenarios.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the network domain name of the current user:
    /// <code>
    /// string domainName = IEnvironment.System.UserDomainName;
    /// </code>
    /// </example>
    /// <exception cref="PlatformNotSupportedException">
    /// Thrown when the operating system does not support the retrieval of network domain names.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the network domain name cannot be retrieved for some reason.
    /// </exception>
    public bool UserInteractive => Environment.UserInteractive;

    /// <summary>
    /// Retrieves the username associated with the currently executing thread.
    /// </summary>
    /// <value>
    /// A string representing the username of the individual associated with the current thread.
    /// </value>
    /// <remarks>
    /// This property is instrumental for identifying the user context under which the current thread is executing. It can be utilized for auditing, logging, or for implementing user-specific functionalities.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the username associated with the current thread:
    /// <code>
    /// string currentUserName = IEnvironment.System.UserName;
    /// </code>
    /// </example>
    public string UserName => Environment.UserName;

    /// <summary>
    /// Retrieves the version of the Common Language Runtime (CLR) currently in use by the application.
    /// </summary>
    /// <value>
    /// A <see cref="Version"/> object representing the version of the CLR.
    /// </value>
    /// <remarks>
    /// This property is essential for understanding the runtime environment of the application. It can be used for compatibility checks, feature toggling based on runtime version, or logging for diagnostic purposes.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the CLR version:
    /// <code>
    /// Version clrVersion = IEnvironment.System.Version;
    /// </code>
    /// </example>
    public Version Version => Environment.Version;

    /// <summary>
    /// Retrieves the amount of physical memory that is mapped to the process context.
    /// </summary>
    /// <value>
    /// A 64-bit signed integer representing the number of bytes of physical memory mapped to the process context.
    /// </value>
    /// <remarks>
    /// This property provides valuable insights into the memory footprint of the application. It is particularly useful for performance monitoring, diagnostics, and resource management.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to obtain the amount of physical memory mapped to the process context:
    /// <code>
    /// long workingSetSize = IEnvironment.System.WorkingSet;
    /// </code>
    /// </example>
    public long WorkingSet => Environment.WorkingSet;

    /// <summary>
    /// Terminates the current process and returns a specified exit code to the operating system.
    /// </summary>
    /// <param name="exitCode">The exit code to be returned to the operating system. A value of 0 (zero) indicates successful completion.</param>
    /// <remarks>
    /// This method provides a controlled mechanism for terminating the application. It is particularly useful for signaling the outcome of operations to external processes or scripts that may be invoking the application.
    /// </remarks>
    /// <exception cref="SecurityException">Thrown when the caller lacks sufficient security permissions to terminate the process.</exception>
    /// <example>
    /// The following code snippet demonstrates how to terminate the process with a specific exit code:
    /// <code>
    /// IEnvironment.System.Exit(1);
    /// </code>
    /// </example>
    public void Exit(int exitCode) => Environment.Exit(exitCode);

    /// <summary>
    /// Replaces each embedded environment variable, denoted by the percent sign (%), in the specified string with its corresponding value and returns the resulting string.
    /// </summary>
    /// <param name="name">A string containing zero or more environment variables enclosed by percent signs (%).</param>
    /// <returns>A string where each embedded environment variable is replaced by its corresponding value.</returns>
    /// <remarks>
    /// This method is useful for resolving environment variables in configuration strings or file paths. It provides a way to dynamically inject environment-specific values into strings.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when the 'name' parameter is null.</exception>
    /// <example>
    /// The following code snippet demonstrates how to expand environment variables in a string:
    /// <code>
    /// string result = IEnvironment.System.ExpandEnvironmentVariables("Path is %PATH%");
    /// </code>
    /// </example>
    public string ExpandEnvironmentVariables(string name) => Environment.ExpandEnvironmentVariables(name);

    /// <summary>
    /// Immediately terminates the current process, writes a specified message to the Windows Application event log, and includes the message in error reporting to Microsoft.
    /// </summary>
    /// <param name="message">An optional message that provides an explanation for the abrupt termination of the process. This message will be written to the Windows Application event log and included in error reporting to Microsoft.</param>
    /// <remarks>
    /// This method is typically used in situations where the state of a process has become corrupt and there is no safe way to continue execution. Invoking this method will terminate the process immediately without executing any finalizers or calling any shutdown hooks.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to use the FailFast method to terminate a process immediately:
    /// <code>
    /// IEnvironment.System.FailFast("Critical error encountered. Terminating process.");
    /// </code>
    /// </example>
    public void FailFast(string? message) => Environment.FailFast(message);

    /// <summary>
    /// Immediately terminates the current process, writes a specified message to the Windows Application event log, and includes both the message and exception information in error reporting to Microsoft.
    /// </summary>
    /// <param name="message">An optional message that provides an explanation for the abrupt termination of the process. This message will be written to the Windows Application event log and included in error reporting to Microsoft.</param>
    /// <param name="exception">An optional exception that represents the error causing the termination. This exception information will be included in error reporting to Microsoft.</param>
    /// <remarks>
    /// This method is typically used in situations where the state of a process has become corrupt and there is no safe way to continue execution. Invoking this method will terminate the process immediately without executing any finalizers or calling any shutdown hooks.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to use the FailFast method to terminate a process immediately with an exception:
    /// <code>
    /// try
    /// {
    ///     // Code that may throw an exception
    /// }
    /// catch (Exception ex)
    /// {
    ///     IEnvironment.System.FailFast("Critical error encountered. Terminating process.", ex);
    /// }
    /// </code>
    /// </example>
    public void FailFast(string? message, Exception? exception) => Environment.FailFast(message, exception);

    /// <summary>
    /// Retrieves an array of strings representing the command-line arguments passed to the current process.
    /// </summary>
    /// <returns>
    /// An array of strings, where the first element is the executable file name, and the subsequent elements contain the command-line arguments.
    /// </returns>
    /// <remarks>
    /// This method provides a programmatic way to access the command-line arguments that were passed to the current process upon its invocation. The first element of the returned array is the name of the executable, followed by the arguments in the order they were provided.
    /// </remarks>
    /// <exception cref="NotSupportedException">
    /// Thrown when the system does not support retrieval of command-line arguments.
    /// </exception>
    /// <example>
    /// The following code snippet demonstrates how to use the GetCommandLineArgs method to retrieve command-line arguments:
    /// <code>
    /// string[] args = IEnvironment.System.GetCommandLineArgs();
    /// foreach (string arg in args)
    /// {
    ///     Console.WriteLine(arg);
    /// }
    /// </code>
    /// </example>
    public string[] GetCommandLineArgs() => Environment.GetCommandLineArgs();




    /// <summary>
    /// Retrieves the value of an environment variable from the current process.
    /// </summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <returns>The value of the environment variable specified by variable, or null if the environment variable is not found.</returns>
    /// <exception cref="ArgumentNullException">variable is null</exception>
    /// <exception cref="SecurityException">The caller does not have the required permission to perform this operation.</exception>
    public string? GetEnvironmentVariable(string variable) => Environment.GetEnvironmentVariable(variable);


    /// <summary>
    /// Retrieves the value of an environment variable from the current process or from
    /// the Windows operating system registry key for the current user or local machine.
    /// </summary>
    /// <param name="variable">The name of an environment variable.</param>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/>
    /// is supported on .NET Core running on Unix-bases systems.</param>
    /// <returns>The value of the environment variable specified by the variable and target parameters, 
    /// or null if the environment variable is not found.
    /// </returns>
    /// <exception cref="ArgumentNullException">variable is null.</exception>
    /// <exception cref="ArgumentException">target is not a valid <see cref="EnvironmentVariableTarget"/> value.</exception>
    /// <exception cref="SecurityException">The caller does not have the required permission to perform this operation.</exception>
    public string? GetEnvironmentVariable(string variable, EnvironmentVariableTarget target) => Environment.GetEnvironmentVariable(variable, target);



    /// <summary>
    /// Retrieves a dictionary containing the names and values of all environment variables for the current process.
    /// </summary>
    /// <returns>
    /// A dictionary containing key-value pairs representing the names and values of all environment variables. Returns an empty dictionary if no environment variables are found.
    /// </returns>
    /// <remarks>
    /// This method provides a programmatic way to access the environment variables of the current process. Each key-value pair in the returned dictionary represents the name and value of an environment variable.
    /// </remarks>
    /// <exception cref="SecurityException">
    /// Thrown when the caller lacks the necessary permissions to perform this operation.
    /// </exception>
    /// <exception cref="OutOfMemoryException">
    /// Thrown when the system is unable to allocate sufficient memory for the operation.
    /// </exception>
    /// <example>
    /// The following code snippet demonstrates how to use the GetEnvironmentVariables method to retrieve environment variables:
    /// <code>
    /// IDictionary envVars = IEnvironment.System.GetEnvironmentVariables();
    /// foreach (DictionaryEntry entry in envVars)
    /// {
    ///     Console.WriteLine($"{entry.Key} = {entry.Value}");
    /// }
    /// </code>
    /// </example>
    public IDictionary GetEnvironmentVariables() => Environment.GetEnvironmentVariables();

    /// <summary>
    /// Retrieves a dictionary containing the names and values of all environment variables from the specified source.
    /// </summary>
    /// <param name="target">
    /// Specifies the source from which to retrieve the environment variables. Accepts one of the <see cref="EnvironmentVariableTarget"/> values.
    /// </param>
    /// <returns>
    /// A dictionary containing key-value pairs representing the names and values of all environment variables from the specified source. Returns an empty dictionary if no environment variables are found.
    /// </returns>
    /// <remarks>
    /// This method provides a programmatic way to access environment variables from different sources, such as the current process, current user, or local machine. The source is specified by the <paramref name="target"/> parameter.
    /// </remarks>
    /// <exception cref="SecurityException">
    /// Thrown when the caller lacks the necessary permissions to perform this operation for the specified <paramref name="target"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the <paramref name="target"/> parameter contains an illegal value.
    /// </exception>
    /// <example>
    /// The following code snippet demonstrates how to use the GetEnvironmentVariables method to retrieve environment variables from the current process:
    /// <code>
    /// IDictionary envVars = IEnvironment.System.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
    /// foreach (DictionaryEntry entry in envVars)
    /// {
    ///     Console.WriteLine($"{entry.Key} = {entry.Value}");
    /// }
    /// </code>
    /// </example>
    public IDictionary GetEnvironmentVariables(EnvironmentVariableTarget target) => Environment.GetEnvironmentVariables(target);

    /// <summary>
    /// Retrieves the file path of a system-defined special folder, as specified by the <see cref="Environment.SpecialFolder"/> enumeration.
    /// </summary>
    /// <param name="folder">
    /// An enumeration value that identifies a system special folder.
    /// </param>
    /// <returns>
    /// The file path of the specified system special folder. Returns an empty string if the folder does not physically exist on the system.
    /// </returns>
    /// <remarks>
    /// This method provides a way to access special folders, such as the Program Files, My Documents, or System folders, in a platform-agnostic manner. The folder's existence is determined by the operating system, and it may not physically exist if it was not created or was subsequently deleted.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Thrown when the <paramref name="folder"/> parameter is not a valid member of the <see cref="Environment.SpecialFolder"/> enumeration.
    /// </exception>
    /// <exception cref="PlatformNotSupportedException">
    /// Thrown when the current platform does not support this operation.
    /// </exception>
    /// <example>
    /// The following code snippet demonstrates how to use the GetFolderPath method to retrieve the path to the My Documents folder:
    /// <code>
    /// string myDocumentsPath = IEnvironment.System.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    /// Console.WriteLine($"My Documents Path: {myDocumentsPath}");
    /// </code>
    /// </example>
    public string GetFolderPath(Environment.SpecialFolder folder) => Environment.GetFolderPath(folder);

    /// <summary>
    /// Retrieves the file path of a system-defined special folder, as specified by the <see cref="Environment.SpecialFolder"/> enumeration, and applies the specified access options.
    /// </summary>
    /// <param name="folder">
    /// An enumeration value that identifies a system special folder.
    /// </param>
    /// <param name="option">
    /// An enumeration value that specifies the access options for the special folder.
    /// </param>
    /// <returns>
    /// The file path of the specified system special folder. Returns an empty string if the folder does not physically exist on the system.
    /// </returns>
    /// <remarks>
    /// This method provides a way to access special folders, such as the Program Files, My Documents, or System folders, in a platform-agnostic manner. The folder's existence is determined by the operating system, and it may not physically exist if it was not created or was subsequently deleted.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Thrown when the <paramref name="folder"/> parameter is not a valid member of the <see cref="Environment.SpecialFolder"/> enumeration, or when the <paramref name="option"/> parameter is not a valid member of the <see cref="Environment.SpecialFolderOption"/> enumeration.
    /// </exception>
    /// <exception cref="PlatformNotSupportedException">
    /// Thrown when the current platform does not support this operation.
    /// </exception>
    /// <example>
    /// The following code snippet demonstrates how to use the GetFolderPath method to retrieve the path to the My Documents folder with the 'Create' option:
    /// <code>
    /// string myDocumentsPath = IEnvironment.System.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create);
    /// Console.WriteLine($"My Documents Path: {myDocumentsPath}");
    /// </code>
    /// </example>
    public string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option) => Environment.GetFolderPath(folder, option);

    /// <summary>
    /// Retrieves an array of strings representing the names of all logical drives on the current computer system.
    /// </summary>
    /// <returns>
    /// An array of strings, where each element represents the name of a logical drive on the system. For instance, the first element could be "C:\" if the computer's primary hard drive is the first logical drive.
    /// </returns>
    /// <remarks>
    /// This method provides a programmatic way to access the logical drives present on the system, which may include hard disk drives, removable drives, and network drives.
    /// </remarks>
    /// <exception cref="IOException">
    /// Thrown when an Input/Output error occurs during the operation.
    /// </exception>
    /// <exception cref="SecurityException">
    /// Thrown when the caller lacks the necessary permissions to perform this operation.
    /// </exception>
    /// <example>
    /// The following code snippet demonstrates how to use the GetLogicalDrives method to list all logical drives on the system:
    /// <code>
    /// string[] drives = IEnvironment.System.GetLogicalDrives();
    /// foreach (string drive in drives)
    /// {
    ///     Console.WriteLine($"Drive: {drive}");
    /// }
    /// </code>
    /// </example>
    public string[] GetLogicalDrives() => Environment.GetLogicalDrives();

    /// <summary>
    /// Modifies, creates, or deletes an environment variable within the context of the current process.
    /// </summary>
    /// <param name="variable">
    /// The name of the environment variable to be modified, created, or deleted.
    /// </param>
    /// <param name="value">
    /// The value to be assigned to the specified environment variable. A null value will result in the deletion of the variable.
    /// </param>
    /// <remarks>
    /// This method provides a way to programmatically manage environment variables for the current process. It allows for the modification, creation, or deletion of environment variables, which can be useful for configuring runtime behavior.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the 'variable' parameter is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the 'variable' parameter contains a zero-length string, an initial hexadecimal zero character (0x00), or an equal sign ("="). Also thrown when the length of 'variable' or 'value' is greater than or equal to 32,767 characters, or if an error occurs during the operation.
    /// </exception>
    /// <exception cref="SecurityException">
    /// Thrown when the caller lacks the necessary permissions to perform this operation.
    /// </exception>
    /// <example>
    /// The following code snippet demonstrates how to use the SetEnvironmentVariable method to set an environment variable for the current process:
    /// <code>
    /// IEnvironment.System.SetEnvironmentVariable("MyVariable", "MyValue");
    /// </code>
    /// </example>
    public void SetEnvironmentVariable(string variable, string? value) => Environment.SetEnvironmentVariable(variable, value);

    /// <summary>
    /// Modifies, creates, or deletes an environment variable within the context of the current process or within the Windows operating system registry key designated for the current user or local machine.
    /// </summary>
    /// <param name="variable">
    /// The name of the environment variable to be modified, created, or deleted.
    /// </param>
    /// <param name="value">
    /// The value to be assigned to the specified environment variable. A null value will result in the deletion of the variable.
    /// </param>
    /// <param name="target">
    /// An enumeration value that specifies the location where the environment variable should be stored.
    /// </param>
    /// <remarks>
    /// This method provides a way to programmatically manage environment variables for the current process or within the Windows operating system registry. It allows for the modification, creation, or deletion of environment variables, which can be useful for configuring runtime behavior.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the 'variable' parameter is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the 'variable' parameter contains a zero-length string, an initial hexadecimal zero character (0x00), or an equal sign ("="). Also thrown when the length of 'variable' or 'value' exceeds the specified limits, or if 'target' is not a valid enumeration value, or if an error occurs during the operation.
    /// </exception>
    /// <exception cref="SecurityException">
    /// Thrown when the caller lacks the necessary permissions to perform this operation.
    /// </exception>
    /// <example>
    /// The following code snippet demonstrates how to use the SetEnvironmentVariable method to set an environment variable for the current process:
    /// <code>
    /// IEnvironment.System.SetEnvironmentVariable("MyVariable", "MyValue", EnvironmentVariableTarget.Process);
    /// </code>
    /// </example>
    public void SetEnvironmentVariable(string variable, string? value, EnvironmentVariableTarget target) =>
        Environment.SetEnvironmentVariable(variable, value, target);

}

public partial interface IEnvironment
{
    /// <summary>
    /// Loads and parses command-line arguments from a specified file.
    /// </summary>
    /// <param name="file">The path to the file containing the command-line arguments to be parsed.</param>
    /// <returns>An array of strings representing the parsed command-line arguments.</returns>
    [DebuggerStepThrough]
    public string[] LoadCommandArgs(string file) => ParseCommandArgs(File.ReadAllText(file));

    /// <summary>
    /// Parses a command line string into an array of arguments, while also expanding any embedded environment variables.
    /// </summary>
    /// <param name="commandLine">
    /// The command line string to be parsed.
    /// </param>
    /// <returns>
    /// An array of strings representing the parsed arguments. Environment variables enclosed in '%' characters are expanded.
    /// </returns>
    /// <remarks>
    /// This method provides a robust way to parse command line arguments and expand environment variables within those arguments. It supports arguments enclosed in double quotes, allowing for arguments that contain spaces.
    /// </remarks>
    /// <example>
    /// The following code snippet demonstrates how to use the ParseCommandArgs method to parse a command line string:
    /// <code>
    /// string[] args = IEnvironment.System.ParseCommandArgs("arg1 \"arg2 with spaces\" %USERNAME%");
    /// </code>
    /// </example>
    public string[] ParseCommandArgs(string commandLine)
    {
        commandLine = ExpandEnvironmentVariables(commandLine);
        return Regex
            .Matches(commandLine, "\"(?<arg>[^\"]*)\"|(?<arg>\\S+)")
            .Select(m => m.Groups["arg"].Value)
            .ToArray();
    }
}