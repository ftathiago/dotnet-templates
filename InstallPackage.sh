#!/bin/bash
rm -rf **/coverage_report/
rm -rf **/obj/
rm -rf **/bin/
rm -rf **/nupkg/
rm -rf **/*opencover.xml
dotnet new --debug:reinit
dotnet pack --output ./nupkg ./src/BlogDoFT.dotnet-templates.csproj
dotnet new --install ./nupkg/*.nupkg