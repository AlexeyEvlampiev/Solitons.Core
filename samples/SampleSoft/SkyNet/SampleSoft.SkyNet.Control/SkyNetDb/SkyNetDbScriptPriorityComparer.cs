using System.Diagnostics;
using System.Text.RegularExpressions;
using Solitons;
using Solitons.Collections.Specialized;
using Solitons.Data.Management;
using Solitons.Data.Management.Postgres.Common;

namespace SampleSoft.SkyNet.Control.SkyNetDb;

/// <summary>
/// A script comparer that assigns priority to scripts in the SkyNetDB context.
/// This comparer is specifically designed to work with Migration and Setup scripts in a SkyNetDB.
/// </summary>
public sealed class SkyNetDbScriptPriorityComparer : ScriptPriorityComparer
{
    private static readonly Regex MigrationScriptIdRegex = new Regex(@"(?i)\.skynetdb\.migrations?\.");
    private static readonly Regex SetupScriptIdRegex = new Regex(@"(?i)\.skynetdb\.setups?\.");
    private readonly RegexCollection _setupScriptExpressions;

    /// <summary>
    /// Initializes a new instance of the <see cref="SkyNetDbScriptPriorityComparer"/> class.
    /// </summary>
    public SkyNetDbScriptPriorityComparer()
    {
        _setupScriptExpressions = RegexCollection
            .FromPatterns(
                @"\bpermissions\b",
                @"\breference-data\b",
                @"\bsystem\b",
                @"\bdata\b",
                @"\bapi\.http_invoke\b",
                @"\bapi\.vw\b",
                @"\bapi\b");
    }

    /// <summary>
    /// Determines whether a script is a migration script.
    /// </summary>
    /// <param name="script">The script to check.</param>
    /// <returns>
    /// <see langword="true"/> if the script is a migration script; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsMigrationScript(Script script)
    {
        if (script is AssemblyEmbeddedScript embeddedScript)
        {
            return MigrationScriptIdRegex.IsMatch(embeddedScript.Path);
        }
        throw new NotSupportedException();
    }

    /// <summary>
    /// Compares two scripts based on their category and the setup script expressions.
    /// </summary>
    /// <param name="category">The category of the scripts.</param>
    /// <param name="script1">The first script to compare.</param>
    /// <param name="script2">The second script to compare.</param>
    /// <returns>A value that indicates the relative order of the scripts within the category.</returns>
    protected override int Compare(int category, Script script1, Script script2)
    {
        Debug.Assert(GetScriptCategoryPriorityOrder(script1) == GetScriptCategoryPriorityOrder(script2));
        if (SetupScriptIdRegex.IsMatch(script1.Path))
        {
            Debug.Assert(SetupScriptIdRegex.IsMatch(script2.Path));
            Debug.Assert(category == 1);
            var indexOfMatchA = _setupScriptExpressions.IndexOfFirstMatch(script1.Path, int.MaxValue);
            var indexOfMatchB = _setupScriptExpressions.IndexOfFirstMatch(script2.Path, int.MaxValue);
            var comparison = indexOfMatchA.CompareTo(indexOfMatchB);
            if (comparison != 0)
            {
                return comparison;
            }
        }
        else
        {
            Debug.Assert(IsMigrationScript(script1));
            Debug.Assert(IsMigrationScript(script2));
            Debug.Assert(category == 0);
        }

        
        return StringComparer.Ordinal.Compare(script1.Path, script2.Path);
    }

    /// <summary>
    /// Determines the priority order for the category of a given script.
    /// </summary>
    /// <param name="script">The script to categorize.</param>
    /// <returns>The priority order of the script's category, where lower values indicate higher priority.</returns>
    protected override int GetScriptCategoryPriorityOrder(Script script)
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