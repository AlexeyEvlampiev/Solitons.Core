using Solitons.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;

namespace Solitons.Security;

public partial interface ISecretsRepository
{
    #region InMemory

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

    #endregion

    #region Environment Variables

    /// <summary>
    /// Creates an environment variable secrets repository with the specified target.
    /// </summary>
    /// <param name="target">The target of the environment variables to retrieve.</param>
    /// <returns>An environment variable secrets repository with the specified target.</returns>
    public static ISecretsRepository Environment(EnvironmentVariableTarget target) =>
        new EnvironmentSecretsRepository(target);

    #endregion

    #region Caching

    /// <summary>
    /// Returns a new secrets repository that reads secrets from this repository, but caches them using a custom expiration trigger.
    /// </summary>
    /// <typeparam name="T">The type of the items in the cache expiration observable sequence.</typeparam>
    /// <param name="cacheExpiration">An observable sequence where each item indicates that the cache should be invalidated.</param>
    /// <returns>A new secrets repository that reads secrets from this repository, but caches them based on the provided observable sequence.</returns>
    [DebuggerStepThrough]
    public ISecretsRepository CacheWithExpiration<T>(
        IObservable<T> cacheExpiration)
    {
        var trigger = cacheExpiration.Select(_ => Unit.Default);
        return CacheWithExpiration(trigger);
    }

    /// <summary>
    /// Returns a new secrets repository that reads secrets from this repository, but caches them for a limited time period.
    /// </summary>
    /// <param name="cacheExpiration">An observable that signals when the cache should expire.</param>
    /// <param name="secretNameComparer">The comparer to use for comparing secret names.</param>
    /// <returns>A new secrets repository that reads secrets from this repository, but caches them for a limited time period.</returns>
    /// <exception cref="InvalidOperationException">Thrown if this method is called on a secrets repository that is already a part of a multilayered cache.</exception>
    [DebuggerStepThrough]
    public ISecretsRepository CacheWithExpiration(
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
    /// Returns a new secrets repository that reads secrets from this repository, but caches them using a custom expiration trigger and a specific name comparer.
    /// </summary>
    /// <typeparam name="T">The type of the items in the cache expiration observable sequence.</typeparam>
    /// <param name="cacheExpiration">An observable sequence where each item indicates that the cache should be invalidated.</param>
    /// <param name="secretNameComparer">The comparer to use for comparing secret names.</param>
    /// <returns>A new secrets repository that reads secrets from this repository, but caches them based on the provided observable sequence and name comparer.</returns>
    [DebuggerStepThrough]
    public ISecretsRepository CacheWithExpiration<T>(
        IObservable<T> cacheExpiration,
        StringComparer secretNameComparer)
    {
        var trigger = cacheExpiration.Select(_ => Unit.Default);
        return CacheWithExpiration(trigger, secretNameComparer);
    }

    /// <summary>
    /// Creates a read-through cache secrets repository with the specified secret name comparer.
    /// </summary>
    /// <param name="secretNameComparer">The comparer to use for comparing secret names.</param>
    /// <returns>A read-through cache secrets repository with the specified secret name comparer.</returns>
    [DebuggerStepThrough]
    public ISecretsRepository CacheWithExpiration(
        StringComparer secretNameComparer) =>
        CacheWithExpiration(Observable.Empty<Unit>(), secretNameComparer);

    /// <summary>
    /// Creates a read-through cache secrets repository with the specified cache expiration observable.
    /// </summary>
    /// <param name="cacheExpiration">An observable that signals when the cache should be invalidated.</param>
    /// <returns>A read-through cache secrets repository with the specified cache expiration observable.</returns>
    [DebuggerStepThrough]
    public ISecretsRepository CacheWithExpiration(
        IObservable<Unit> cacheExpiration) =>
        CacheWithExpiration(cacheExpiration, StringComparer.Ordinal);

    #endregion
}
