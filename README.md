# Solitons.Core
Solitons.Core is a .NET class library designed for cloud software systems. It provides interfaces, algorithms, and utility functions for efficient development, including data processing and visualization. The library's comprehensive features help developers save time and resources, while delivering high-quality solutions for modern cloud computing.
## Configuration
The Configuration namespace, provides a programming model for handling configuration data in .NET applications. The namespace contains types that enable developers to manage application configuration settings and connection strings, making it easier to configure and manage different aspects of an application. The namespace includes classes for loading and parsing configuration data, validation of configuration data, and support for different configuration sources.
### SettingsGroup class
In software development, managing configuration data is an essential task. Configuration data can include settings such as database connection details, API keys, feature flags, and other runtime parameters. The challenge with configuration data is that it needs to be easily configurable, and often, different configuration options need to be combined and stored as a single unit.

One common approach to manage configuration data is to use key-value pairs stored in a configuration file or database. However, using a key-value store alone can be cumbersome, especially when dealing with large or complex configuration data. It can be difficult to keep track of all the configuration options, their types, and their dependencies. Additionally, configuration data can contain sensitive information, and it needs to be stored securely.

Another common approach is to use connection string like formatted settings to represent sets of related configuration options in a structured way. This approach is widely used in scenarios where configuration data is required as a secret value for DevOps pipelines or stored in secure storage such as Azure KeyVault. However, manually parsing and managing the configuration data can still be error-prone, time-consuming, and require a significant amount of boilerplate code.

The SettingsGroup abstract class provides a flexible way to define groups of settings as properties of a class, which can be serialized and deserialized in a plain-text semicolon-separated format. This approach is similar to using connection string like formatted settings to represent configuration data, but with the added benefit of being able to manage configuration data in a structured way.

Developers can extend the SettingsGroup class to create their own custom settings groups, allowing them to define their own settings and manage them in a consistent and structured way. By using SettingsGroup, developers can reduce the amount of boilerplate code required to manage settings and ensure that their code adheres to a consistent and well-defined structure.

The SettingsGroup class provides several methods to manage and parse configuration data. The ToString() method converts the properties into a string format suitable for serialization, and the Parse<T>() method deserializes the plain-text format into a concrete settings group object of type T. Custom parsing can be implemented by overriding the PreProcess() and SetProperty() methods.

The class also provides methods for iterating over the settings as key-value pairs, checking for required properties, and comparing objects for equality. The class is designed to be flexible and can be used to represent different kinds of settings, but the connection string format is a common use case.

Using the SettingsGroup approach can simplify the management of configuration data, reduce errors and inconsistencies, and make it easier to maintain and update the configuration data. By having a well-defined structure, developers can also ensure that the code is more robust, reliable, and secure.



#### Example 1: Using custom settings group
The following example demonstrates how to use the SettingsGroup class to manage a specific type of settings group: a user login settings group. The UserLoginSettings class inherits from SettingsGroup and defines two settings: User and Password.
```csharp
public static class UsingSettingsGroupExample
{
    // The Run() method is a demonstration of how to use the UserLoginSettings class.
    public static void Run()
    {
        // Create a new UserLoginSettings object and set its properties.
        var login = new UserLoginSettings()
        {
            User = "admin",
            Password = "bb307dafcbe7"
        };

        // Serialize the login object to a plain-text semicolon-separated format using the ToString() method and display the output.
        Console.WriteLine(login);
        // Output: user=admin;password=bb307dafcbe7

        // Deserialize a plain-text semicolon-separated string that contains different User and Password values.
        login = UserLoginSettings.Parse("u=superuser;pwd=c82584fa160f");
        // Serialize the deserialized login object to a plain-text semicolon-separated format using the ToString() method and display the output.
        Console.WriteLine(login);
        // Output: user=superuser;password=c82584fa160f
    }

    // The UserLoginSettings class defines a specific type of settings group that inherits from SettingsGroup.
    public sealed class UserLoginSettings : SettingsGroup
    {
        // The User property is a required setting with a specific pattern to match its name.
        [Setting("user", IsRequired = true, Pattern = "(?i)(username|use?r|u)")]
        public string User { get; set; } = String.Empty;

        // The Password property is a required setting with a specific pattern to match its name.
        [Setting("password", IsRequired = true, Pattern = "(?i)(password|pass|pwd|p)")]
        public string Password { get; set; } = String.Empty;

        // The Parse method uses the Parse<T> method of the SettingsGroup class to deserialize a plain-text semicolon-separated string to a UserLoginSettings object.
        public static UserLoginSettings Parse(string text) => Parse<UserLoginSettings>(text);
    }
}
```
The User and Password properties are decorated with the Setting attribute, which defines the name of the setting, a regular expression pattern to match the setting, and whether the setting is required. In this case, both settings are required and have different regular expression patterns that match their names.

In the Run() method, the UserLoginSettings class is instantiated with a User and Password value, and then serialized to a plain-text semicolon-separated format using the ToString() method. The output is displayed in the console.

Then, the UserLoginSettings.Parse() method is used to deserialize a plain-text semicolon-separated string that contains a different User and Password value. The output is again displayed in the console.

