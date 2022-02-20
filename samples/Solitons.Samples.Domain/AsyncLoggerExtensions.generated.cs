namespace Solitons.Samples.Domain
{
	using Solitons.Diagnostics;
	public static partial class LogPropertyNames
	{ 

		/// <summary>
        /// NetBIOS name of the hosting computer
        /// </summary>
		public const string MachineName = "MachineName";  

		/// <summary>
        /// .NET assembly full name
        /// </summary>
		public const string AssemblyFullName = "AssemblyFullName";  

		/// <summary>
        /// The current platform identifier and version number.
        /// </summary>
		public const string OSVersion = "OSVersion";  

		/// <summary>
        /// The current Applet ID.
        /// </summary>
		public const string AppletId = "AppletId";  

		/// <summary>
        /// User email claim.
        /// </summary>
		public const string UserEmail = "UserEmail";  

		/// <summary>
        /// Requested resource Uri.
        /// </summary>
		public const string RequestUri = "RequestUri";  

		/// <summary>
        /// Remote IP address.
        /// </summary>
		public const string RemoteIpAddress = "RemoteIpAddress";  

		/// <summary>
        /// Correlation ID.
        /// </summary>
		public const string CorrelationId = "CorrelationId";  		
	}

	public static partial class Extensions
	{ 

		/// <summary>
        /// NetBIOS name of the hosting computer
        /// </summary>
		public static IAsyncLogger WithMachineName(this IAsyncLogger self, string value)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            return self.WithProperty(LogPropertyNames.MachineName, value);
        }  

		/// <summary>
        /// .NET assembly full name
        /// </summary>
		public static IAsyncLogger WithAssemblyFullName(this IAsyncLogger self, string value)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            return self.WithProperty(LogPropertyNames.AssemblyFullName, value);
        }  

		/// <summary>
        /// The current platform identifier and version number.
        /// </summary>
		public static IAsyncLogger WithOSVersion(this IAsyncLogger self, string value)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            return self.WithProperty(LogPropertyNames.OSVersion, value);
        }  

		/// <summary>
        /// The current Applet ID.
        /// </summary>
		public static IAsyncLogger WithAppletId(this IAsyncLogger self, string value)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            return self.WithProperty(LogPropertyNames.AppletId, value);
        }  

		/// <summary>
        /// User email claim.
        /// </summary>
		public static IAsyncLogger WithUserEmail(this IAsyncLogger self, string value)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            return self.WithProperty(LogPropertyNames.UserEmail, value);
        }  

		/// <summary>
        /// Requested resource Uri.
        /// </summary>
		public static IAsyncLogger WithRequestUri(this IAsyncLogger self, string value)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            return self.WithProperty(LogPropertyNames.RequestUri, value);
        }  

		/// <summary>
        /// Remote IP address.
        /// </summary>
		public static IAsyncLogger WithRemoteIpAddress(this IAsyncLogger self, string value)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            return self.WithProperty(LogPropertyNames.RemoteIpAddress, value);
        }  

		/// <summary>
        /// Correlation ID.
        /// </summary>
		public static IAsyncLogger WithCorrelationId(this IAsyncLogger self, string value)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            return self.WithProperty(LogPropertyNames.CorrelationId, value);
        }  			
	}
}
