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
            Dto = ThrowIf.NullArgument(dto, nameof(dto));
            Package = ThrowIf.NullArgument(package, nameof(package));
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
