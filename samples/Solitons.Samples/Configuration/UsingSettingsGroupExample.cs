namespace Solitons.Configuration;

public static class UsingSettingsGroupExample
{
    public static void Run()
    {
        var login = new UserLoginSettings()
        {
            User = "admin",
            Password = "bb307dafcbe7"
        };

        Console.WriteLine(login);
        //Output: user=admin;password=bb307dafcbe7

        login = UserLoginSettings.Parse("u=superuser;pwd=c82584fa160f");
        Console.WriteLine(login);
        //Output: user=superuser;password=c82584fa160f
    }

    public sealed class UserLoginSettings : SettingsGroup
    {
        [Setting("user", IsRequired = true, Pattern = "(?i)(username|use?r|u)")]
        public string User { get; set; } = String.Empty;

        [Setting("password", IsRequired = true, Pattern = "(?i)(password|pass|pwd|p)")]
        public string Password { get; set; } = String.Empty;

        public static UserLoginSettings Parse(string text) => Parse<UserLoginSettings>(text);
    }
}