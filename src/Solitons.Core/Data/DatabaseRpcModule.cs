using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// RPC routed invocation handler. 
    /// </summary>
    sealed class DatabaseRpcModule : IDatabaseRpcModule
    {
        private readonly IReadOnlyDictionary<Guid, Type> _commandTypes;
        private readonly IServiceProvider _provider;

        public DatabaseRpcModule(IReadOnlyDictionary<Guid, Type> commandTypes, IServiceProvider provider)
        {
            _commandTypes = commandTypes;
            _provider = provider;
        }

        [DebuggerNonUserCode]
        public bool Contains(Guid commandId) => _commandTypes.ContainsKey(commandId);

        [DebuggerStepThrough]
        public Task<MediaContent> InvokeAsync(Guid commandId, MediaContent request, CancellationToken cancellation = default)
        {
            cancellation.ThrowIfCancellationRequested();
            return GetCommand(commandId)
                .InvokeAsync(request, cancellation);
        }

        [DebuggerStepThrough]
        public Task SendAsync(Guid commandId, MediaContent content, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return GetCommand(commandId)
                .SendAsync(content, cancellation);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        [DebuggerNonUserCode]
        public IDatabaseRpcCommand GetCommand(Guid commandId)
        {
            commandId.ThrowIfEmptyArgument(nameof(commandId));
            if (_commandTypes.TryGetValue(commandId, out var commandType))
            {
                var command = _provider.GetService(commandType);
                return (IDatabaseRpcCommand)command!;
            }

            throw new KeyNotFoundException($"The module does not include the {commandId} context.");
        }
    }
}
