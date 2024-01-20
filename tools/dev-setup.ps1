# Try-Catch block to check Docker availability
try {
    docker version | Out-Null
} catch {
    if ($_.Exception -like "*Cannot connect*") {
        Write-Error "Error: Docker daemon is not currently active. Please start Docker and retry executing this script."
    } elseif ($_.Exception -like "*not recognized*") {
        Write-Error "Error: Docker is either not installed or not configured in the system PATH. Please install Docker or verify your PATH settings."
    } else {
        Write-Error "Error: An unexpected issue has been encountered. Details: $_"
    }
    exit 1
}

Write-Host "Docker is running."

if (-not (Get-Module -ListAvailable -Name PSSQLite)) {
  Write-Host "Initiating installation of the PSSQLite module..."
  Install-Module -Name PSSQLite -Force
}
Import-Module PSSQLite

$IsWindows = [System.Environment]::OSVersion.Platform -eq [System.PlatformID]::Win32NT

$devDir = ""
if ($IsWindows) {
    if ([string]::IsNullOrEmpty($env:USERPROFILE)) {
        Write-Error "Error: The USERPROFILE environment variable is missing or undefined."
        exit 1
    }
    $devDir = Join-Path -Path $env:USERPROFILE -ChildPath "Development"
} else {
    if ([string]::IsNullOrEmpty($env:HOME)) {
        Write-Error "Error: The HOME environment variable is missing or undefined."
        exit 1
    }
    $devDir = Join-Path -Path $env:HOME -ChildPath "Development"
}

$dataDir = [System.IO.Path]::Combine($devDir, "Data")

if (-not (Test-Path -Path $devDir)) {
    Write-Host "Setting up a Development directory at the following path: $devDir."
    New-Item -Path $devDir -ItemType Directory -Force | Out-Null
}
if (-not (Test-Path -Path $dataDir)) {
    Write-Host "Setting up a Data directory at the following location: $dataDir."
    New-Item -Path $dataDir -ItemType Directory -Force | Out-Null
}

[Environment]::SetEnvironmentVariable("DEVELOPMENT_PATH", $devDir, [EnvironmentVariableTarget]::User)

$keyVaultSqlitePath = [system.io.path]::Combine($dataDir, "keyvault.sqlite")

$sqlCommands = @'
CREATE TABLE IF NOT EXISTS secret (
    scope VARCHAR(150),
    key VARCHAR(150),
    value TEXT,
    created_utc DATETIME DEFAULT (datetime('now','utc')),
    updated_utc DATETIME DEFAULT (datetime('now','utc')),
    PRIMARY KEY (scope, key)
);
CREATE UNIQUE INDEX IF NOT EXISTS idx_secret_scope_key ON secret (scope, key);
'@

try {
    Invoke-SqliteQuery -DataSource $keyVaultSqlitePath -Query $sqlCommands
    Write-Host "The database and its tables have been successfully initialized at the following path: $keyVaultSqlitePath"
    [Environment]::SetEnvironmentVariable("SQLITE_KEYVAULT_PATH", $keyVaultSqlitePath, [EnvironmentVariableTarget]::User)
} catch {
    Write-Error "Error: Database creation failed. Details: $_"
}

$containers = @()


$containers += @{
    Name = "postgis-16-3.4";
    Image = "postgis/postgis:16-3.4";
    Volume = "/var/lib/postgresql/data";
    Ports = @("5433:5432");
    Variables = @("POSTGRES_PASSWORD=postgres");
    
}

$containers += @{
    Name = "citus-12.0";
    Image = "citusdata/citus:12.0";
    Volume = "/var/lib/postgresql/data";
    Ports = @("5434:5432");
    Variables = @("POSTGRES_PASSWORD=postgres");
}


$containers += @{
    Name = "mysql-8.2.0";
    Image = "mysql:8.2.0";
    Volume = "/var/lib/mysql";
    Ports = @("3307:3306");
    Variables = @("MYSQL_ROOT_PASSWORD=mysql");
}


