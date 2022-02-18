namespace Solitons.Security.Cryptography
{
    public interface ISasValidator
    {
        bool IsValid(string sas, out string? data);
    }
}
