#!/bin/bash
dotnet new --debug:reinit

find ./**/*.nupkg -printf "%f\n" | xargs -i find ~/.templateengine/packages/{} -delete
find ./**/obj/ -delete
find ./**/bin/ -delete
find ./**/nupkg/ -delete
find ./**/*opencover.xml -delete
find ./**/node_modules -delete
find ./**/coverage_report/ -delete

dotnet pack --output ./nupkg ./src/ambevtech.dotnet-templates.csproj
dotnet new --install ./nupkg/*.nupkg