$containers += @{
    Name = "rabbitmq-3.12.12";
    Image = "rabbitmq:3.12.12-management";
    Volume = "/var/lib/rabbitmq";
    Ports = @("5673:5672", "15673:15672");
    Variables = @();
}



$containers += @{
    Name = "redis-7.2.4";
    Image = "redis:7.2.4";
    Volume = "/data";
    Ports = @("6380:6379");
    Variables = @();
}


$containers += @{
    Name = "mongo-4.4.2";
    Image = "mongo:4.4.2";
    Volume = "/data/db";
    Ports = @("27018:27017");
    Variables = @();
}


$containers += @{
    Name = "ganache-v7.9.2";
    Image = "trufflesuite/ganache:v7.9.2";
    Volume = "/data";
    Ports = @("8546:8545");
    Variables = @();
}



$containers = $containers | foreach{
    $output = $_
    $path = [System.Io.path]::Combine($dataDir, $_.Name)
    $cmd = "docker run -d " + "--name $($output.Name)"
    $output.Ports | foreach{ $cmd += " -p $_ "}
    if($output.Volume){ $cmd += " -v `"$($path):$($_.Volume)`"" }
    $output.Variables | foreach{ $cmd += " -e $_"}

    $output.Command = $cmd + " $($_.Image)"
    $output.Volume = $path

    "Ports", "Variables", "Image" | foreach{ $output.Remove($_) }
    $output
}




foreach ($item in $containers) {
    Write-Host "Commencing setup for the container named: $($item.Name)"
    
    if (-not (docker ps -a --format '{{.Names}}' | Select-String -Pattern $item.Name)) {
        if ($item.Volume) {
            if (Test-Path $item.Volume) {
                $directoryContent = Get-ChildItem -Path $item.Volume
                if($directoryContent){
                    $confirmation = Read-Host "Notice: The directory at [$item.Volume] contains existing data. Would you like to clear this directory before proceeding? [Y/N]"
                    if ($confirmation.ToUpper() -eq 'Y') {
                        Write-Host "Deleting contents of [$item.Volume]"
                        Remove-Item -Path $item.Volume -Recurse -Force
                    }
                }                
            }
            if (-not (Test-Path $item.Volume)) {
                [System.IO.Directory]::CreateDirectory($item.Volume)
            }
        }
        try {
            Invoke-Expression $item.Command
        } catch {
            Write-Error "Error: Failed to execute command for $($item.Name): $_"
        }
    } else {
        # If the container exists, notify the user
        Write-Host "Container named '$($item.Name)' already exists."
    }
}






# Function to insert/update connection string in the database
Function Set-ConnectionString {
    param (
        [string]$scope,
        [string]$key,
        [string]$value
    )

    $query = "REPLACE INTO secret (scope, key, value) VALUES ('$scope', '$key', '$value'); "

    Invoke-SqliteQuery -DataSource $keyVaultSqlitePath -Query $query
}

# Register connection strings
Set-ConnectionString -scope "localhost" -key "POSTGRES_CONNECTION_STRING" -value "Host=localhost;Port=5433;Username=postgres;Password=postgres;Database=postgres;"
Set-ConnectionString -scope "localhost" -key "CITUS_CONNECTION_STRING" -value "Host=localhost;Port=5434;Username=postgres;Password=postgres;Database=postgres;"
Set-ConnectionString -scope "localhost" -key "MYSQL_CONNECTION_STRING" -value "Server=localhost;Port=3307;User=root;Password=mysql;;"
Set-ConnectionString -scope "localhost" -key "RABBITMQ_CONNECTION_STRING" -value "amqp://guest:guest@localhost:5673/"
Set-ConnectionString -scope "localhost" -key "REDIS_CONNECTION_STRING" -value "localhost:6380,ssl=True,abortConnect=False"
Set-ConnectionString -scope "localhost" -key "MONGODB_CONNECTION_STRING" -value "mongodb://localhost:27018"
Set-ConnectionString -scope "localhost" -key "GANACHE_URL" -value "http://localhost:8546"

