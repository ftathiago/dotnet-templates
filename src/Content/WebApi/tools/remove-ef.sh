#!/bin/bash
dotnet sln add ./**/*.DapperInfraData*
find **/*.EfInfraData* -delete
