using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DataContractSerializerBuilder
    {
        private readonly DataContractSerializerBehaviour _behaviour;
        private readonly HashSet<Registration> _cache = new();
        private readonly List<Registration> _registrations = new();
        sealed record Registration(Type DtoType, IMediaTypeSerializer Serializer);

        sealed class Serializer : DataContractSerializer
        {
            public Serializer(DataContractSerializerBehaviour behaviour, IEnumerable<Registration> registrations) 
                : base(behaviour)
            {
                foreach (var registration in registrations)
                {
                    Register(registration.DtoType, registration.Serializer);
                }
            }
        }

        [DebuggerNonUserCode]
        private DataContractSerializerBuilder(DataContractSerializerBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static DataContractSerializerBuilder Create(DataContractSerializerBehaviour behaviour) => new DataContractSerializerBuilder(behaviour);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public DataContractSerializerBuilder With(Type type, IMediaTypeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            var registration = new Registration(type, serializer);
            if (_cache.Add(registration))
            {
                _registrations.Add(registration);
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataContractSerializer Build() => new Serializer(_behaviour, _registrations);

    }
}
