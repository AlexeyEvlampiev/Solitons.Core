using System;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Solitons
{
    // ReSharper disable once InconsistentNaming
    public class IEnvironment_ParseCommandLine_Should
    {
        [Theory]
        [InlineData("-a x -b y %ENV_TEST_VARIABLE%", new[]{"-a", "x", "-b", "y"})]
        [InlineData("-a \"Hello world!\" %ENV_TEST_VARIABLE_B%", new[] { "-a", "Hello world!" })]
        public void Work(string commandLine, string[] expectedArgs)
        {
            var envKey = Regex.Match(commandLine, $"%(\\S+)%").Result("$1");
            var envValue = Guid.NewGuid().ToString();
            Environment.SetEnvironmentVariable(envKey, envValue);

            var actualArgs = IEnvironment.System.ParseCommandArgs(commandLine);

            Array.ForEach(expectedArgs, a=> Assert.True(actualArgs.Contains(a)));
            Assert.True(actualArgs.Contains(envValue));
        }
    }
}
