namespace Solitons.Configuration;

public static class UsingSettingsGroupExample
{
    // The Run() method is a demonstration of how to use the UserLoginSettings class.
    public static void Run()
    {
        // Create a new UserLoginSettings object and set its properties.
        var login = new UserLoginSettings()
        {
            User = "admin",
            Password = "bb307dafcbe7"
        };

        // Serialize the login object to a plain-text semicolon-separated format using the ToString() method and display the output.
        Console.WriteLine(login);
        // Output: user=admin;password=bb307dafcbe7

        // Deserialize a plain-text semicolon-separated string that contains different User and Password values.
        login = UserLoginSettings.Parse("u=superuser;pwd=c82584fa160f");
        // Serialize the deserialized login object to a plain-text semicolon-separated format using the ToString() method and display the output.
        Console.WriteLine(login);
        // Output: user=superuser;password=c82584fa160f
    }

    // The UserLoginSettings class defines a specific type of settings group that inherits from SettingsGroup.
    public sealed class UserLoginSettings : SettingsGroup
    {
        // The User property is a required setting with a specific pattern to match its name.
        [Setting("user", IsRequired = true, Pattern = "(?i)(username|use?r|u)")]
        public string User { get; set; } = String.Empty;

        // The Password property is a required setting with a specific pattern to match its name.
        [Setting("password", IsRequired = true, Pattern = "(?i)(password|pass|pwd|p)")]
        public string Password { get; set; } = String.Empty;

        // The Parse method uses the Parse<T> method of the SettingsGroup class to deserialize a plain-text semicolon-separated string to a UserLoginSettings object.
        public static UserLoginSettings Parse(string text) => Parse<UserLoginSettings>(text);
    }
}
