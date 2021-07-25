param (
  [string]$solution = "WebApi.sln"
)

$outfile = ".\eng\docker\dockerfile"

# This script creates the $outfile file, with Dockerfile commands.
# To increase build speed by optimizing the use of docker build images cache.
# This script is only needed when adding or removing projects from the solution.
Set-Content -Path $outfile "FROM mcr.microsoft.com/dotnet/sdk AS dotnet_restore"
Add-Content -Path $outfile "WORKDIR /src"
Add-Content -Path $outfile "COPY ""$solution"" ""$solution""" 
Select-String -Path $solution -Pattern ', "(.*?\.csproj)"' | ForEach-Object { $_.Matches.Groups[1].Value.Replace("\", "/") } | Sort-Object | ForEach-Object { "COPY ""$_"" ""$_""" } | Out-File -FilePath $outfile -Append
Select-String -Path $solution -Pattern ', "(.*?\.dcproj)"' | ForEach-Object { $_.Matches.Groups[1].Value.Replace("\", "/") } | Sort-Object | ForEach-Object { "COPY ""$_"" ""$_""" } | Out-File -FilePath $outfile -Append
Add-Content -Path $outfile "RUN dotnet restore ""$solution"""
Add-Content -Path $outfile ""
Add-Content -Path $outfile "FROM dotnet_restore AS dotnet_publish"
Add-Content -Path $outfile "WORKDIR /src"
Add-Content -Path $outfile "COPY . ."
Add-Content -Path $outfile "RUN dotnet publish ""$solution"" -c Release -o /app "
Add-Content -Path $outfile ""
Add-Content -Path $outfile "FROM mcr.microsoft.com/dotnet/aspnet AS runtime"
Add-Content -Path $outfile "WORKDIR /app"
Add-Content -Path $outfile "COPY --from=dotnet_publish /app ."
Add-Content -Path $outfile "EXPOSE 80"
Add-Content -Path $outfile "EXPOSE 443"
Add-Content -Path $outfile "ENTRYPOINT [""dotnet"", ""WebApi.Api.dll""]"

# Get-Content $outfile