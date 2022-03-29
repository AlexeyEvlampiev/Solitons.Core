using System;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommandArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid CommandId => GetType().GUID;
    }
}
