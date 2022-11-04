using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Security.Common;

namespace Solitons.Security
{
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

        protected override async Task<string> GetSecretAsync(string secretName)
        {
            return (await GetSecretIfExistsAsync(secretName))
                .ThrowIfNullOrEmpty(() => new KeyNotFoundException()
                {
                    Data = { [GetType()] = this }
                });
        }

        protected override Task<string?> GetSecretIfExistsAsync(string secretName) => Task.FromResult(Environment.GetEnvironmentVariable(secretName, _target));

        protected override async Task<string> GetOrSetSecretAsync(string secretName, string defaultValue)
        {
            var secretValue = await GetSecretIfExistsAsync(secretName);
            if (secretValue.IsNullOrWhiteSpace())
            {
                Environment.SetEnvironmentVariable(secretName, defaultValue, _target);
                return defaultValue;
            }

            return secretValue!;
        }

        protected override Task SetSecretAsync(string secretName, string secretValue)
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
}
