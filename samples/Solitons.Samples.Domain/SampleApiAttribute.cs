using System.ComponentModel;
using System.Runtime.InteropServices;
using Solitons.Web.Common;

namespace Solitons.Samples.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [Guid("bec9929a-0a9f-48a7-a92b-f184d5309d6e")]
    [Description("Sample API description goes here")]
    [DisplayName("Sample API")]
    public sealed class SampleApiAttribute : HttpServiceAttribute
    {
        public static readonly Version CurrentVersion = Version.Parse("1.0");

        /// <summary>
        /// 
        /// </summary>
        public SampleApiAttribute() 
            : base("sample-api", CurrentVersion)
        {
        }
    }
}
