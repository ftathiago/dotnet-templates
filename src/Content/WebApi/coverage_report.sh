#!/bin/bash
find ./coverage_report/ -delete
find ./__tests__/**/coverage.info -delete
find ./__tests__/**/coverage.cobertura.xml -delete
find ./__tests__/**/coverage.opencover.xml -delete
dotnet clean
dotnet tool update dotnet-reportgenerator-globaltool
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat="lcov%2ccobertura%2copencover" -- /Parallel
reportgenerator -reports:__tests__/**/coverage.opencover.xml -targetdir:coverage_report
reportgenerator -reports:__tests__/**/coverage.info -targetdir:coverage_report -reporttypes:"lcov"
npx http-server -o coverage_report
