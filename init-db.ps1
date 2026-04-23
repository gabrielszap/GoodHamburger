param(
    [string]$ContainerName = "goodhamburger-postgres",
    [string]$DatabaseName = "goodhamburger",
    [string]$Username = "postgres",
    [string]$Password = "postgres"
)

$ErrorActionPreference = "Stop"

function Invoke-ContainerPsql {
    param(
        [string]$Database,
        [string]$CommandText
    )

    docker exec -e PGPASSWORD=$Password $ContainerName psql -v ON_ERROR_STOP=1 -U $Username -d $Database -c $CommandText
}

function Invoke-ContainerPsqlFile {
    param(
        [string]$Database,
        [string]$FilePath
    )

    Get-Content -Raw $FilePath | docker exec -i -e PGPASSWORD=$Password $ContainerName psql -v ON_ERROR_STOP=1 -U $Username -d $Database
}

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path

$containerId = docker ps -q -f "name=^${ContainerName}$"
if ([string]::IsNullOrWhiteSpace($containerId)) {
    throw "Container '$ContainerName' is not running. Start it with: docker compose -f database/docker-compose.yml up -d"
}

$databaseExists = docker exec -e PGPASSWORD=$Password $ContainerName `
    psql -U $Username -d postgres -tAc "select 1 from pg_database where datname = '$DatabaseName';"

if ($databaseExists.Trim() -ne "1") {
    Invoke-ContainerPsql -Database "postgres" -CommandText "create database $DatabaseName;"
}

Invoke-ContainerPsqlFile -Database $DatabaseName -FilePath (Join-Path $scriptRoot "./database/001_initial_schema.sql")
Invoke-ContainerPsqlFile -Database $DatabaseName -FilePath (Join-Path $scriptRoot "./database/002_indexes_and_constraints.sql")
Invoke-ContainerPsqlFile -Database $DatabaseName -FilePath (Join-Path $scriptRoot "./database/003_seed_minimal.sql")

Invoke-ContainerPsql -Database $DatabaseName -CommandText "select to_regclass('public.users') as users_table;"