The example illustrates how the SettingsGroup class can be used to manage a specific type of settings group, and how the settings can be serialized and deserialized in a standard connection string like formatted string. By using the SettingsGroup class, developers can define and manage related configuration options in a structured way, reducing the amount of boilerplate code required to manage and parse these settings.
  
    
## Diagnostics
Solitons Diagnostics namespace provides types and interfaces for application logging and tracing purposes.
### IAsyncLogger interface
The IAsyncLogger interface is an immutable and efficient way to capture and store critical events asynchronously, which allows for continuous enrichment of the execution context down the call chain. The immutability of the logger ensures that all events are recorded accurately and consistently, even as the context of the call chain changes over time. This means that each event is tagged with a complete set of information about the call chain at the time the event was generated, enabling developers to track the execution path of their application and identify potential issues more easily. Additionally, the use of asynchronous logging enables non-blocking I/O operations, which can greatly improve the performance and responsiveness of the application. Overall, the IAsyncLogger interface is a powerful and reliable logging solution for developers who require accurate and consistent event capture and efficient performance.
#### Example 1: Using default IAsyncLogger implementations
The following is an example of using the default implementation of the IAsyncLogger interface to capture and store events asynchronously. In this example, a console logger is created with default tags and properties, such as the name of the machine and the name of the user. The logger is then used to record various events using different log levels (i.e., Info).

```csharp
public static class UsingDefaultConsoleLoggerExample
{
    public static async Task RunAsync()
    {
        // Create a console logger with default tags and properties
        var logger = ConsoleAsyncLogger
            .Create()
            .WithTags("example", "console")
            .WithProperty("machine", Environment.MachineName)
            .WithProperty("user", Environment.UserName);

        await logger.InfoAsync($"Starting...");
        // Output: {"level":"Info","message":"Starting...","created":"2022-02-21T18:39:56.1717421+00:00","details":null,"tags":["example","console"],"properties":{"machine":"developer-pc","user":"developer"}}

        await ExecuteWithLoggingAsync(logger);
        // Output: {"level":"Info","message":"Some message","created":"2022-02-21T18:39:56.4674155+00:00","details":"Thread ID: 1","tags":["example","console"],"properties":{"machine":"developer-pc","user":"developer","method":"ExecuteWithLoggingAsync"}}

        await logger.InfoAsync("Done...");
        // Output: {"level":"Info","message":"Done...","created":"2022-02-21T18:39:56.4679397+00:00","details":null,"tags":["example","console"],"properties":{"machine":"developer-pc","user":"developer"}}
    }

    private static async Task ExecuteWithLoggingAsync(IAsyncLogger logger)
    {
        // Extend logger configurations
        logger = logger
            .WithProperty("thread", Thread.CurrentThread.ManagedThreadId.ToString());
        await logger.InfoAsync("Message goes here.");
    }
}
```

In the ExecuteWithLoggingAsync method, the logger is further configured to include the ID of the current thread. This showcases the ability to add and modify properties of the logger in a dynamic and flexible manner.

Throughout the example, the logger outputs the events in a standardized JSON format, which includes details such as the log level, message, time of creation, tags, properties, and additional details. This ensures consistency and accuracy of the logged events, while also providing developers with useful information for troubleshooting and debugging their application.
#### Example 2: Custom IAsyncLogger implementation
The following is an example of how developers can create and utilize a custom implementation of the IAsyncLogger interface. In this example, a custom logger is created by extending the AsyncLogger class with a new implementation, named CustomAsyncLogger.

```csharp
public static class UsingCustomLoggerExample
{
    public static async Task RunAsync()
    {
        var logger = CustomAsyncLogger
            .Create()
            .WithTags("example", "custom implementation")
            .WithProperty("machine", Environment.MachineName)
            .WithProperty("user", Environment.UserName)
            .WithPrincipal(new GenericPrincipal(
                new GenericIdentity("DemoUser", "DemoAuth"), new[] { "DemoAdmin" }));


        await logger.InfoAsync("Information goes here");
        await logger.WarningAsync("Warning goes here");
        await logger.ErrorAsync("Error goes here");
    }


    sealed class CustomAsyncLogger : Solitons.Diagnostics.Common.AsyncLogger
    {
        public static IAsyncLogger Create() => new CustomAsyncLogger();

        protected override Task LogAsync(LogEventArgs args)
        {
            // Your custom logging goes here
            return Task.CompletedTask;
        }
    }
}
```
In the RunAsync method, the custom logger is instantiated and configured with custom tags, properties, and a principal object. The logger is then used to capture and store different events using different log levels (i.e., Info, Warning, and Error).

The key feature of this example is the creation of a custom logger implementation that overrides the LogAsync method. This method is responsible for handling the actual logging of events and provides developers with the flexibility to implement their own custom logic for capturing and storing events. The example implementation provided simply returns a completed Task, but this is where custom logging logic would be written in a real-world scenario.

By creating a custom implementation of the IAsyncLogger interface, developers can tailor the logger to the specific needs of their application and its environment. This provides a powerful tool for capturing and storing events in a customized and efficient manner, while also allowing developers to control the flow of logging and integrate with other systems as needed.
