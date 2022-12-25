using System;
using System.Diagnostics;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataContractSerializerBase : DataContractSerializerProxy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        [DebuggerStepThrough]
        protected DataContractSerializerBase(Action<IDataContractSerializerBuilder> config) 
            : base(IDataContractSerializer.Build(config))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => GetType()?.FullName ?? base.ToString() ?? nameof(DataContractSerializerBase);
    }
}
