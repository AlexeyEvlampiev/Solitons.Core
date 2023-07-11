using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Security.Common;

namespace Solitons.Security;

sealed class EnvironmentSecretsRepository : SecretsRepository
{
    private readonly EnvironmentVariableTarget _target;

    public EnvironmentSecretsRepository(EnvironmentVariableTarget target)
    {
        _target = target;
    }


    protected override Task<string[]> ListSecretNamesAsync(CancellationToken cancellation)
    {
        var variables = Environment.GetEnvironmentVariables(_target);
        var keys = new HashSet<string>(StringComparer.Ordinal);
        foreach (var key in variables.Keys)
        {
            if (key != null)
            {
                keys.Add(key.ToString() ?? string.Empty);
            }
        }

        return Task.FromResult(keys.ToArray());
    }

    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>The value of the secret with the specified name.</returns>
    protected override async Task<string> GetSecretAsync(string secretName, CancellationToken cancellation)
    {
        return (await GetSecretIfExistsAsync(secretName, cancellation))
            .ThrowIfNullOrEmpty(() => new KeyNotFoundException()
            {
                Data = { [GetType()] = this }
            });
    }

    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name, if it exists in the repository.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>The value of the secret with the specified name, or <c>null</c> if it does not exist in the repository.</returns>
    protected override Task<string?> GetSecretIfExistsAsync(string secretName, CancellationToken cancellation) => Task.FromResult(Environment.GetEnvironmentVariable(secretName, _target));

    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name, or sets it to the specified default value if it does not exist in the repository.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="defaultValue">The default value to set if the secret does not exist.</param>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>The value of the secret with the specified name, or the default value if it does not exist in the repository.</returns>
    protected override async Task<string> GetOrSetSecretAsync(string secretName, string defaultValue, CancellationToken cancellation)
    {
        var secretValue = await GetSecretIfExistsAsync(secretName, cancellation);
        if (secretValue.IsNullOrWhiteSpace())
        {
            Environment.SetEnvironmentVariable(secretName, defaultValue, _target);
            return defaultValue;
        }

        return secretValue!;
    }

    /// <summary>
    /// Asynchronously sets the value of the secret with the specified name.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="secretValue">The value of the secret.</param>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override Task SetSecretAsync(string secretName, string secretValue, CancellationToken cancellation)
    {
        Environment.SetEnvironmentVariable(secretName, secretValue, _target);
        return Task.CompletedTask;
    }

    protected override bool IsSecretNotFoundError(Exception exception)
    {
        return exception is KeyNotFoundException keyNotFoundException &&
               keyNotFoundException.Data.Contains(GetType()) &&
               keyNotFoundException.Data[GetType()] == this;
    }
}