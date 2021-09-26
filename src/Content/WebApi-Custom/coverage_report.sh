#!/bin/bash
dotnet clean
dotnet tool update -g dotnet-reportgenerator-globaltool
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat="lcov%2ccobertura%2copencover"
reportgenerator -reports:__tests__/**/coverage.opencover.xml -targetdir:coverage_report
reportgenerator -reports:__tests__/**/coverage.info -targetdir:coverage_report -reporttypes:"lcov"
xdg-open ./coverage_report/index.html
