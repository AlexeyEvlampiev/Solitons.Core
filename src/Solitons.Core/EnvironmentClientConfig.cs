using System;
using System.Diagnostics;

namespace Solitons;

public sealed class EnvironmentClientConfig
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private EventHandler<MissingVariableEventArgs> _missingVariableHandlers;

    internal EnvironmentClientConfig()
    {
    }

    public event EventHandler<MissingVariableEventArgs> EnvironmentVariableNotFound
    {
        add => _missingVariableHandlers += value;
        remove => _missingVariableHandlers -= value;
    }

    public sealed class MissingVariableEventArgs : EventArgs
    {
        public MissingVariableEventArgs(string variableName)
        {
            VariableName = variableName;
        }

        public string VariableName { get; }
    }


    internal void OnEnvironmentVariableNotFound(string value)
    {
        _missingVariableHandlers?.Invoke(this, new MissingVariableEventArgs(value));
    }
}