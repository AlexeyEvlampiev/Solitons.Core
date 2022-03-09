using System;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDtoPackageWriter
    {
        void SetContentType(string contentType);
        void SetTypeGuid(Guid guid);
        void SetContent(byte[] content);

        void SetProperty(string key, string value);
    }
}
