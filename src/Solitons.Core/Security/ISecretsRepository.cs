using Solitons.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Reactive;

namespace Solitons.Security;

/// <summary>
/// Represents a repository of secrets, such as passwords or API keys.
/// </summary>
public partial interface ISecretsRepository
{
    /// <summary>
    /// Asynchronously returns an array of names of all secrets in the repository.
    /// </summary>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>An array of names of all secrets in the repository.</returns>
    Task<string[]> ListSecretNamesAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <returns>The value of the secret with the specified name.</returns>
    Task<string> GetSecretAsync(string secretName);

    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name, if it exists in the repository.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <returns>The value of the secret with the specified name, or <c>null</c> if it does not exist in the repository.</returns>
    Task<string?> GetSecretIfExistsAsync(string secretName);


    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name, or sets it to the specified default value if it does not exist in the repository.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="defaultValue">The default value to set if the secret does not exist.</param>
    /// <returns>The value of the secret with the specified name, or the default value if it does not exist in the repository.</returns>
    Task<string> GetOrSetSecretAsync(string secretName, string defaultValue);

    /// <summary>
    /// Asynchronously sets the value of the secret with the specified name.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="secretValue">The value of the secret.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetSecretAsync(string secretName, string secretValue);

    /// <summary>
    /// Determines if the specified exception is a "secret not found" error.
    /// </summary>
    /// <param name="exception">The exception to check.</param>
    /// <returns><c>true</c> if the exception is a "secret not found" error; otherwise, <c>false</c>.</returns>
    bool IsSecretNotFoundError(Exception exception);
}

public partial interface ISecretsRepository
{
    /// <summary>
    /// Creates an in-memory secrets repository with an empty dictionary.
    /// </summary>
    /// <returns>An in-memory secrets repository.</returns>
    public static ISecretsRepository InMemory() =>
        new InMemorySecretsRepository(new Dictionary<string, string>(StringComparer.Ordinal));

    /// <summary>
    /// Creates an in-memory secrets repository with a dictionary using the specified comparer.
    /// </summary>
    /// <param name="comparer">The comparer to use for comparing secret names.</param>
    /// <returns>An in-memory secrets repository with a dictionary using the specified comparer.</returns>
    public static ISecretsRepository InMemory(StringComparer comparer) =>
        new InMemorySecretsRepository(new Dictionary<string, string>(comparer));

    /// <summary>
    /// Creates an in-memory secrets repository with the specified dictionary.
    /// </summary>
    /// <param name="secrets">The dictionary to use as the underlying storage for the repository.</param>
    /// <returns>An in-memory secrets repository with the specified dictionary.</returns>
    public static ISecretsRepository InMemory(IDictionary<string, string> secrets) =>
        new InMemorySecretsRepository(secrets);

    /// <summary>
    /// Creates an in-memory secrets repository with a dictionary configured using the specified action.
    /// </summary>
    /// <param name="config">The action that configures the dictionary to use as the underlying storage for the repository.</param>
    /// <returns>An in-memory secrets repository with a dictionary configured using the specified action.</returns>
    [DebuggerStepThrough]
    public static ISecretsRepository InMemory(Action<FluentDictionary<string, string>> config)
    {
        var secrets = FluentDictionary.Create<string, string>(StringComparer.Ordinal);
        config.Invoke(secrets);
        return InMemory(secrets);
    }



    /// <summary>
    /// Creates an environment variable secrets repository with the specified target.
    /// </summary>
    /// <param name="target">The target of the environment variables to retrieve.</param>
    /// <returns>An environment variable secrets repository with the specified target.</returns>
    public static ISecretsRepository Environment(EnvironmentVariableTarget target) =>
        new EnvironmentSecretsRepository(target);


    /// <summary>
    /// Returns a new secrets repository that reads secrets from this repository, but caches them for a limited time period.
    /// </summary>
    /// <param name="cacheExpiration">An observable that signals when the cache should expire.</param>
    /// <param name="secretNameComparer">The comparer to use for comparing secret names.</param>
    /// <returns>A new secrets repository that reads secrets from this repository, but caches them for a limited time period.</returns>
    /// <exception cref="InvalidOperationException">Thrown if this method is called on a secrets repository that is already a part of a multilayered cache.</exception>
    [DebuggerStepThrough]
    public ISecretsRepository ReadThroughCache(
        IObservable<Unit> cacheExpiration,
        StringComparer secretNameComparer)
    {
        if (this is ReadThroughCacheSecretsRepository)
        {
            throw new InvalidOperationException("Cannot create a multilayered cache.");
        }

        return new ReadThroughCacheSecretsRepository(this, cacheExpiration, secretNameComparer);
    }

