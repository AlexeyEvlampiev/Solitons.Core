using System;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHttpServiceAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 
        /// </summary>
        Guid Guid { get; }


        /// <summary>
        /// 
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 
        /// </summary>
        Version CurrentVersion { get; }

    }
}
