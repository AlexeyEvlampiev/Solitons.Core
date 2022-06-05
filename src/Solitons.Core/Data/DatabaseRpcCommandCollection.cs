using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DatabaseRpcCommandCollection : IEnumerable<IDatabaseRpcCommand>
    {
        private readonly Dictionary<Guid, IDatabaseRpcCommand> _commands = new();

        private DatabaseRpcCommandCollection(
            Assembly[] assemblies, 
            Func<DatabaseRpcCommandMetadata, bool> commandSelector,
            Func<Type, DatabaseRpcCommand> commandBuilder)
        {
            var metadata = DatabaseRpcCommandMetadata
                .From(assemblies)
                .Where(commandSelector);

            foreach (var item in metadata)
            {
                var command = commandBuilder.Invoke(item.CommandType);
                _commands.Add(item.CommandOid, command);
            }
        }

        [DebuggerStepThrough]
        public static DatabaseRpcCommandCollection Create(
            IEnumerable<Assembly> assemblies,
            Func<DatabaseRpcCommandMetadata, bool> commandSelector,
            Func<Type, DatabaseRpcCommand> commandBuilder)
        {
            var arr = assemblies.SkipNulls().ToArray();
            if (arr.Length == 0)
                throw new ArgumentException($"", nameof(assemblies));
            return new DatabaseRpcCommandCollection(arr, commandSelector, commandBuilder);
        }


        [DebuggerStepThrough]
        public static DatabaseRpcCommandCollection Create(
            IEnumerable<Assembly> assemblies,
            Func<Type, DatabaseRpcCommand> commandBuilder)
        {
            var arr = assemblies.SkipNulls().ToArray();
            if (arr.Length == 0)
                throw new ArgumentException($"", nameof(assemblies));
            return new DatabaseRpcCommandCollection(arr, (cmd)=> true, commandBuilder);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandOid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public bool TryGetCommand(Guid commandOid, out IDatabaseRpcCommand? command)
        {
            if (_commands.TryGetValue(commandOid, out var value))
            {
                command = value;
                return true;
            }
            command = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public IEnumerator<IDatabaseRpcCommand> GetEnumerator() => _commands.Values
            .OfType<IDatabaseRpcCommand>()
            .GetEnumerator();

        [DebuggerNonUserCode]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
