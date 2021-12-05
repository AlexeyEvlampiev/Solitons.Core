using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Solitons.Common;

namespace Solitons.Web
{
    public interface IHttpTriggerMetadata 
    {
        Guid TriggerId { get; }
        string Name { get; }
        string Description { get; }
        string VersionRegexp { get; }
        string UriRegexp { get; }
        string CSharpClientName { get; }

        /// <summary>
        /// Specifies the C# method name to be provided by the api client.
        /// </summary>
        string CSharpMethod { get; init; }

        Type TargetType { get; }
        bool IsUriMatch(string requestUri);
        bool IsVersionMatch(string version);
        bool IsMethodMatch(string method);
        Match MatchUri(string requestUri);
        IEnumerable<DbTransactionAttribute> GetDbTransactions();
    }
}