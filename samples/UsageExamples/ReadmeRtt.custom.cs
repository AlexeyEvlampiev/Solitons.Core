using System.Reflection;
using Solitons.Collections;

namespace UsageExamples;

public partial class ReadmeRtt
{
    sealed record Member(Type MemberType, ExampleAttribute Example);

    private readonly Member[] _members;

    public ReadmeRtt()
    {
        _members = GetType().Assembly
            .GetTypes()
            .Where(type => Attribute.IsDefined(type, typeof(ExampleAttribute)))
            .Select(type => new Member(type, type.GetCustomAttribute<ExampleAttribute>()!))
            .ToArray();

    }

    public IEnumerable<string> Namespaces => FluentArray
        .Create(
            "Solitons",
            "Solitons.Data",
            "Solitons.Diagnostics")
        .OrderBy(_ => _, StringComparer.Ordinal);

    private IEnumerable<Member> GetNamespaceMembers(string ns)
    {
        return _members.Where(m => m.MemberType.Namespace!.Equals(ns, StringComparison.Ordinal));
    }
}