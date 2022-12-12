using System.Diagnostics;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public abstract record BasicJsonDataTransferRecord : IBasicJsonDataTransferObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => this.ToJsonString();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        protected static T Parse<T>(string text) where T : BasicJsonDataTransferObject
        {
            return ThrowIf
                .NullOrWhiteSpaceArgument(text, nameof(text))
                .Convert(IBasicJsonDataTransferObject.Parse<T>);
        }
    }
}
