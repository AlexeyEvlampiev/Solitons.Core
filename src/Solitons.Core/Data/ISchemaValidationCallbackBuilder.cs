namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public interface ISchemaValidationCallbackBuilder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="contentType"></param>
    /// <param name="schema"></param>
    /// <returns></returns>
    SchemaValidationCallback Build(string contentType, string schema);
}