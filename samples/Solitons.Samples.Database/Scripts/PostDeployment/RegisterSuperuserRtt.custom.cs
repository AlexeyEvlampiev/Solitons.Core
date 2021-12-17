namespace Solitons.Samples.Database.Scripts.PostDeployment
{
    public partial class RegisterSuperuserRtt
    {
        private readonly IEnumerable<string> _emails;

        public RegisterSuperuserRtt(IEnumerable<string> emails)
        {
            _emails = emails.ThrowIfNullArgument(nameof(emails));
        }

        public string EmailValues => _emails.Select(e=> $"('{e}')").Join();
    }
}
