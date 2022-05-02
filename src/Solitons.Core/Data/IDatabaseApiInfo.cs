using System;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public interface IDatabaseApiInfo
{
    /// <summary>
    /// 
    /// </summary>
    string ETag { get; }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    bool TryGetCommandInfo(Guid id, out IDatabaseApiCommandInfo? command);
}