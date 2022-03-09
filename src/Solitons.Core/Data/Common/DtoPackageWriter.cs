using System;
using System.Diagnostics;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DtoPackageWriter : IDtoPackageWriter
    {
        protected abstract void SetContentType(string contentType);
        protected abstract void SetTypeGuid(Guid guid);
        protected abstract void SetContent(byte[] content);
        protected abstract void SetProperty(string key, string value);

        [DebuggerStepThrough]
        void IDtoPackageWriter.SetContentType(string contentType)
        {
            SetContentType(contentType
                .ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)));
        }

        [DebuggerStepThrough]
        void IDtoPackageWriter.SetTypeGuid(Guid guid)
        {
            SetTypeGuid(guid
                .ThrowIfEmptyArgument(nameof(guid)));
        }

        [DebuggerStepThrough]
        void IDtoPackageWriter.SetContent(byte[] content)
        {
            SetContent(
                content.ThrowIfNullArgument(nameof(content)));
        }

        [DebuggerStepThrough]
        void IDtoPackageWriter.SetProperty(string key, string value)
        {
            SetProperty(
                key.ThrowIfNullOrWhiteSpaceArgument(nameof(key)).Trim(),
                value.ThrowIfNullOrWhiteSpaceArgument(nameof(value)));
        }
    }
}
