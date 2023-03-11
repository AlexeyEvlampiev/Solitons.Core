using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IEnvironment
    {
        /// <summary>
        /// Gets the context line for this process.
        /// </summary>
        /// <returns> A string containing context-line arguments. </returns>
        string CommandLine { get; }


        /// <summary>
        /// Gets or sets the fully qualified path of the current working directory.
        /// </summary>
        /// <returns>The directory path.</returns>
        /// <exception cref="ArgumentNullException">Attempted to set to an empty string ("").</exception>
        /// <exception cref="ArgumentNullException">An I/O error occurred.</exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="DirectoryNotFoundException">Attempted to set a local path that cannot be found.</exception>
        /// <exception cref="SecurityException">The caller does not have the appropriate permission.</exception>
        string CurrentDirectory { get; set; }

        IEnvironment With(Action<EnvironmentClientConfig> options);

        /*
        //
        // Summary:
        //     Gets a unique identifier for the current managed thread.
        //
        // Returns:
        //     A unique identifier for this managed thread.
        public static int CurrentManagedThreadId
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets or sets the exit code of the process.
        //
        // Returns:
        //     A 32-bit signed integer containing the exit code. The default value is 0 (zero),
        //     which indicates that the process completed successfully.
        public static int ExitCode
        {
            get
            {
                throw null;
            }
            set
            {
            }
        }

        //
        // Summary:
        //     Gets a value that indicates whether the current application domain is being unloaded
        //     or the common language runtime (CLR) is shutting down.
        //
        // Returns:
        //     true if the current application domain is being unloaded or the CLR is shutting
        //     down; otherwise, false.
        public static bool HasShutdownStarted
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets a value that indicates whether the current operating system is a 64-bit
        //     operating system.
        //
        // Returns:
        //     true if the operating system is 64-bit; otherwise, false.
        public static bool Is64BitOperatingSystem
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets a value that indicates whether the current process is a 64-bit process.
        //
        // Returns:
        //     true if the process is 64-bit; otherwise, false.
        public static bool Is64BitProcess
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets the NetBIOS name of this local computer.
        //
        // Returns:
        //     The name of this computer.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The name of this computer cannot be obtained.
        public static string MachineName
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets the newline string defined for this environment.
        //
        // Returns:
        //     \r\n for non-Unix platforms, or \n for Unix platforms.
        public static string NewLine
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets the current platform identifier and version number.
        //
        // Returns:
        //     The platform identifier and version number.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     This property was unable to obtain the system version. -or- The obtained platform
        //     identifier is not a member of System.PlatformID
        public static OperatingSystem OSVersion
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Returns the path of the executable that started the currently executing process.
        //     Returns null when the path is not available.
        //
        // Returns:
        //     The path of the executable that started the currently executing process.
        public static string? ProcessPath
        {
            [NullableContext(2)]
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets the unique identifier for the current process.
        //
        // Returns:
        //     A number that represents the unique identifier for the current process.
        public static int ProcessId
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets the number of processors available to the current process.
        //
        // Returns:
        //     The 32-bit signed integer that specifies the number of processors that are available.
        public static int ProcessorCount
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets current stack trace information.
        //
        // Returns:
        //     A string containing stack trace information. This value can be System.String.Empty.
        public static string StackTrace
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets the fully qualified path of the system directory.
        //
        // Returns:
        //     A string containing a directory path.
        public static string SystemDirectory
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets the number of bytes in the operating system's memory page.
        //
        // Returns:
        //     The number of bytes in the system memory page.
        public static int SystemPageSize
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets the number of milliseconds elapsed since the system started.
        //
        // Returns:
        //     A 32-bit signed integer containing the amount of time in milliseconds that has
        //     passed since the last time the computer was started.
        public static int TickCount
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets the number of milliseconds elapsed since the system started.
        //
        // Returns:
        //     The elapsed milliseconds since the system started.
        public static long TickCount64
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets the network domain name associated with the current user.
        //
        // Returns:
        //     The network domain name associated with the current user.
        //
        // Exceptions:
        //   T:System.PlatformNotSupportedException:
        //     The operating system does not support retrieving the network domain name.
        //
        //   T:System.InvalidOperationException:
        //     The network domain name cannot be retrieved.
        public static string UserDomainName
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets a value indicating whether the current process is running in user interactive
        //     mode.
        //
        // Returns:
        //     true if the current process is running in user interactive mode; otherwise, false.
        public static bool UserInteractive
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets the user name of the person who is associated with the current thread.
        //
        // Returns:
        //     The user name of the person who is associated with the current thread.
        public static string UserName
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets a version consisting of the major, minor, build, and revision numbers of
        //     the common language runtime.
        //
        // Returns:
        //     The version of the common language runtime.
        public static Version Version
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Gets the amount of physical memory mapped to the process context.
        //
        // Returns:
        //     A 64-bit signed integer containing the number of bytes of physical memory mapped
        //     to the process context.
        public static long WorkingSet
        {
            get
            {
                throw null;
            }
        }

        //
        // Summary:
        //     Terminates this process and returns an exit code to the operating system.
        //
        // Parameters:
        //   exitCode:
        //     The exit code to return to the operating system. Use 0 (zero) to indicate that
        //     the process completed successfully.
        //
        // Exceptions:
        //   T:System.Security.SecurityException:
        //     The caller does not have sufficient security permission to perform this function.
        [DoesNotReturn]
        public static void Exit(int exitCode)
        {
            throw null;
        }

        //
        // Summary:
        //     Replaces the name of each environment variable embedded in the specified string
        //     with the string equivalent of the value of the variable, then returns the resulting
        //     string.
        //
        // Parameters:
        //   name:
        //     A string containing the names of zero or more environment variables. Each environment
        //     variable is quoted with the percent sign character (%).
        //
        // Returns:
        //     A string with each environment variable replaced by its value.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     name is null.
        public static string ExpandEnvironmentVariables(string name)
        {
            throw null;
        }

        //
        // Summary:
        //     Immediately terminates a process after writing a message to the Windows Application
        //     event log, and then includes the message in error reporting to Microsoft.
        //
        // Parameters:
        //   message:
        //     A message that explains why the process was terminated, or null if no explanation
        //     is provided.
        [DoesNotReturn]
        public static void FailFast(string? message)
        {
            throw null;
        }

        //
        // Summary:
        //     Immediately terminates a process after writing a message to the Windows Application
        //     event log, and then includes the message and exception information in error reporting
        //     to Microsoft.
        //
        // Parameters:
        //   message:
        //     A message that explains why the process was terminated, or null if no explanation
        //     is provided.
        //
        //   exception:
        //     An exception that represents the error that caused the termination. This is typically
        //     the exception in a catch block.
        [DoesNotReturn]
        public static void FailFast(string? message, Exception? exception)
        {
            throw null;
        }

        //
        // Summary:
        //     Returns a string array containing the context-line arguments for the current
        //     process.
        //
        // Returns:
        //     An array of strings where each element contains a context-line argument. The
        //     first element is the executable file name, and the following zero or more elements
        //     contain the remaining context-line arguments.
        //
        // Exceptions:
        //   T:System.NotSupportedException:
        //     The system does not support context-line arguments.
        public static string[] GetCommandLineArgs()
        {
            throw null;
        }

        */


        /// <summary>
        /// Retrieves the value of an environment variable from the current process.
        /// </summary>
        /// <param name="variable">The name of the environment variable.</param>
        /// <returns>The value of the environment variable specified by variable, or null if the environment variable is not found.</returns>
        /// <exception cref="ArgumentNullException">variable is null</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission to perform this operation.</exception>
        string? GetEnvironmentVariable(string variable);


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
        string? GetEnvironmentVariable(string variable, EnvironmentVariableTarget target);

        /*

//
// Summary:
//     Retrieves all environment variable names and their values from the current process.
//
// Returns:
//     A dictionary that contains all environment variable names and their values; otherwise,
//     an empty dictionary if no environment variables are found.
//
// Exceptions:
//   T:System.Security.SecurityException:
//     The caller does not have the required permission to perform this operation.
//
//   T:System.OutOfMemoryException:
//     The buffer is out of memory.
public static IDictionary GetEnvironmentVariables()
{
    throw null;
}

//
// Summary:
//     Retrieves all environment variable names and their values from the current process,
//     or from the Windows operating system registry key for the current user or local
//     machine.
//
// Parameters:
//   target:
//     One of the System.EnvironmentVariableTarget values. Only System.EnvironmentVariableTarget.Process
//     is supported on .NET Core running on Unix-based systems.
//
// Returns:
//     A dictionary that contains all environment variable names and their values from
//     the source specified by the target parameter; otherwise, an empty dictionary
//     if no environment variables are found.
//
// Exceptions:
//   T:System.Security.SecurityException:
//     The caller does not have the required permission to perform this operation for
//     the specified value of target.
//
//   T:System.ArgumentException:
//     target contains an illegal value.
public static IDictionary GetEnvironmentVariables(EnvironmentVariableTarget target)
{
    throw null;
}

//
// Summary:
//     Gets the path to the system special folder that is identified by the specified
//     enumeration.
//
// Parameters:
//   folder:
//     One of enumeration values that identifies a system special folder.
//
// Returns:
//     The path to the specified system special folder, if that folder physically exists
//     on your computer; otherwise, an empty string (""). A folder will not physically
//     exist if the operating system did not create it, the existing folder was deleted,
//     or the folder is a virtual directory, such as My Computer, which does not correspond
//     to a physical path.
//
// Exceptions:
//   T:System.ArgumentException:
//     folder is not a member of System.Environment.SpecialFolder.
//
//   T:System.PlatformNotSupportedException:
//     The current platform is not supported.
public static string GetFolderPath(SpecialFolder folder)
{
    throw null;
}

//
// Summary:
//     Gets the path to the system special folder that is identified by the specified
//     enumeration, and uses a specified option for accessing special folders.
//
// Parameters:
//   folder:
//     One of the enumeration values that identifies a system special folder.
//
//   option:
//     One of the enumeration values that specifies options to use for accessing a special
//     folder.
//
// Returns:
//     The path to the specified system special folder, if that folder physically exists
//     on your computer; otherwise, an empty string (""). A folder will not physically
//     exist if the operating system did not create it, the existing folder was deleted,
//     or the folder is a virtual directory, such as My Computer, which does not correspond
//     to a physical path.
//
// Exceptions:
//   T:System.ArgumentException:
//     folder is not a member of System.Environment.SpecialFolder. -or- options is not
//     a member of System.Environment.SpecialFolderOption.
//
//   T:System.PlatformNotSupportedException:
//     The current platform is not supported.
public static string GetFolderPath(SpecialFolder folder, SpecialFolderOption option)
{
    throw null;
}

//
// Summary:
//     Returns an array of string containing the names of the logical drives on the
//     current computer.
//
// Returns:
//     An array of strings where each element contains the name of a logical drive.
//     For example, if the computer's hard drive is the first logical drive, the first
//     element returned is "C:\".
//
// Exceptions:
//   T:System.IO.IOException:
//     An I/O error occurs.
//
//   T:System.Security.SecurityException:
//     The caller does not have the required permissions.
public static string[] GetLogicalDrives()
{
    throw null;
}

//
// Summary:
//     Creates, modifies, or deletes an environment variable stored in the current process.
//
// Parameters:
//   variable:
//     The name of an environment variable.
//
//   value:
//     A value to assign to variable.
//
// Exceptions:
//   T:System.ArgumentNullException:
//     variable is null.
//
//   T:System.ArgumentException:
//     variable contains a zero-length string, an initial hexadecimal zero character
//     (0x00), or an equal sign ("="). -or- The length of variable or value is greater
//     than or equal to 32,767 characters. -or- An error occurred during the execution
//     of this operation.
//
//   T:System.Security.SecurityException:
//     The caller does not have the required permission to perform this operation.
public static void SetEnvironmentVariable(string variable, string? value)
{
}

//
// Summary:
//     Creates, modifies, or deletes an environment variable stored in the current process
//     or in the Windows operating system registry key reserved for the current user
//     or local machine.
//
// Parameters:
//   variable:
//     The name of an environment variable.
//
//   value:
//     A value to assign to variable.
//
//   target:
//     One of the enumeration values that specifies the location of the environment
//     variable.
//
// Exceptions:
//   T:System.ArgumentNullException:
//     variable is null.
//
//   T:System.ArgumentException:
//     variable contains a zero-length string, an initial hexadecimal zero character
//     (0x00), or an equal sign ("="). -or- The length of variable is greater than or
//     equal to 32,767 characters. -or- target is not a member of the System.EnvironmentVariableTarget
//     enumeration. -or- target is System.EnvironmentVariableTarget.Machine or System.EnvironmentVariableTarget.User,
//     and the length of variable is greater than or equal to 255. -or- target is System.EnvironmentVariableTarget.Process
//     and the length of value is greater than or equal to 32,767 characters. -or- An
//     error occurred during the execution of this operation.
//
//   T:System.Security.SecurityException:
//     The caller does not have the required permission to perform this operation.
public static void SetEnvironmentVariable(string variable, string? value, EnvironmentVariableTarget target)
{
}

*/


    }

    public partial interface IEnvironment
    {
        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        public static IEnvironment System => SysEnvironment.Instance;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string[] LoadCommandArgs(string file) => ParseCommandArgs(File.ReadAllText(file));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        public static string[] ParseCommandArgs(string commandLine)
        {
            return Regex
                .Matches(commandLine, "\"(?<arg>[^\"]*)\"|%(?<env>[\\w_]+)%|(?<arg>\\S+)")
                .Select(m =>
                {
                    if (m.Groups["arg"].Success)
                        return m.Groups["arg"].Value;
                    var key = m.Groups["env"].Value;
                    return Environment.GetEnvironmentVariable(key) ?? string.Empty;
                })
                .ToArray();
        }
    }
    

}
