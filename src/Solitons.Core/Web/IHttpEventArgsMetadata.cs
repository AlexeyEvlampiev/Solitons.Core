using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Solitons.Common;

namespace Solitons.Web
{
    public interface IHttpEventArgsMetadata 
    {
        bool IsUriMatch(string requestUri);
        bool IsVersionMatch(string version);
        bool IsMethodMatch(string method);
        Match MatchUri(string requestUri);
    }
}