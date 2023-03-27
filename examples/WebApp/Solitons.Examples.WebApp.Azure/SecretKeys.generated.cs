namespace Solitons.Examples.WebApp.Azure;

/// <summary>
/// Provides constant values for secret keys used by the Solitons web application.
/// </summary>
public static class SecretKeys
{
	/// <summary>
    ///  WebApp postgres server maintenance database connection string.
    /// </summary>
    /// <value>The connection string for the maintenance database used by the Solitons web application.</value>
	public const string MaintenanceDbConnectionString = "SOLITONS-WEBAPP-MAINTENANCE-DB-CONNECTION-STRING"; 
	/// <summary>
    ///  WebApp database admin connection string.
    /// </summary>
    /// <value>The connection string for the maintenance database used by the Solitons web application.</value>
	public const string DatabaseAdminConnectionString = "SOLITONS-WEBAPPDB-ADMIN-CONNECTION-STRING"; 
	/// <summary>
    ///  WebApp database application connection string.
    /// </summary>
    /// <value>The connection string for the maintenance database used by the Solitons web application.</value>
	public const string DatabaseAppConnectionString = "SOLITONS-WEBAPPDB-APP-CONNECTION-STRING";  
}

/// <summary>
/// Provides access to environment variables used by the Solitons web application.
/// </summary>
public sealed class WebAppEnvVariables
{	
	/// <summary>
    /// Initializes a new instance of the <see cref="WebAppEnvVariables"/> class.
    /// </summary>
    /// <param name="target">The target of the environment variables.</param>
	public WebAppEnvVariables(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
	{
		EnvironmentVariableTarget = target;
	}

	/// <summary>
    /// Gets the target of the environment variables.
    /// </summary>
    /// <value>The target of the environment variables.</value>
	public EnvironmentVariableTarget EnvironmentVariableTarget { get; }

	/// <summary>
    ///  Gets the SOLITONS-WEBAPP-MAINTENANCE-DB-CONNECTION-STRING entironment variable. 
    /// </summary>
	/// <remarks>
	/// WebApp postgres server maintenance database connection string.
	/// </remarks>
    /// <value>The connection string for the maintenance database used by the Solitons web application.</value>
	public string MaintenanceDbConnectionString => Environment.GetEnvironmentVariable("SOLITONS-WEBAPP-MAINTENANCE-DB-CONNECTION-STRING", this.EnvironmentVariableTarget);

	/// <summary>
    ///  Gets the SOLITONS-WEBAPPDB-ADMIN-CONNECTION-STRING entironment variable. 
    /// </summary>
	/// <remarks>
	/// WebApp database admin connection string.
	/// </remarks>
    /// <value>The connection string for the maintenance database used by the Solitons web application.</value>
	public string DatabaseAdminConnectionString => Environment.GetEnvironmentVariable("SOLITONS-WEBAPPDB-ADMIN-CONNECTION-STRING", this.EnvironmentVariableTarget);

	/// <summary>
    ///  Gets the SOLITONS-WEBAPPDB-APP-CONNECTION-STRING entironment variable. 
    /// </summary>
	/// <remarks>
	/// WebApp database application connection string.
	/// </remarks>
    /// <value>The connection string for the maintenance database used by the Solitons web application.</value>
	public string DatabaseAppConnectionString => Environment.GetEnvironmentVariable("SOLITONS-WEBAPPDB-APP-CONNECTION-STRING", this.EnvironmentVariableTarget);
 	
}

