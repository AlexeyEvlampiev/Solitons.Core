using System.Reflection;

namespace Solitons.Samples.Domain
{
    public static partial class Extensions
    {
        public static IAsyncLogger WithAssemblyInfo(this IAsyncLogger self, Assembly assembly)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            return self.WithProperty(LogPropertyNames.AssemblyFullName, assembly.FullName);
        }

        public static IAsyncLogger WithEnvironmentInfo(this IAsyncLogger self)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            return self.WithProperties(new Dictionary<string, string>()
            {
                [LogPropertyNames.MachineName] = Environment.MachineName,
                [LogPropertyNames.OSVersion] = Environment.OSVersion.ToString()
            });
        }

        public static IAsyncLogger FireAndForget(this IAsyncLogger self, AppletEvent appletEvent)
        {
            self.InfoAsync(appletEvent.ToString(), log => log.WithDetails($"Applet event"));
            return self;
        }
    }
}
