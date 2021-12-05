using System;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public sealed record TransientStorageReceipt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="contentType"></param>
        /// <param name="body"></param>
        /// <param name="expiresOn"></param>
        public TransientStorageReceipt(DataTransferMethod method, string contentType, byte[] body, DateTimeOffset expiresOn)
        {
            DataTransferMethod = method;
            ContentType = contentType;
            Body = body;
            ExpiresOn = expiresOn;
        }

        public DataTransferMethod DataTransferMethod { get; }
        public string ContentType { get; }
        public byte[] Body { get; }
        public DateTimeOffset ExpiresOn { get; }
    }
    
}
