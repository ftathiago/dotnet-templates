Write-Host "Update CycloneDX"
dotnet tool update --global CycloneDX | Out-Null

Write-Host "Generating bom file"
dotnet CycloneDX .\Vom.Backend.sln -o dtrack_files | Out-Null

Write-Host "+---------------------------------------------------------------+"
Write-Host "|                                                               |"
Write-Host "| To verify your packages, please visit https://localhost:8080/ |"
Write-Host "| search this project and upload file at .\dtrack_files\bom.xml |"
Write-Host "|                                                               |"
Write-Host "+---------------------------------------------------------------+"
