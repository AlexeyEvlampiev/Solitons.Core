namespace Solitons
{
    /// <summary>
    /// Types of data transfer
    /// </summary>
    /// <remarks>
    /// Specifies the intended data transfer method.
    /// </remarks>
    /// <seealso cref="ISpecializedSerializer"/>
    /// <seealso cref="ISpecializedTransientStorageReceipt"/>
    public enum DataTransferMethod
    {
        /// <summary>
        /// By value
        /// </summary>
        ByValue = 0,

        /// <summary>
        /// By reference
        /// </summary>
        ByReference = 1
    }
}
