using System;

namespace Solitons.Web
{
    public sealed class ClaimAttribute : Attribute
    {
        public ClaimAttribute(string claimTypeName)
        {
            ClaimTypeName = claimTypeName;
        }

        public string ClaimTypeName { get; }

        public bool IsRequired { get; set; }
    }
}
