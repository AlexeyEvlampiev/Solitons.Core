using System.Diagnostics;
using Solitons.Security;

namespace UsageExamples.Data;


[Example]
public sealed class ExampleUsingSecretsRepository
{
    public async Task Example()
    {
        // Instantiate a secrets repository interface with process environment variables as the source.
        ISecretsRepository envRepository = ISecretsRepository.Environment(EnvironmentVariableTarget.Process);

        // Pre-populate the DEMO_SECRET environment variable for this example.
        Environment.SetEnvironmentVariable("DEMO_SECRET", "Secret value goes here...");

        // Retrieve the DEMO_SECRET using the secrets repository interface.
        var secret = await envRepository.GetSecretAsync("DEMO_SECRET");
        Debug.Assert(secret == "Secret value goes here...", "Ensure the DEMO_SECRET value is correctly set in the environment variables.");

        // Update the DEMO_SECRET with a new value.
        await envRepository.SetSecretAsync("DEMO_SECRET", "New value goes here...");
        Debug.Assert("New value goes here..." == Environment.GetEnvironmentVariable("DEMO_SECRET"));
    }

}
