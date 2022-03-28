$scriptDir = split-path -parent $MyInvocation.MyCommand.Definition
$srcDir = (Join-Path -path $scriptDir src)
# nuget.exe needs to be on the path or aliased
function Reset-Templates {
    [cmdletbinding()]
    param(
        [string]$templateEngineUserDir = (join-path -Path $env:USERPROFILE -ChildPath .templateengine)
    )
    process {
        'resetting dotnet new templates. folder: "{0}"' -f $templateEngineUserDir | Write-host
        get-childitem -path $templateEngineUserDir -directory | Select-Object -ExpandProperty FullName | remove-item -recurse
        &dotnet new --debug:reinit
    }
}
function Clean() {
    [cmdletbinding()]
    param(
        [string]$rootFolder = $scriptDir
    )
    process {
        'clean started, rootFolder "{0}"' -f $rootFolder | write-host
        # delete folders that should not be included in the nuget package
        Get-ChildItem -path $rootFolder -include bin, obj, nupkg, coverage_report, app -Recurse -Directory | Select-Object -ExpandProperty FullName | Remove-item -recurse
        Get-ChildItem -name -r | select-string -pattern ".*open.*\.xml" | ForEach-Object { Remove-Item $_ -Recurse -Force -Confirm:$false } | Out-Null
        Get-ChildItem -name -r | select-string -pattern "node_modules" | ForEach-Object { Remove-Item $_ -Recurse -Force -Confirm:$false } | Out-Null
    }
}



# start script
Clean

# create nuget package
$outputpath = Join-Path $scriptDir nupkg
$pathtonuspec = Join-Path $srcDir ambevtech.dotnet-templates.csproj
if (Test-Path $pathtonuspec) {
    dotnet pack --output $outputpath $pathtonuspec
}
else {
    'ERROR: nuspec file not found at {0}' -f $pathtonuspec | Write-Error
    return
}

$pathtonupkg = join-path $scriptDir nupkg/*.nupkg
# install nuget package using dotnet new --install
if (test-path $pathtonupkg) {   
    Reset-Templates
    'installing template with command "dotnet new --install {0}"' -f $pathtonupkg | write-host
    &dotnet new --install $pathtonupkg
}
else {
    'Not installing template because it was not found at "{0}"' -f $pathtonupkg | Write-Error
}