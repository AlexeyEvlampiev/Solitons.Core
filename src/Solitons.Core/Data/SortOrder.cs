using System.Runtime.Serialization;

namespace Solitons.Data;

/// <summary>
/// Represents sort order
/// </summary>
public enum SortOrder : byte
{
    /// <summary>
    /// Ascending order
    /// </summary>
    [EnumMember(Value = "asc")]
    Asc = 0,
    /// <summary>
    /// Descending
    /// </summary>
    [EnumMember(Value = "desc")]
    Desc = 1
}