using System;
using System.Diagnostics;
using Solitons.Common;

namespace Solitons
{
    sealed class SysEnvironment : EnvironmentBase
    {
        private SysEnvironment(){}

        public static readonly SysEnvironment Instance = new();

        [DebuggerNonUserCode]
        public override string ToString() => typeof(Environment).ToString();

    }
}
