#!/bin/bash
dotnet clean
dotnet tool update -g dotnet-reportgenerator-globaltool
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=coverage.opencover.xml
reportgenerator -reports:__tests__/**/coverage.opencover.xml -targetdir:coverage_report
xdg-open ./coverage_report/index.html
