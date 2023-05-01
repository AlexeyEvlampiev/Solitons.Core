namespace Solitons.Data.Common.Postgres;

public partial class LoggingPgScriptRtt
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="schemaName"></param>
    public LoggingPgScriptRtt(LoggingPgScriptPartitioningOptions options, string schemaName = "public")
    {
        Options = options;
        SchemaName = schemaName.DefaultIfNullOrWhiteSpace("public");
    }

    /// <summary>
    /// 
    /// </summary>
    public string SchemaName { get; }

    /// <summary>
    /// 
    /// </summary>
    public LoggingPgScriptPartitioningOptions Options { get; }
}