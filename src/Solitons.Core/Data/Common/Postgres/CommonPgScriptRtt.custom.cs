using Solitons.Text;

namespace Solitons.Data.Common.Postgres;

public partial class CommonPgScriptRtt
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="schemaName"></param>
    public CommonPgScriptRtt(string schemaName = "public")
    {
        SchemaName = schemaName.DefaultIfNullOrWhiteSpace("public");
    }

    /// <summary>
    /// 
    /// </summary>
    public string SchemaName { get; }

    /// <summary>
    /// 
    /// </summary>
    public string EmailPattern => RegexPatterns.Email;

}