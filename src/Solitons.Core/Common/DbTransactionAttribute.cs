using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Solitons.Web;

namespace Solitons.Common
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct)]
    public abstract class DbTransactionAttribute : Attribute, IDbTransactionMetadata
    {
        protected DbTransactionAttribute(
            string guid, 
            string name, 
            string description,
            string schema,
            string procedure,
            IsolationLevel isolationLevel,
            string operationTimeout)
        {
            Guid = Guid.Parse(guid).ThrowIfEmptyArgument(nameof(guid));
            Name = name.ThrowIfNullOrWhiteSpaceArgument(nameof(name)).Trim();
            Description = description.ThrowIfNullOrWhiteSpaceArgument(nameof(description)).Trim();
            Schema = schema.ThrowIfNullOrWhiteSpaceArgument(nameof(schema)).Trim();
            Procedure = procedure.ThrowIfNullOrWhiteSpaceArgument(nameof(procedure)).Trim();
            IsolationLevel = isolationLevel;
            OperationTimeout = TimeSpan.Parse(operationTimeout);
        }

        public Guid Guid { get; }
        public string Name { get; }
        public string Description { get; }
        public string Schema { get; }
        public string Procedure { get; }
        public IsolationLevel IsolationLevel { get; }
        public TimeSpan OperationTimeout { get; }

        public T AsRestApi<T>() where T : IHttpTriggerMetadata => TargetType.GetCustomAttributes().OfType<T>().SingleOrDefault();


        public string CSharpMethod { get; init; } = null;

        public string ContentType{ get; init; } = null;

        public string Accepts { get; init; } = null;

        public Type TargetType { get; private set; }

        public override string ToString() => $"UUID={Guid}; ID={Name}";

        public static IEnumerable<DbTransactionAttribute> Get(Type targetType) =>
            targetType
                .ThrowIfNullArgument(nameof(targetType))
                .GetCustomAttributes()
                .OfType<DbTransactionAttribute>()
                .Do(att => att.TargetType = targetType);

        public static Dictionary<Type, DbTransactionAttribute[]> Discover(IEnumerable<Assembly> assemblies) =>
            Discover(assemblies
                .ThrowIfNullArgument(nameof(assemblies))
                .SelectMany(a=> a.GetTypes()));

        public static Dictionary<Type, DbTransactionAttribute[]> Discover(IEnumerable<Type> types)
        {
            var attributes =
                from t in types.ThrowIfNullArgument(nameof(types))
                from att in Get(t)
                select att;
            return attributes
                .GroupBy(att => att.GetType())
                .ToDictionary(grp => grp.Key, grp => grp.ToArray());
        }


        public IEnumerable<HttpTriggerAttribute> GetRestApiTriggers()
        {
            return HttpTriggerAttribute.Get(
                TargetType.ThrowIfNull(()=> new InvalidOperationException($"{nameof(TargetType)} is not set.")));
        }
    }
}
