using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons.Data.Management.Postgres.Common;

/// <summary>
/// Defines a comparer for <see cref="Script"/> objects, where the comparison logic can be customized per category.
/// </summary>
public abstract class ScriptPriorityComparer : Comparer<Script>
{
    /// <summary>
    /// Compares two scripts based on their category priority order and then their path.
    /// </summary>
    /// <param name="script1">The first script to compare.</param>
    /// <param name="script2">The second script to compare.</param>
    /// <returns>A value that indicates the relative order of the objects being compared. 
    /// The return value has these meanings: Less than zero: script1 precedes script2. 
    /// Zero: script1 equals script2. Greater than zero: script1 follows script2.
    /// </returns>
    public sealed override int Compare(Script? script1, Script? script2)
    {
        script1 = ThrowIf.ArgumentNull(script1);
        script2 = ThrowIf.ArgumentNull(script2);


        var category1 = GetScriptCategoryPriorityOrder(script1);
        var category2 = GetScriptCategoryPriorityOrder(script2);
        if (category1 != category2)
        {
            return category1.CompareTo(category2);
        }

        var category = category1;
        return Compare(category, script1, script2);
    }

    /// <summary>
    /// Defines the priority order for each category of scripts. By convention, migration scripts are in category 0 and setup scripts are in category 1.
    /// This method can be overridden in derived classes to define additional categories.
    /// </summary>
    /// <param name="script">The script for which to determine the category priority order.</param>
    /// <returns>The priority order of the script's category. Lower values have higher priority.</returns>
    [DebuggerStepThrough]
    protected virtual int GetScriptCategoryPriorityOrder(Script script) => 0;

    /// <summary>
    /// Compares two scripts within the same category. By default, scripts are compared based on their path using ordinal string comparison.
    /// This method can be overridden in derived classes to implement custom comparison logic.
    /// </summary>
    /// <param name="category">The category of the scripts.</param>
    /// <param name="script1">The first script to compare.</param>
    /// <param name="script2">The second script to compare.</param>
    /// <returns>A value that indicates the relative order of the scripts within the category.</returns>
    protected virtual int Compare(int category, Script script1, Script script2) => StringComparer.Ordinal
        .Compare(script1.Path, script2.Path);
}