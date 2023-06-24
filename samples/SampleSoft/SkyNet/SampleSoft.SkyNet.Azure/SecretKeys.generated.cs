namespace SampleSoft.SkyNet.Azure
{
	using System.Diagnostics;

    public static class KeyVaultSecretNames
    { 
		/// <summary>
        /// The connection string for the administrator account in the SkyNet database.
        /// </summary>
		public const string SkyNetDbAdminConnectionString = "SKYNET-ADMIN-CONNECTION-STRING";  
		/// <summary>
        /// The connection string for accessing the SkyNet database via API calls.
        /// </summary>
		public const string SkyNetDbApiConnectionString = "SKYNET-API-CONNECTION-STRING";  
		/// <summary>
        /// The connection string for the Postgres server used by the SkyNet application.
        /// </summary>
		public const string SkyNetPgServerConnectionString = "SKYNET-PGSERVER-CONNECTION-STRING";  	 
    }

	public static class AppSettingsKeys
    { 
		/// <summary>
        /// The connection string for the administrator account in the SkyNet database.
        /// </summary>
		public const string SkyNetDbAdminConnectionString = "SKYNET_ADMIN_CONNECTION_STRING";  
		/// <summary>
        /// The connection string for accessing the SkyNet database via API calls.
        /// </summary>
		public const string SkyNetDbApiConnectionString = "SKYNET_API_CONNECTION_STRING";  
		/// <summary>
        /// The connection string for the Postgres server used by the SkyNet application.
        /// </summary>
		public const string SkyNetPgServerConnectionString = "SKYNET_PGSERVER_CONNECTION_STRING";     }

	public static partial class EnvironmentVariables
    { 
		/// <summary>
        /// The connection string for the administrator account in the SkyNet database.
        /// </summary>
		/// <remarks>
		/// SKYNET_ADMIN_CONNECTION_STRING
		/// </remarks>
		public static string SkyNetDbAdminConnectionString
		{
			[DebuggerNonUserCode]
			get
			{
				var key = "SKYNET_ADMIN_CONNECTION_STRING";
				var value = Environment.GetEnvironmentVariable(key);
				return value ?? throw new InvalidOperationException($"Missing environment variable. See variable {key}");
			}
		} 
		/// <summary>
        /// The connection string for accessing the SkyNet database via API calls.
        /// </summary>
		/// <remarks>
		/// SKYNET_API_CONNECTION_STRING
		/// </remarks>
		public static string SkyNetDbApiConnectionString
		{
			[DebuggerNonUserCode]
			get
			{
				var key = "SKYNET_API_CONNECTION_STRING";
				var value = Environment.GetEnvironmentVariable(key);
				return value ?? throw new InvalidOperationException($"Missing environment variable. See variable {key}");
			}
		} 
		/// <summary>
        /// The connection string for the Postgres server used by the SkyNet application.
        /// </summary>
		/// <remarks>
		/// SKYNET_PGSERVER_CONNECTION_STRING
		/// </remarks>
		public static string SkyNetPgServerConnectionString
		{
			[DebuggerNonUserCode]
			get
			{
				var key = "SKYNET_PGSERVER_CONNECTION_STRING";
				var value = Environment.GetEnvironmentVariable(key);
				return value ?? throw new InvalidOperationException($"Missing environment variable. See variable {key}");
			}
		} 	 
    }
}
