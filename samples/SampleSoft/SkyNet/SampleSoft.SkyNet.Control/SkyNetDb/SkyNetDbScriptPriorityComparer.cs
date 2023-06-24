using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Solitons;
using Solitons.Collections.Specialized;
using Solitons.Data.Management;

namespace SampleSoft.SkyNet.Control.SkyNetDb;

public sealed class SkyNetDbScriptPriorityComparer : Comparer<AssemblyEmbeddedScript>
{
    private static readonly Regex MigrationScriptIdRegex = new Regex(@"(?i)\.skynetdb\.migrations?\.");
    private static readonly Regex SetupScriptIdRegex = new Regex(@"(?i)\.skynetdb\.setups?\.");
    private readonly RegexCollection _setupScriptExpressions;
    public SkyNetDbScriptPriorityComparer()
    {
        _setupScriptExpressions = RegexCollection
            .FromPatterns(
                @"reference-data",
                @"\bsystem\b",
                @"\bdata\.reference-data\b",
                @"\bdata\b",
                @"\bapi\.http_invoke\b",
                @"\bapi\b");
    }


    public static bool IsMigrationScript(Script script)
    {
        if (script is AssemblyEmbeddedScript embeddedScript)
        {
            return MigrationScriptIdRegex.IsMatch(embeddedScript.Path);
        }
        throw new NotSupportedException();
    }

    public override int Compare(AssemblyEmbeddedScript? a, AssemblyEmbeddedScript? b)
    {
        a = ThrowIf.ArgumentNull(a);
        b = ThrowIf.ArgumentNull(b);
        var scriptA = a as AssemblyEmbeddedScript ?? throw new ArgumentException();
        var scriptB = b as AssemblyEmbeddedScript ?? throw new ArgumentException();

        var categoryA = GetCategory(scriptA);
        var categoryB = GetCategory(scriptB);
        if (categoryA != categoryB)
        {
            return categoryA.CompareTo(categoryB);
        }

        if (SetupScriptIdRegex.IsMatch(scriptA.Path))
        {
            Debug.Assert(SetupScriptIdRegex.IsMatch(scriptB.Path));
            var indexOfMatchA = _setupScriptExpressions.IndexOfFirstMatch(scriptA.Path, int.MaxValue);
            var indexOfMatchB = _setupScriptExpressions.IndexOfFirstMatch(scriptB.Path, int.MaxValue);
            var comparison = indexOfMatchA.CompareTo(indexOfMatchB);
            if (comparison != 0)
            {
                return comparison;
            }
        }

        return StringComparer.Ordinal.Compare(scriptA.Path, scriptB.Path);
    }

    private int GetCategory(AssemblyEmbeddedScript script)
    {
        if (MigrationScriptIdRegex.IsMatch(script.Path))
        {
            return 0;
        }
        else if(SetupScriptIdRegex.IsMatch(script.Path))
        {
            return 1;
        }
        else
        {
            throw new NotSupportedException();
        }
    }

}