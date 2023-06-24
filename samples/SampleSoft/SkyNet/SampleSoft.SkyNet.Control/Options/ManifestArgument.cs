using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Solitons;
using SampleSoft.SkyNet.Azure.Manifest;

namespace SampleSoft.SkyNet.Control.Options;

/// <summary>
/// Represents an argument that parses a JSON file for the SkyNet staging environment.
/// </summary>
public sealed class ManifestArgument : Argument<ManifestData?>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ManifestArgument"/> class.
    /// </summary>
    public ManifestArgument() : base(Parse, false)
    {
        Name = "manifest";
        Arity = ArgumentArity.ExactlyOne;
        HelpName = "Sky Net Staging Environment Manifest";
        Description = "JSON file for the SkyNet staging environment that contains configuration details.";
    }

    private static ManifestData? Parse(ArgumentResult result)
    {
        var path = result.Tokens.FirstOrDefault()?.Value;
        if (result.Tokens.Count != 1 ||
            File.Exists(path) == false)
        {
            result.ErrorMessage = "Please provide a valid file path. The provided file path either doesn't exist or is inaccessible.";
            return null;
        }

        try
        {
            var json = File
                .ReadAllText(path)
                .Replace("%USERNAME%", Environment.UserName);
            var manifest = JsonSerializer.Deserialize<ManifestData>(json) 
                           ?? throw new FormatException("Failed to parse JSON.");

            var errors = new HashSet<string>();
            manifest.Assert(error => errors.Add(error));
            if (errors.Count == 0)
            {
                return manifest;
            }

            throw new ValidationException($"Errors detected in manifest: {string.Join("; ", errors)}");
        }
        catch (Exception e)
        {
            result.ErrorMessage = $"Failed to parse manifest due to an error: {e.Message}.";
            return null;
        }
    }
}