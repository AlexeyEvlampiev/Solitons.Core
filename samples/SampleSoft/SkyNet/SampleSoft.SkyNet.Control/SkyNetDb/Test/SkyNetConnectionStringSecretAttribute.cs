namespace SampleSoft.SkyNet.Control.SkyNetDb.Test;

[AttributeUsage(AttributeTargets.Method)]
public sealed class SkyNetConnectionStringSecretAttribute : Attribute
{
    public SkyNetConnectionStringSecretAttribute(string secretName)
    {
        SecretName = secretName;
    }

    public string SecretName { get; }
}