    /// <summary>
    /// Creates a read-through cache secrets repository with the specified secret name comparer.
    /// </summary>
    /// <param name="secretNameComparer">The comparer to use for comparing secret names.</param>
    /// <returns>A read-through cache secrets repository with the specified secret name comparer.</returns>
    [DebuggerStepThrough]
    public ISecretsRepository ReadThroughCache(
        StringComparer secretNameComparer) =>
        ReadThroughCache(Observable.Empty<Unit>(), secretNameComparer);

    /// <summary>
    /// Creates a read-through cache secrets repository with the specified cache expiration observable.
    /// </summary>
    /// <param name="cacheExpiration">An observable that signals when the cache should be invalidated.</param>
    /// <returns>A read-through cache secrets repository with the specified cache expiration observable.</returns>
    [DebuggerStepThrough]
    public ISecretsRepository ReadThroughCache(
        IObservable<Unit> cacheExpiration) =>
        ReadThroughCache(cacheExpiration, StringComparer.Ordinal);


    /// <summary>
    /// Gets the secret value for the specified secret name if it exists in the repository, or returns null otherwise.
    /// </summary>
    /// <param name="secretName">The name of the secret to retrieve.</param>
    /// <returns>The secret value for the specified secret name if it exists in the repository, or null otherwise.</returns>
    [DebuggerStepThrough]
    public string? GetSecretIfExists(string secretName) =>
        GetSecretIfExistsAsync(secretName)
            .GetAwaiter()
            .GetResult();

    /// <summary>
    /// Sets the secret value for the specified secret name.
    /// </summary>
    /// <param name="secretName">The name of the secret to set.</param>
    /// <param name="secretValue">The value to set for the specified secret.</param>
    [DebuggerStepThrough]
    public void SetSecret(string secretName, string secretValue) =>
        SetSecretAsync(secretName, secretValue)
            .GetAwaiter()
            .GetResult();

    /// <summary>
    /// Returns an observable that emits the key-value pair of the specified secret name and its value in the repository.
    /// </summary>
    /// <param name="key">The name of the secret to retrieve.</param>
    /// <returns>An observable that emits the key-value pair of the specified secret name and its value in the repository.</returns>
    public sealed IObservable<KeyValuePair<string, string>> Get(string key)
    {
        return FluentObservable
            .Defer(() => GetSecretAsync(key))
            .Select(value => KeyValuePair.Create(key, value));
    }

    /// <summary>
    /// Returns an observable that emits the key-value pair of the specified secret name and its value in the repository if it exists,
    /// or skips the emission otherwise.
    /// </summary>
    /// <param name="key">The name of the secret to retrieve.</param>
    /// <returns>An observable that emits the key-value pair of the specified secret name and its value in the repository if it exists,
    /// or skips the emission otherwise.</returns>
    [DebuggerStepThrough]
    public sealed IObservable<KeyValuePair<string, string>> GetIfExists(string key)
    {
        return FluentObservable
            .Defer(() => GetSecretIfExistsAsync(key))
            .Where(value => value.IsPrintable())
            .Select(value => KeyValuePair.Create(key, value!));
    }

    /// <summary>
    /// Returns an observable that emits a dictionary containing all secrets and their values in the repository, using the specified
    /// comparer for the secret names.
    /// </summary>
    /// <param name="comparer">The comparer to use for comparing secret names in the resulting dictionary.</param>
    /// <returns>An observable that emits a dictionary containing all secrets and their values in the repository.</returns>
    [DebuggerStepThrough]
    public sealed IObservable<Dictionary<string, string>> GetAll(StringComparer comparer)
    {
        return FluentObservable
            .Defer(() => ListSecretNamesAsync(CancellationToken.None))
            .SelectMany(_ => _)
            .SelectMany(Get)
            .ToList()
            .Select(list => list.ToDictionary(comparer));
    }

    /// <summary>
    /// Returns an observable that emits a dictionary containing all secrets and their values in the repository, using the ordinal
    /// comparer for the secret names.
    /// </summary>
    /// <returns>An observable that emits a dictionary containing all secrets and their values in the repository.</returns>
    [DebuggerStepThrough]
    public sealed IObservable<Dictionary<string, string>> GetAll() => GetAll(StringComparer.Ordinal);
}