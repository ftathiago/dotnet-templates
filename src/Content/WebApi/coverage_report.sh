#!/bin/bash
find ./coverage_report/ -delete
find ./**/coverage.info -delete
find ./**/coverage.cobertura.xml -delete
find ./**/coverage.opencover.xml -delete
find ./**/coverage.json -delete
find ./__tests__/**/TestResults -delete
dotnet clean
dotnet tool update dotnet-reportgenerator-globaltool
dotnet test -l trx  /p:CollectCoverage=true /p:CoverletOutput="../" /p:MergeWith="../coverage.json" /p:CoverletOutputFormat=json%2copencover%2clcov%2ccobertura
reportgenerator -reports:./__tests__/**/coverage.opencover.xml -targetdir:coverage_report
reportgenerator -reports:./__tests__/**/coverage.info -targetdir:coverage_report -reporttypes:"lcov"
npx http-server -o coverage_report
