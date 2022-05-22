namespace Solitons.Data
{
    public readonly struct BrokeredResponse
    {
        public BrokeredResponse(object dto, DataTransferPackage package)
        {
            Dto = dto;
            Package = package;
        }

        public object Dto { get; }
        public DataTransferPackage Package { get; }
    }
}
