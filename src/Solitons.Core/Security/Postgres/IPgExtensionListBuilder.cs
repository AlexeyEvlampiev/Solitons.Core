namespace Solitons.Security.Postgres
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPgExtensionListBuilder
    {
        IPgExtensionListBuilder With(string extension, string schema = null);
    }
}
