using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Solitons.Data.Management;

namespace SampleSoft.SkyNet.Control.SkyNetDb;

public sealed record SkyNetDbEmbeddedScript : AssemblyEmbeddedScript
{
    private static readonly Regex ScriptRegex = new Regex(@"(?i)^.+?\.skynetdb\.(?:@category?\.)?"
        .Replace("@category", @"(?:migrations?|setups?)"));

    public SkyNetDbEmbeddedScript(string resourceName, Assembly assembly) 
        : base(ScriptRegex.Replace(resourceName, String.Empty), resourceName, assembly)
    {
    }

    [DebuggerStepThrough]
    public static SkyNetDbEmbeddedScript Create(string resourceName, Assembly assembly) =>
        new SkyNetDbEmbeddedScript(resourceName, assembly);

    public static bool IsScript(string path)
    {
        return path.EndsWith(".sql", StringComparison.OrdinalIgnoreCase) &&
               ScriptRegex.IsMatch(path);
    }
}