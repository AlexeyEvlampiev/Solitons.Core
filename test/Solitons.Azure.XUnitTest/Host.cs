using System;
using System.Diagnostics;

namespace Solitons.Azure
{
    public static class Host
    {
        private static  readonly Lazy<string> LazyStorageConnectionString;

        static Host()
        {
            LazyStorageConnectionString = new Lazy<string>(()=> Environment
                .GetEnvironmentVariable("AZ_STORAGE_CONNECTION_STRING")
                .DefaultIfNullOrWhiteSpace("UseDevelopmentStorage=true"));
        }

        public static string StorageConnectionString
        {
            [DebuggerNonUserCode]
            get=> LazyStorageConnectionString.Value;
        }
    }
}
