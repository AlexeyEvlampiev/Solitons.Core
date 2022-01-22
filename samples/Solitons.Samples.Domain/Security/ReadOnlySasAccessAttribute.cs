namespace Solitons.Samples.Domain.Security
{
    public class ReadOnlySasAccessAttribute : Attribute
    {

        public ReadOnlySasAccessAttribute(string expiredAfter)
        {
            ExpiresAfter = TimeSpan.Parse(expiredAfter);
        }

        public TimeSpan ExpiresAfter { get; set; }
    }
}
