using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DatabaseApiInfo : IDatabaseApiInfo
    {
        private readonly Dictionary<Guid, IDatabaseApiCommandInfo?> _commandsById = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DatabaseApiInfo(IDbApiInfoSet set)
        {
            if (set == null) throw new ArgumentNullException(nameof(set));
            ETag = set.ETag;
            var commandIds = set
                .GetCommandIds()
                .Select(id=> id.ThrowIfEmpty(()=> new InvalidOperationException($"Command ID is required")))
                .ToHashSet();
            foreach (var id in commandIds)
            {
                _commandsById.Add(id, new DatabaseApiCommandInfo(id, set));
            }
        }

        public string ETag { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        bool IDatabaseApiInfo.TryGetCommandInfo(Guid id, out IDatabaseApiCommandInfo? command)
        {
            return _commandsById.TryGetValue(id, out command);
        }
    }
}
