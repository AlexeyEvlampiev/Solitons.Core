using System;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataTransferPackageWriter
    {
        void SetContentType(string contentType);
        void SetTypeId(Guid guid);
        void SetContent(byte[] content);

        void SetCommandId(Guid commandId);

        void SetProperty(string key, string value);
    }
}
