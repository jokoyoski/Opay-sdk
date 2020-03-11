$output = "out"
$version = $env:APPVEYOR_BUILD_VERSION
$nugetFile = $output + "/OPayCashier." + $version + ".nupkg"

Set-Location ./src/OpayCashier

dotnet pack -c Release -p:PackageVersion=$version -o $output

dotnet nuget push $nugetFile -k $env:NUGET_API_KEY -s https://api.nuget.org/v3/index.json
