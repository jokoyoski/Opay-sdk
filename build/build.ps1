param(
    [switch]$skip = $false
)
if ($skip)
{
    Write-Host "Skipping build"
}
else
{
    dotnet build -c Release ./src/OpayCashier/OpayCashier.csproj
}