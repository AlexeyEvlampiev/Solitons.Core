using Moq;
using Solitons;

namespace UsageExamples;

[Example]
public sealed class ExampleEnvironment
{
    public void Example()
    {
        var env = new Mock<IEnvironment>();
        env.Setup(_ => _.OSVersion).Returns(
            new OperatingSystem(
                PlatformID.MacOSX, 
                Version.Parse("10.1")));
        var program = new Program(env.Object);
        program.Run();
    }

    // Defines a Program class that utilizes an IEnvironment interface
    // to get information about the environment it's running on.
    public sealed class Program
    {
        // A read-only field to hold an instance of IEnvironment.
        private readonly IEnvironment _env;

        // A constructor that accepts an IEnvironment implementation.
        // This constructor allows dependency injection of custom IEnvironment
        // implementations, which can be useful for unit testing.
        public Program(IEnvironment env)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        // A default constructor that passes the system's environment
        // information to the internal constructor.
        // It utilizes a default implementation of IEnvironment.
        public Program() : this(IEnvironment.System) { }

        // The Run method checks if the current operating system platform is
        // supported, and if not, throws a NotSupportedException.
        public void Run()
        {
            // Define an array of supported platforms.
            var supportedPlatforms = new[] { PlatformID.MacOSX, PlatformID.Unix };

            // Check if the current platform is in the list of supported platforms.
            if (!supportedPlatforms.Contains(_env.OSVersion.Platform))
            {
                // Throw an exception if the current platform is not supported.
                throw new NotSupportedException($"{_env.OSVersion} is not supported");
            }

            // The rest of the code goes here.
        }
    }

}