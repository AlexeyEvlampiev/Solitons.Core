using System;
using System.Diagnostics;

namespace Solitons.Common
{
    sealed class EnvironmentProxy : IEnvironment
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IEnvironment _innerEnvironment;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly EnvironmentClientConfig _config;

        public EnvironmentProxy(IEnvironment innerEnvironment, EnvironmentClientConfig config)
        {
            _innerEnvironment = innerEnvironment;
            _config = config;
        }

        public string CommandLine => _innerEnvironment.CommandLine;

        public string CurrentDirectory
        {
            get => _innerEnvironment.CurrentDirectory;
            set => _innerEnvironment.CurrentDirectory = value;
        }

        public IEnvironment With(Action<EnvironmentClientConfig> options)
        {
            return _innerEnvironment.With(options);
        }

        [DebuggerStepThrough]
        public string? GetEnvironmentVariable(string variable)
        {
            var value = _innerEnvironment.GetEnvironmentVariable(variable);
            if (value == null)
            {
                _config.OnEnvironmentVariableNotFound(variable);
            }
            return value;
        }

        [DebuggerStepThrough]
        public string? GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
        {
            var value = _innerEnvironment.GetEnvironmentVariable(variable, target);
            if (value == null)
            {
                _config.OnEnvironmentVariableNotFound(variable);
            }
            return value;
        }

        public override string ToString() => _innerEnvironment.ToString() ?? _innerEnvironment.GetType().ToString();
    }
}
