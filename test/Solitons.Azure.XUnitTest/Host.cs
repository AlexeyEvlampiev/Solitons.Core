using System;
using System.Diagnostics;

namespace Solitons.Azure
{
    public static class Host
    {
        private const string StorageConnectionStringKey = "AZ_STORAGE_CONNECTION_STRING";
        private static  readonly Lazy<string> LazyStorageConnectionString;

        static Host()
        {
            LazyStorageConnectionString = new Lazy<string>(()=> Environment
                .GetEnvironmentVariable(StorageConnectionStringKey)
                .ThrowIfNullOrWhiteSpace(()=> new InvalidOperationException($"{StorageConnectionStringKey} environment variable is missing.")));
        }

        public static string StorageConnectionString
        {
            [DebuggerNonUserCode]
            get=> LazyStorageConnectionString.Value;
        }
    }
}
