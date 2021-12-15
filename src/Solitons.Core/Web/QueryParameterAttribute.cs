using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Solitons.Web
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class QueryParameterAttribute : Attribute
    {
        private readonly Regex _regex;
        public QueryParameterAttribute(string name, string parameterNamePattern)
        {
            ParameterName = name;
            _regex = new Regex($"[?&](?<key>(?:(?i){parameterNamePattern}))=(?-i)(?<value>[^&]+)");
        }


        public string ParameterName { get;  }

        public bool IsRequired { get; init; }

        public bool TryGetValue(string url, out string value)
        {
            var match = _regex.Match(url);
            if (match.Success)
            {
                value = match.Groups["value"].Value;
                return true;
            }
            value = null;
            return false;
        }

    }
}
