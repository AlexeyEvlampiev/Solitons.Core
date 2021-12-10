using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Solitons.Common;
using Solitons.Text;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct, Inherited = true, AllowMultiple = true)]
    public abstract class HttpTriggerAttribute : Attribute, IHttpEventArgsMetadata
    {
        private readonly Regex _uriRegex;
        private readonly Regex _versionRegex;
        private readonly Regex _methodRegex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="clientCSharpClass"></param>
        /// <param name="methodRegexp"></param>
        /// <param name="versionRegexp"></param>
        /// <param name="uriRegexp"></param>
        protected HttpTriggerAttribute(
            string guid, 
            string name, 
            string description,
            string clientCSharpClass,
            string methodRegexp,
            string versionRegexp,
            string uriRegexp)
        {
            TriggerId = Guid.Parse(guid).ThrowIfEmptyArgument(nameof(guid));
            Name = name.ThrowIfNullOrWhiteSpaceArgument(nameof(name)).Trim();
            Description = description.ThrowIfNullOrWhiteSpaceArgument(nameof(description)).Trim();
            CSharpClientName = clientCSharpClass.ThrowIfNullOrWhiteSpaceArgument(nameof(clientCSharpClass)).Trim();
            MethodRegexp = methodRegexp.ThrowIfNullOrWhiteSpaceArgument(nameof(methodRegexp));
            VersionRegexp = versionRegexp.ThrowIfNullOrWhiteSpaceArgument(nameof(versionRegexp));
            uriRegexp = uriRegexp
                .ThrowIfNullOrWhiteSpaceArgument(nameof(uriRegexp))
                .Replace(new Regex(@"rgx:(\w+)"), match =>
                {
                    return match.Groups[1].Value.ToLower() switch
                    {
                        "uuid"=> RegexPatterns.Uuid.LooseWithoutBrakets,
                        "guid" => RegexPatterns.Uuid.LooseWithoutBrakets,
                        _ => throw new NotSupportedException(match.Value)
                    };
                });
            UriRegexp = uriRegexp;
            _methodRegex = new Regex(methodRegexp);
            _uriRegex = new Regex(UriRegexp);
            _versionRegex = new Regex(VersionRegexp);
        }

        public Guid TriggerId { get; }
        public string Name { get; }
        public string Description { get; }

        public string VersionRegexp { get; }
        public string UriRegexp { get; }
        public string CSharpClientName { get; }

        /// <summary>
        /// Specifies the C# method name to be provided by the api client.
        /// </summary>
        public string CSharpMethod { get; init; }

        public Type TargetType { get; private set; }
        public string MethodRegexp { get; }


        [DebuggerStepThrough]
        public static IEnumerable<HttpTriggerAttribute> Discover(Assembly assemblies) =>
            Discover(assemblies?.ToEnumerable());

        [DebuggerStepThrough]
        public static IEnumerable<HttpTriggerAttribute> Discover(params Assembly[] assemblies) =>
            Discover(assemblies?.AsEnumerable());

        public static IEnumerable<HttpTriggerAttribute> Discover(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            var result =
                from a in assemblies
                from t in a.GetTypes()
                from att in Get(t)
                select att;
            return result
                .Do(_ =>
                {
                    //TODO: Implement validation logic here
                });
        }

        public static IEnumerable<IHttpEventArgsMetadata> Discover(IEnumerable<Type> types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));
            var result =
                from t in types
                from att in Get(t)
                select att;
            return result
                .Do(_ =>
                {
                    //TODO: Implement validation logic here
                });
        }

        

        public override string ToString() => $"Trigger={TriggerId}; ID={Name}";


        public static IEnumerable<HttpTriggerAttribute> Get(Type targetType)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));
            return targetType
                .GetCustomAttributes()
                .OfType<HttpTriggerAttribute>()
                .Do(_=> _.TargetType = targetType);

        }

        [DebuggerNonUserCode]
        public bool IsUriMatch(string requestUri) => _uriRegex.IsMatch(requestUri ?? string.Empty);

        [DebuggerNonUserCode]
        public bool IsVersionMatch(string version) => _versionRegex.IsMatch(version ?? string.Empty);

        [DebuggerNonUserCode]
        public bool IsMethodMatch(string method) => _methodRegex.IsMatch(method ?? string.Empty);


        [DebuggerNonUserCode]
        public Match MatchUri(string requestUri) => _uriRegex.Match(requestUri ?? string.Empty);

        public IEnumerable<DbTransactionAttribute> GetDbTransactions()
        {
            return DbTransactionAttribute.Get(
                TargetType.ThrowIfNull(() => new InvalidOperationException($"{nameof(TargetType)} is not set.")));
        }
    }
}
