namespace Solitons.Samples.Database
{
    [Flags]
    public enum SampleDbUpgradeOptions
    {
        Default = 0,
        DropAllObjects = 1,
        CreateStabRecords = 256
    }
}
