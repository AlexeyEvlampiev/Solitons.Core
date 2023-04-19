namespace Solitons.Data;

/// <summary>
/// Specifies the data transfer method for transferring objects between client and server.
/// </summary>
public enum DataTransferMethod
{
    /// <summary>
    /// The object is transferred by value.
    /// </summary>
    ByValue = 0,

    /// <summary>
    /// The object is transferred by reference.
    /// </summary>
    ByReference = 1
}