namespace Solitons;

/// <summary>
/// Quote type
/// </summary>
public enum QuoteType
{
    /// <summary>
    /// Basic double quote
    /// </summary>
    Double = 0,

    /// <summary>
    /// Basic single quote
    /// </summary>
    Single = 1,

    /// <summary>
    /// SQL literal (single) quote 
    /// </summary>
    SqlLiteral,

    /// <summary>
    /// SQL identity (double) quote
    /// </summary>
    SqlIdentity
}