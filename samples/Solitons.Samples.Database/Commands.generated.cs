namespace Sample.DbUp
{
    using McMaster.Extensions.CommandLineUtils;     
    
    /// <summary>
    /// 
    /// </summary>
    public sealed class UpgradeOptions
    {
        public UpgradeOptions(CommandLineApplication command)
        {  
            this.ConnectionString = command.Option("--connection", "Target database connection string", CommandOptionType.SingleValue);  
            this.Recreate = command.Option("--recreate|-rc", "Drop and recreate all the database objects", CommandOptionType.NoValue);  
            this.Superuser = command.Option("--superuser|-su", "Superuser settings string", CommandOptionType.MultipleValue);  
            this.Stubs = command.Option("--with-stubs|-stubs", "Include stabs", CommandOptionType.NoValue); 
        }

          
        /// <summary>
        /// Target database connection string. --connection
        /// </summary>
        public CommandOption ConnectionString { get; private set;}   
        /// <summary>
        /// Drop and recreate all the database objects. --recreate|-rc
        /// </summary>
        public CommandOption Recreate { get; private set;}   
        /// <summary>
        /// Superuser settings string. --superuser|-su
        /// </summary>
        public CommandOption Superuser { get; private set;}   
        /// <summary>
        /// Include stabs. --with-stubs|-stubs
        /// </summary>
        public CommandOption Stubs { get; private set;}  

    }     
    
    /// <summary>
    /// 
    /// </summary>
    public sealed class ProvisionOptions
    {
        public ProvisionOptions(CommandLineApplication command)
        {  
            this.Host = command.Option("--host|-h", "Server name", CommandOptionType.SingleValue);  
            this.Username = command.Option("--username|-u", "User name", CommandOptionType.SingleValue);  
            this.Password = command.Option("--password|-p", "Password", CommandOptionType.SingleValue);  
            this.Database = command.Option("--dbname|-db", "Name of the database to be created", CommandOptionType.SingleValue);  
            this.DatabaseAdminPassword = command.Option("--admin-password|-dap", "Database admin- password", CommandOptionType.SingleValue); 
        }

          
        /// <summary>
        /// Server name. --host|-h
        /// </summary>
        public CommandOption Host { get; private set;}   
        /// <summary>
        /// User name. --username|-u
        /// </summary>
        public CommandOption Username { get; private set;}   
        /// <summary>
        /// Password. --password|-p
        /// </summary>
        public CommandOption Password { get; private set;}   
        /// <summary>
        /// Name of the database to be created. --dbname|-db
        /// </summary>
        public CommandOption Database { get; private set;}   
        /// <summary>
        /// Database admin- password. --admin-password|-dap
        /// </summary>
        public CommandOption DatabaseAdminPassword { get; private set;}  

    }     
    
    /// <summary>
    /// 
    /// </summary>
    public sealed class DeprovisionOptions
    {
        public DeprovisionOptions(CommandLineApplication command)
        {  
            this.Host = command.Option("--host|-h", "Server name", CommandOptionType.SingleValue);  
            this.Username = command.Option("--username|-u", "User name", CommandOptionType.SingleValue);  
            this.Password = command.Option("--password|-p", "Password", CommandOptionType.SingleValue);  
            this.Database = command.Option("--dbname|-db", "Name of the database to be created", CommandOptionType.SingleValue); 
        }

          
        /// <summary>
        /// Server name. --host|-h
        /// </summary>
        public CommandOption Host { get; private set;}   
        /// <summary>
        /// User name. --username|-u
        /// </summary>
        public CommandOption Username { get; private set;}   
        /// <summary>
        /// Password. --password|-p
        /// </summary>
        public CommandOption Password { get; private set;}   
        /// <summary>
        /// Name of the database to be created. --dbname|-db
        /// </summary>
        public CommandOption Database { get; private set;}  

    } 

}
