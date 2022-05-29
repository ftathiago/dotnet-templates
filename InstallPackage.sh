#!/bin/bash
dotnet new --uninstall BlogDoFT.dotnet-templates.nuspec
dotnet new --debug:reinit

find ./**/*.nupkg -printf "%f\n" | xargs -i find ~/.templateengine/packages/{} -delete
find ./**/obj/ -delete
find ./**/bin/ -delete
find ./**/nupkg/ -delete
find ./**/*opencover.xml -delete
find ./**/node_modules -delete
find ./**/coverage_report/ -delete
find ./**/__tests__/**/coverage.json -delete
find ./**/__tests__/**/coverage.info -delete
find ./**/__tests__/**/coverage.cobertura.xml -delete
find ./**/__tests__/**/coverage.opencover.xml -delete


dotnet pack --output ./nupkg ./src/BlogDoFT.dotnet-templates.csproj
dotnet new --install ./nupkg/*.nupkg