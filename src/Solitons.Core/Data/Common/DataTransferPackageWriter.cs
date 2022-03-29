using System;
using System.Diagnostics;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataTransferPackageWriter : IDataTransferPackageWriter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        protected abstract void SetContentType(string contentType);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        protected abstract void SetTypeGuid(Guid guid);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        protected abstract void SetContent(byte[] content);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        protected abstract void SetCommandId(Guid commandId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected abstract void SetProperty(string key, string value);



        void IDataTransferPackageWriter.SetCommandId(Guid commandId)
        {
            SetCommandId(commandId
                .ThrowIfEmptyArgument(nameof(commandId)));
        }

        [DebuggerStepThrough]
        void IDataTransferPackageWriter.SetContentType(string contentType)
        {
            SetContentType(contentType
                .ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)));
        }

        [DebuggerStepThrough]
        void IDataTransferPackageWriter.SetTypeGuid(Guid guid)
        {
            SetTypeGuid(guid
                .ThrowIfEmptyArgument(nameof(guid)));
        }

        [DebuggerStepThrough]
        void IDataTransferPackageWriter.SetContent(byte[] content)
        {
            SetContent(
                content.ThrowIfNullArgument(nameof(content)));
        }

        [DebuggerStepThrough]
        void IDataTransferPackageWriter.SetProperty(string key, string value)
        {
            SetProperty(
                key.ThrowIfNullOrWhiteSpaceArgument(nameof(key)).Trim(),
                value.ThrowIfNullOrWhiteSpaceArgument(nameof(value)));
        }
    }
}
