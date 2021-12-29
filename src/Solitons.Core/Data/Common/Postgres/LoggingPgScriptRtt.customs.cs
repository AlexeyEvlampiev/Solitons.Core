namespace Solitons.Data.Common.Postgres
{
    public partial class LoggingPgScriptRtt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="schemaName"></param>
        public LoggingPgScriptRtt(string schemaName = "public")
        {
            SchemaName = schemaName.DefaultIfNullOrWhiteSpace("public");
        }

        public string SchemaName { get; }
    }
}
