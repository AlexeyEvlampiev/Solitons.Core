using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public sealed record LogEventArgs(
        LogLevel Level,
        string Message,
        IPrincipal? Principal,
        CallerInfo SourceInfo,
        string Content);
}
