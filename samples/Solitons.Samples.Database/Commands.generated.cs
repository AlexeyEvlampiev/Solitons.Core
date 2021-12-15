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
            this.Superuser = command.Option("--superuser|-su", "Superuser email address", CommandOptionType.MultipleValue);  
            this.Stubs = command.Option("--with-stubs|-stubs", "Superuser email address", CommandOptionType.NoValue); 
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
        /// Superuser email address. --superuser|-su
        /// </summary>
        public CommandOption Superuser { get; private set;}   
        /// <summary>
        /// Superuser email address. --with-stubs|-stubs
        /// </summary>
        public CommandOption Stubs { get; private set;}  

    } 

}
