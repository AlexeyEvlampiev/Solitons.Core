using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SampleSoft.SkyNet.Control;

public sealed class CommandLineTransformer
{
    public string Transform(string command)
    {
        command = Regex.Replace(command, @"(?i)^.+?\.(?:exe|dll)\s+", string.Empty);

        command = Regex.Replace(
            command, 
            @"\b(skynetdb|db|database) (create) (using|for|from|in)\b"
                .Replace(" ", @"\s+"), 
            "skynetdb create $using");


        return command;
    }
}