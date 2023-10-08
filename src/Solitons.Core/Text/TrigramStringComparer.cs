using System;
using System.Collections.Generic;
using System.Linq;

namespace Solitons.Text;

/// <summary>
/// Class <c>TrigramStringComparer</c> implements trigram-based string comparison.
/// </summary>
public class TrigramStringComparer 
{
    private readonly double _similarityThreshold;

    /// <summary>
    /// Initializes a new instance of the TrigramStringComparer class.
    /// </summary>
    /// <param name="similarityThreshold">A value between 0 and 1 defining the threshold for string similarity.</param>
    /// <exception cref="ArgumentException">Thrown when similarityThreshold is not between 0 and 1</exception>
    public TrigramStringComparer(double similarityThreshold = 0.3)
    {
        if (similarityThreshold < 0 || similarityThreshold > 1)
            throw new ArgumentException("Similarity threshold must be between 0 and 1");

        _similarityThreshold = similarityThreshold;
    }

    /// <summary>
    /// Generates trigrams for a given string.
    /// </summary>
    /// <param name="input">Input string for trigram generation.</param>
    /// <returns>A list of trigrams.</returns>
    public static List<string> GenerateTrigrams(string input)
    {
        var trigrams = new List<string>();

        // Generate trigrams from input
        for (int i = 0; i < input.Length - 2; i++)
            trigrams.Add(input.Substring(i, 3));

        return trigrams;
    }

    /// <summary>
    /// Calculates the similarity between two sets of trigrams.
    /// </summary>
    /// <param name="trigrams1">First set of trigrams.</param>
    /// <param name="trigrams2">Second set of trigrams.</param>
    /// <returns>The Jaccard similarity coefficient between the two sets.</returns>
    private static double CalculateSimilarity(List<string> trigrams1, List<string> trigrams2)
    {
        // Create a union of the two sets of trigrams
        var allTrigrams = new HashSet<string>(trigrams1);
        allTrigrams.UnionWith(trigrams2);

        // Calculate the number of matching trigrams
        double matches = allTrigrams.Count(trigram => trigrams1.Contains(trigram) && trigrams2.Contains(trigram));

        // Calculate the Jaccard similarity coefficient
        return matches / allTrigrams.Count;
    }

    public static double CalculateSimilarity(string s1, string s2)
    {
        // Generate trigrams for each string
        var trigrams1 = GenerateTrigrams(s1);
        var trigrams2 = GenerateTrigrams(s2);

        // Calculate the similarity between the two strings
        double similarity = CalculateSimilarity(trigrams1, trigrams2);
        return similarity;
    }


    /// <summary>
    /// Determines whether two strings are similar based on the similarity threshold.
    /// </summary>
    /// <param name="s1">The first string to compare.</param>
    /// <param name="s2">The second string to compare.</param>
    /// <returns>Returns true if the strings are similar; otherwise, false.</returns>
    public bool AreSimilar(string s1, string s2)
    {
        var similarity = CalculateSimilarity(s1, s2);
        return similarity > _similarityThreshold;
    }

    
}