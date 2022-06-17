namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public readonly record struct BrokeredResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="package"></param>
        public BrokeredResponse(object dto, DataTransferPackage package)
        {
            Dto = dto.ThrowIfNullArgument(nameof(dto));
            Package = package.ThrowIfNullArgument(nameof(package));
        }

        /// <summary>
        /// 
        /// </summary>
        public object Dto { get; }

        /// <summary>
        /// 
        /// </summary>
        public DataTransferPackage Package { get; }
    }
}
