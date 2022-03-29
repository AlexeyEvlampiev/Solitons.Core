using System;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataTransferPackage
    {
        /// <summary>
        /// 
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// 
        /// </summary>
        Guid TypeId { get; }

        /// <summary>
        /// 
        /// </summary>
        Guid CommandId { get; }

        /// <summary>
        /// 
        /// </summary>
        byte[] Content { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetProperty(string key, out string? value);
    }
}
