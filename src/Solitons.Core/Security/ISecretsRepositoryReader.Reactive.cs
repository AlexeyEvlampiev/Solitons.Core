using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using System.Linq;
namespace Solitons.Security;

public partial interface ISecretsRepositoryReader
{
    /// <summary>
    /// Returns an observable that emits the key-value pair of the specified secret name and its value in the repository if it exists,
    /// or skips the emission otherwise.
    /// </summary>
    /// <param name="key">The name of the secret to retrieve.</param>
    /// <returns>An observable that emits the key-value pair of the specified secret name and its value in the repository if it exists,
    /// or skips the emission otherwise.</returns>
    [DebuggerStepThrough]
    public sealed IObservable<KeyValuePair<string, string>> GetSecret(string key)
    {
        return Observable
            .FromAsync(() => GetSecretIfExistsAsync(key))
            .Where(value => value.IsPrintable())
            .Select(value => KeyValuePair.Create(key, value!));
    }

    /// <summary>
    /// Returns an observable that emits a dictionary containing all secrets and their values in the repository, using the ordinal
    /// comparer for the secret names.
    /// </summary>
    /// <returns>An observable that emits a dictionary containing all secrets and their values in the repository.</returns>
    [DebuggerStepThrough]
    public sealed IObservable<Dictionary<string, string>> FetchAll() => FetchAll(StringComparer.Ordinal);


    /// <summary>
    /// Returns an observable that emits a dictionary containing all secrets and their values in the repository, using the specified
    /// comparer for the secret names.
    /// </summary>
    /// <param name="comparer">The comparer to use for comparing secret names in the resulting dictionary.</param>
    /// <returns>An observable that emits a dictionary containing all secrets and their values in the repository.</returns>
    [DebuggerStepThrough]
    public sealed IObservable<Dictionary<string, string>> FetchAll(StringComparer comparer)
    {
        return Observable
            .FromAsync(() => ListSecretNamesAsync(CancellationToken.None))
            .SelectMany(_ => _)
            .SelectMany(GetSecret)
            .ToList()
            .Select(list => list.ToDictionary(comparer));
    }
}
