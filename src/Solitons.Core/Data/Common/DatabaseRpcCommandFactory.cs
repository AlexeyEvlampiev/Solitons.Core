using System;
using System.Diagnostics;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    sealed class DatabaseRpcCommandFactory : IDatabaseRpcCommandFactory
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IDatabaseRpcCommandLookup _lookup;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookup"></param>
        /// <param name="serviceProvider"></param>
        [DebuggerNonUserCode]
        public DatabaseRpcCommandFactory(IDatabaseRpcCommandLookup lookup, IServiceProvider serviceProvider)
        {
            _lookup = lookup;
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandOid"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [DebuggerStepThrough]
        public IDatabaseRpcCommand? Create(Guid commandOid)
        {
            var commandType = _lookup.FindCommandType(commandOid);
            if(commandType == null) return null;

            var instance = _serviceProvider.GetService(commandType);
            return instance as IDatabaseRpcCommand ?? throw new InvalidOperationException($"Internal factory created an object that does not implement {typeof(IDatabaseRpcCommand)}");
        }
    }
}
