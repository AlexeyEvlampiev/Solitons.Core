namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public abstract record BasicJsonDataTransferRecord : IBasicJsonDataTransferObject
    {
        public override string ToString() => this.ToJsonString();
    }
}
