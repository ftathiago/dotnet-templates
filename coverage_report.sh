#!/bin/bash
find ./**/coverage_report/ -delete
find ./**/coverage.json -delete
find ./**/coverage.info -delete
find ./**/coverage.cobertura.xml -delete
find ./**/coverage.opencover.xml -delete
dotnet tool update -g dotnet-reportgenerator-globaltool --version=5.0.4
dotnet test ./src/Content/WebApi/WebApi.sln --collect:"XPlat Code Coverage" /p:CollectCoverage=true /p:CoverletOutput="../" /p:MergeWith="../coverage.json"  /p:CoverletOutputFormat=json%2copencover%2clcov%2ccobertura
dotnet test ./src/Content/WebApi/__tests__/WebApi.EfInfraData.Tests/WebApi.EfInfraData.Tests.csproj --collect:"XPlat Code Coverage" /p:CollectCoverage=true /p:CoverletOutput="../" /p:MergeWith="../coverage.json"  /p:CoverletOutputFormat=json%2copencover%2clcov%2ccobertura
dotnet test ./src/Content/WebApi/__tests__/WebApi.DapperInfraData.Tests/WebApi.DapperInfraData.Tests.csproj --collect:"XPlat Code Coverage" /p:CollectCoverage=true /p:CoverletOutput="../" /p:MergeWith="../coverage.json"  /p:CoverletOutputFormat=json%2copencover%2clcov%2ccobertura
reportgenerator -reports:./src/**/coverage.opencover.xml -targetdir:coverage_report -reporttypes:"html"
npx http-server -o ./coverage_report/index.html
 
 