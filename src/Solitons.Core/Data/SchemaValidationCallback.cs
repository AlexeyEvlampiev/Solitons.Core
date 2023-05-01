namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
/// <param name="content"></param>
/// <param name="comment"></param>
/// <returns></returns>
public delegate bool SchemaValidationCallback(string content, out string comment);