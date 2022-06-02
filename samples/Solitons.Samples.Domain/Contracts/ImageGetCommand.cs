using System.Runtime.InteropServices;
using Solitons.Data;

namespace Solitons.Samples.Domain.Contracts
{
    [Guid("a7cccf10-406f-4707-967f-7f2112766ba4")]
    [DatabaseRpcCommand("image_get", OperationTimeout = "00:00:30")]
    public sealed class ImageGetCommand : DatabaseRpcCommand<ImageGetRequest, ImageGetResponse>
    {
        public ImageGetCommand(IDatabaseRpcProvider client, IDataContractSerializer serializer) 
            : base(client, serializer)
        {
            
        }
    }
}
