using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Security.Common;

namespace Solitons.Security;

sealed class InMemorySecretsRepository : SecretsRepository
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly ConcurrentDictionary<string, string> _secrets;

    public InMemorySecretsRepository(IDictionary<string, string> secrets)
    {
        _secrets = new ConcurrentDictionary<string, string>(secrets, StringComparer.Ordinal);
    }
    protected override async Task<string[]> ListSecretNamesAsync(CancellationToken cancellation)
    {
        return await _secrets
            .Keys
            .ToObservable()
            .ToArray();
    }

    protected override Task<string> GetSecretAsync(string secretName, CancellationToken cancellation)
    {
        try
        {
            return Task.FromResult(_secrets[secretName]);
        }
        catch (KeyNotFoundException e)
        {
            e.Data.Add(GetType(), this);
            throw;
        }
            
    }

    protected override Task<string?> GetSecretIfExistsAsync(string secretName, CancellationToken cancellation)
    {
        var result = _secrets.TryGetValue(secretName, out var value)
            ? value
            : null;
        return Task.FromResult(result);
    }

    protected override Task<string> GetOrSetSecretAsync(string secretName, string defaultValue, CancellationToken cancellation)
    {
        var result = _secrets.GetOrAdd(secretName, defaultValue);
        return Task.FromResult(result);
    }

    protected override Task SetSecretAsync(string secretName, string secretValue, CancellationToken cancellation)
    {
        _secrets[secretName] = secretValue;
        return Task.CompletedTask;
    }

    protected override bool IsSecretNotFoundError(Exception exception)
    {
        return exception is KeyNotFoundException keyNotFoundException &&
               keyNotFoundException.Data.Contains(GetType());
    }
}