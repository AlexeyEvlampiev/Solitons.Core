namespace Solitons.Samples.Database.Scripts.PostDeployment
{
    public partial class RegisterSuperuser
    {
        private readonly IEnumerable<string> _emails;

        public RegisterSuperuser(IEnumerable<string> emails)
        {
            _emails = emails.ThrowIfNullArgument(nameof(emails));
        }

        public string EmailValues => _emails.Select(e=> $"('{e}')").Join();
    }
}
