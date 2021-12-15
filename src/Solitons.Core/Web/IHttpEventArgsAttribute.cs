using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Solitons.Common;

namespace Solitons.Web
{
    public partial interface IHttpEventArgsAttribute 
    {
        Type PayloadObjectType { get; }

        Type ResponseObjectType { get; }

        bool IsUriMatch(string requestUri);
        bool IsVersionMatch(string version);
        bool IsMethodMatch(string method);
        Match MatchUri(string requestUri);
    }


    public partial interface IHttpEventArgsAttribute
    {
        internal static IEnumerable<IHttpEventArgsAttribute> Discover(IEnumerable<Type> types) => throw new NotImplementedException();

        internal static IEnumerable<IHttpEventArgsAttribute> Get(Type type) => throw new NotImplementedException();
    }
}