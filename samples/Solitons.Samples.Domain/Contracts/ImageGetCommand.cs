using System.Runtime.InteropServices;
using Solitons.Data;

namespace Solitons.Samples.Domain.Contracts
{
    [Guid("a7cccf10-406f-4707-967f-7f2112766ba4")]
    public sealed class ImageGetCommand : DatabaseRpcCommand<ImageGetRequest, ImageGetResponse>
    {
        public ImageGetCommand(IDatabaseRpcProvider client, IDataContractSerializer serializer) 
            : base("image_get", client, serializer)
        {
            OperationTimeout = TimeSpan.FromSeconds(3);
        }
    }
}
