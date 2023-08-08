using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Reactive;

namespace Solitons.Security.Common;

/// <summary>
/// Provides an abstract base class for implementing a repository of secrets, such as passwords or API keys.
/// This class cannot be instantiated. Derived classes must provide concrete implementations of the defined methods.
/// </summary>
public abstract class SecretsRepository : ISecretsRepository
{
    private const int DefaultMinRetryMilliseconds = 30;
    private const int DefaultMaxRetryDelayScaleFactorExponent = 100;
    private const double DefaultRetryDelayScaleFactor = 1.1;

    /// <summary>
    /// When overridden in a derived class, asynchronously retrieves an array of names of all secrets in the repository.
    /// </summary>
    /// <param name="cancellation">A cancellation token that should be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a value that is an array of names of all secrets in the repository.</returns>
    protected abstract Task<string[]> ListSecretNamesAsync(CancellationToken cancellation);

    /// <summary>
    /// When overridden in a derived class, asynchronously retrieves the value of the secret with the specified name.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="cancellation">A cancellation token that should be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a value that is the value of the secret with the specified name.</returns>
    protected abstract Task<string> GetSecretAsync(string secretName, CancellationToken cancellation);

    /// <summary>
    /// When overridden in a derived class, asynchronously retrieves the value of the secret with the specified name, or null if it does not exist in the repository.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="cancellation">A cancellation token that should be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a value that is the value of the secret with the specified name, or null if the secret does not exist in the repository.</returns>
    protected abstract Task<string?> GetSecretIfExistsAsync(string secretName, CancellationToken cancellation);

    /// <summary>
    /// When overridden in a derived class, asynchronously retrieves the value of the secret with the specified name, or sets it to the specified default value if it does not exist in the repository.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="defaultValue">The default value to set if the secret does not exist in the repository.</param>
    /// <param name="cancellation">A cancellation token that should be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, with a value that is the value of the secret with the specified name, or the default value if the secret does not exist in the repository.</returns>
    protected abstract Task<string> GetOrSetSecretAsync(string secretName, string defaultValue, CancellationToken cancellation);

    /// <summary>
    /// When overridden in a derived class, asynchronously sets the value of the secret with the specified name in the repository.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="secretValue">The value to set for the secret.</param>
    /// <param name="cancellation">A cancellation token that should be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected abstract Task SetSecretAsync(string secretName, string secretValue, CancellationToken cancellation);

    /// <summary>
    /// When overridden in a derived class, determines if the specified exception is a "secret not found" error.
    /// </summary>
    /// <param name="exception">The exception to evaluate.</param>
    /// <returns>True if the exception is a "secret not found" error; otherwise, false.</returns>
    protected abstract bool IsSecretNotFoundError(Exception exception);

    /// <summary>
    /// Determines whether a retry attempt should be made for a failed operation.
    /// </summary>
    /// <param name="args">The <see cref="RetryPolicyArgs"/> containing information about the failed operation and the current retry attempt.</param>
    /// <returns>
    /// <c>true</c> if a retry attempt should be made; otherwise, <c>false</c>.
    /// </returns>
    protected virtual bool ShouldRetry(RetryPolicyArgs args) => false;

    /// <summary>
    /// Calculates the delay before the next retry attempt based on the retry policy arguments.
    /// </summary>
    /// <param name="args">The <see cref="RetryPolicyArgs"/> containing information about the failed operation and the current retry attempt.</param>
    /// <returns>
    /// The <see cref="TimeSpan"/> representing the delay before the next retry attempt.
    /// </returns>
    protected virtual TimeSpan CalcRetryDelay(RetryPolicyArgs args) => TimeSpan
        .FromMilliseconds(DefaultMinRetryMilliseconds)
        .ScaleByFactor(
            DefaultRetryDelayScaleFactor, 
            args.AttemptNumber.Max(DefaultMaxRetryDelayScaleFactorExponent));

    /// <summary>
    /// Determines if the provided secret name is valid. A secret name is considered valid if it is printable.
    /// Derived classes can override this method to provide specific secret name validation logic.
    /// </summary>
    /// <param name="secretName">The name of the secret to evaluate.</param>
    /// <returns>True if the secret name is valid; otherwise, false.</returns>
    protected virtual bool IsValidSecretName(string secretName) => secretName.IsPrintable();


    [DebuggerStepThrough]
    async Task<string[]> ISecretsRepositoryReader.ListSecretNamesAsync(CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return await Observable
            .FromAsync(()=> ListSecretNamesAsync(cancellation))
            .WithRetryPolicy(args => args
                .SignalNextAttempt(ShouldRetry(args))
                .Delay(CalcRetryDelay(args), cancellation))
            .ToTask(cancellation);
    }

    


    [DebuggerStepThrough]
    async Task<string> ISecretsRepositoryReader.GetSecretAsync(string secretName, CancellationToken cancellation)
    {
        if (false == IsValidSecretName(secretName))
            throw new ArgumentException($"'{secretName}' is not a valid secret name.");
        cancellation.ThrowIfCancellationRequested();
        return await Observable
            .FromAsync(() => GetSecretAsync(secretName, cancellation))
            .WithRetryPolicy(args => args.SignalNextAttempt(ShouldRetry(args)))
            .ToTask(cancellation);
    }

    [DebuggerStepThrough]
    async Task<string?> ISecretsRepositoryReader.GetSecretIfExistsAsync(string secretName, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        if (!IsValidSecretName(secretName))
        {
            return null;
        }
        return await Observable
            .FromAsync(() => GetSecretIfExistsAsync(secretName, cancellation))
            .WithRetryPolicy(args => args.SignalNextAttempt(ShouldRetry(args)))
            .ToTask(cancellation);
    }

    [DebuggerStepThrough]
    async Task<string> ISecretsRepository.GetOrSetSecretAsync(string secretName, string defaultValue, CancellationToken cancellation)
    {
        if (false == IsValidSecretName(secretName))
            throw new ArgumentException($"'{secretName}' is not a valid secret name.");
        cancellation.ThrowIfCancellationRequested();
        defaultValue = ThrowIf.ArgumentNullOrWhiteSpace(defaultValue, nameof(defaultValue));
        return await Observable
            .FromAsync(() => GetOrSetSecretAsync(secretName, defaultValue, cancellation))
            .WithRetryPolicy(args => args.SignalNextAttempt(ShouldRetry(args)))
            .ToTask(cancellation);
    }

    [DebuggerStepThrough]
    async Task ISecretsRepositoryWriter.SetSecretAsync(string secretName, string secretValue, CancellationToken cancellation)
    {
        if (false == IsValidSecretName(secretName))
            throw new ArgumentException($"'{secretName}' is not a valid secret name.");
        secretValue = ThrowIf.ArgumentNullOrWhiteSpace(secretValue, nameof(secretValue));
        cancellation.ThrowIfCancellationRequested();
        await Observable
            .FromAsync(() => SetSecretAsync(secretName, secretValue, cancellation))
            .WithRetryPolicy(args => args.SignalNextAttempt(ShouldRetry(args)))
            .ToTask(cancellation);
    }

    [DebuggerStepThrough]
    bool ISecretsRepositoryReader.IsSecretNotFoundError(Exception exception) => IsSecretNotFoundError(exception);
}