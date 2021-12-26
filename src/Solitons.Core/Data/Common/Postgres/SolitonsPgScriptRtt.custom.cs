using Solitons.Text;

namespace Solitons.Data.Common.Postgres
{
    public partial class SolitonsPgScriptRtt
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="schemaName"></param>
        public SolitonsPgScriptRtt(SolitonsPgScriptRttOptions options, string schemaName = "public")
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
        public string EmailPattern => RegexPatterns.Email;

        /// <summary>
        /// 
        /// </summary>
        public SolitonsPgScriptRttOptions Options { get; }
    }
}
