using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Solitons.Data
{
    public partial interface IDatabaseExternalTriggerArgsAttribute
    {
        string Procedure { get; }

        IsolationLevel IsolationLevel { get; }

        TimeSpan Timeout { get; }

        string ProcedureEventArgsContentType { get; }

        string ProcedurePayloadContentType { get; }

        Type PayloadObjectType { get; }

        string Authorize { get; }
    }

    public partial interface IDatabaseExternalTriggerArgsAttribute
    {
        internal static Dictionary<IDatabaseExternalTriggerArgsAttribute, Type> Discover(IEnumerable<Type> types)
        {
            var pairs =
                from type in types
                from attribute in type
                    .GetCustomAttributes(false)
                    .OfType<IDatabaseExternalTriggerArgsAttribute>()
                select KeyValuePair.Create(attribute, type);
            return pairs.ToDictionary();
        }
    }
}
