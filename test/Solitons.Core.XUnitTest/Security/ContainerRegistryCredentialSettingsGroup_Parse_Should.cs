using System.Diagnostics;
using System.Linq;
using Solitons.Collections;
using Xunit;

namespace Solitons.Security
{
    // ReSharper disable once InconsistentNaming
    public sealed class ContainerRegistryCredentialSettingsGroup_Parse_Should
    {
        [Fact]
        public void HandleValidInput()
        {

            var inputs =
                from serverKey in new[] { "server", "host", "address", "url", "uri", "endpoint" }
                from userKey in new[] { "user", "user-name", "username", "login", "usr", "u" }
                from passwordKey in new[] { "password", "pass", "pwd", "p", "secret" }
                from delimiter in new[] {";", " ; "}
                select FluentArray.Create(
                    $"{serverKey}=contoso.azurecr.io",
                    $"{userKey}=john",
                    $"{passwordKey}=test-password").Join(delimiter);
            int counter = 0;
            foreach (var input in inputs)
            {
                Debug.WriteLine($"{counter++}: {input}");
                var settings = ContainerRegistryCredentialSettingsGroup.Parse(input);
                Assert.Equal("contoso.azurecr.io", settings.Host);
                Assert.Equal("john", settings.Username);
                Assert.Equal("test-password", settings.Password);
            }
        }
    }
}
