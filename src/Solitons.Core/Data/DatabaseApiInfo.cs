using System;
using System.Collections.Generic;
using System.Linq;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DatabaseApiInfo
    {
        private readonly Dictionary<Guid, DatabaseApiCommandInfo?> _memberById = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DatabaseApiInfo(IDbApiInfoSet set)
        {
            if (set == null) throw new ArgumentNullException(nameof(set));
            var commandIds = set
                .GetCommandIds()
                .Select(id=> id.ThrowIfEmpty(()=> new InvalidOperationException($"Command ID is required")))
                .ToHashSet();
            foreach (var id in commandIds)
            {
                _memberById.Add(id, new DatabaseApiCommandInfo(id, set));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public bool TryGetCommandInfo(Guid id, out DatabaseApiCommandInfo? member)
        {
            return _memberById.TryGetValue(id, out member);
        }
    }
}
