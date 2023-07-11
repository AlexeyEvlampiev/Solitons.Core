using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Security.Common;

namespace Solitons.Security
{
    internal sealed class ReadThroughCacheSecretsRepository : SecretsRepositoryProxy
    {
        private const int NullCacheCapacity = 1000;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private ConcurrentDictionary<string, string?> _secrets;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDisposable _cacheUpdate;

        [DebuggerNonUserCode]
        public ReadThroughCacheSecretsRepository(
            ISecretsRepository innerRepository, 
            IObservable<Unit> cacheExpiration, 
            StringComparer comparer) 
            : base(innerRepository)
        {
            _secrets = new ConcurrentDictionary<string, string?>(comparer);
            _cacheUpdate = cacheExpiration
                .Subscribe( _ =>
                {
                    _secrets
                        .Where(p => p.Value != null)
                        .Select(p => p.Key)
                        .ToObservable()
                        .SelectMany(secretName => base
                            .GetSecretIfExistsAsync(secretName)
                            .ToObservable()
                            .Select(value => KeyValuePair.Create(secretName, value)))
                        .ToList()
                        .Select(latest=> new ConcurrentDictionary<string, string?>(latest, comparer))
                        .Subscribe(latest => _secrets = latest);
                });

        }

        [DebuggerStepThrough]
        public override async Task<string> GetSecretAsync(string secretName, CancellationToken cancellation = default)
        {
            if (_secrets.TryGetValue(secretName, out string? value))
            {
                if (value == null)
                {
                    throw new KeyNotFoundException()
                    {
                        Data = { [GetType()] = secretName }
                    };
                }

                return value;
            }

            try
            {
                value = await base.GetSecretAsync(secretName, cancellation);
                _secrets.TryAdd(secretName, value);
                return value;
            }
            catch (Exception e) when(base.IsSecretNotFoundError(e))
            {
                if (_secrets.Count(p => p.Value is null) > NullCacheCapacity)
                {
                    _secrets
                        .Where(p => p.Value is null)
                        .ForEach(p=> _secrets.TryRemove(p));
                }
                _secrets.TryAdd(secretName, null);
                throw;
            }
        }

        [DebuggerStepThrough]
        public override bool IsSecretNotFoundError(Exception exception)
        {
            if (exception is KeyNotFoundException keyNotFound &&
                keyNotFound.Data.Contains(GetType()))
            {
                return true;
            }

            return base.IsSecretNotFoundError(exception);
        }

        [DebuggerStepThrough]
        public override async Task<string> GetOrSetSecretAsync(string secretName, string defaultValue, CancellationToken cancellation = default)
        {
            if (_secrets.TryGetValue(secretName, out var value) && 
                value != null)
            {
                return value!;
            }

            value = await base.GetOrSetSecretAsync(secretName, defaultValue, cancellation);
            _secrets.TryAdd(secretName, value);
            return value;
        }

        [DebuggerStepThrough]
        public override async Task<string?> GetSecretIfExistsAsync(string secretName, CancellationToken cancellation = default)
        {
            if (_secrets.TryGetValue(secretName, out var value) &&
                value != null)
            {
                return value!;
            }

            value = await base.GetSecretIfExistsAsync(secretName, cancellation);
            _secrets.TryAdd(secretName, value);
            return value;
        }

        [DebuggerStepThrough]
        public override async Task SetSecretAsync(string secretName, string secretValue, CancellationToken cancellation = default)
        {
            await base.SetSecretAsync(secretName, secretValue, cancellation);
            _secrets.TryAdd(secretName, secretValue);
        }
    }
}
