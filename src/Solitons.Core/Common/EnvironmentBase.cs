using System;
using System.Diagnostics;

namespace Solitons.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EnvironmentBase : IEnvironment
    {
        [DebuggerNonUserCode]
        public virtual string CommandLine => Environment.CommandLine;

        public virtual string CurrentDirectory
        {
            [DebuggerNonUserCode]
            get => Environment.CurrentDirectory;
            [DebuggerNonUserCode]
            set => Environment.CurrentDirectory = value;
        }


        [DebuggerStepThrough]
        public IEnvironment With(Action<EnvironmentClientConfig> config)
        {
            var options = new EnvironmentClientConfig();
            config?.Invoke(options);
            return new EnvironmentProxy(this, options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public virtual string GetEnvironmentVariable(string variable) => Environment.GetEnvironmentVariable(variable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public virtual string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target) => Environment.GetEnvironmentVariable(variable, target);
    }
}
