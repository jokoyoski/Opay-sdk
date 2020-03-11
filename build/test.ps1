$testOutput = dotnet test ./tests/OpayCashier.Tests/OpayCashier.Tests.csproj | Out-String

Write-Host $testOutput

if ($testOutput.Contains("Test Run Successful.") -eq $False) {
    Write-Host "Build failed!";
    Exit;
}
