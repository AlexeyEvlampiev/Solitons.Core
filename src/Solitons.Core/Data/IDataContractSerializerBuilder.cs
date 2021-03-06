using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IDataContractSerializerBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="switch"></param>
        /// <returns></returns>
        IDataContractSerializerBuilder RequireCustomGuidAnnotation(bool @switch);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoType"></param>
        /// <param name="mediaTypeSerializer"></param>
        /// <returns></returns>
        IDataContractSerializerBuilder Add(Type dtoType, IMediaTypeSerializer mediaTypeSerializer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="mediaTypeSelector"></param>
        /// <returns></returns>
        IDataContractSerializerBuilder AddAssemblyTypes(IEnumerable<Assembly> assemblies, Func<Type, IEnumerable<IMediaTypeSerializer>>? mediaTypeSelector = null);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDataContractSerializer Build();

        
    }

    public partial interface IDataContractSerializerBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoType"></param>
        /// <param name="mediaTypeSerializers"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        IDataContractSerializerBuilder Add(Type dtoType, params IMediaTypeSerializer[] mediaTypeSerializers)
        {
            if (dtoType == null) throw new ArgumentNullException(nameof(dtoType));
            if (mediaTypeSerializers == null) throw new ArgumentNullException(nameof(mediaTypeSerializers));
            Array.ForEach(mediaTypeSerializers, mts=> Add(dtoType, mts));
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="mediaTypeSelector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public IDataContractSerializerBuilder AddAssemblyTypes(Assembly assembly, Func<Type, IEnumerable<IMediaTypeSerializer>>? mediaTypeSelector = null)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            return AddAssemblyTypes(new Assembly[] { assembly }, mediaTypeSelector);
        }

    }
}
