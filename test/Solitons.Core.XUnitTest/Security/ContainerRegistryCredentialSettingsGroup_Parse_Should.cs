using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Solitons.Collections;
using Xunit;

namespace Solitons.Security
{
    // ReSharper disable once InconsistentNaming
    public sealed class ContainerRegistryCredentialSettingsGroup_Parse_Should
    {
        [Theory]
        [InlineData("contoso.azurecr.io", "john", "p@ssword")]
        [InlineData("http://contoso.azurecr.io", "john", "p@ssword")]
        [InlineData("https://contoso.azurecr.io", "john", "p@ssword")]
        public void HandleValidInput(string server, string user, string password)
        {

            var inputs =
                from serverKey in new[] { "server", "host", "address", "url", "uri", "endpoint" }
                from userKey in new[] { "user", "user-name", "username", "login", "usr", "u" }
                from passwordKey in new[] { "password", "pass", "pwd", "p", "secret" }
                from delimiter in new[] {";", " ; "}
                select FluentArray.Create(
                    $"{serverKey}={server}",
                    $"{userKey}={user}",
                    $"{passwordKey}={password}").Join(delimiter);

            var expectedServer = Regex.Replace(server, @"^https?://", String.Empty);
            var expectedUri = new Uri($"https://{expectedServer}");
            var expectedHost = Regex.Match(expectedServer, @"^\w+").Value;

            int counter = 0;
            foreach (var input in inputs)
            {
                Debug.WriteLine($"{counter++}: {input}");
                var settings = ContainerRegistryCredentialSettingsGroup.Parse(input);
                Assert.Equal(expectedServer, settings.Server);
                Assert.Equal(user, settings.Username);
                Assert.Equal(password, settings.Password);
                Assert.Equal(expectedUri, settings.Uri);
                Assert.Equal(expectedHost, settings.Host);
            }
        }

        [Theory]
        [InlineData("server=...; user=john; pwd=testpwd")]
        [InlineData("server=<>; user=john; pwd=testpwd")]
        [InlineData("server=contoso")]
        public void ThrowFormatException(string input)
        {
            Assert.Throws<FormatException>(()=> ContainerRegistryCredentialSettingsGroup.Parse(input));
        }
        
    }
}
