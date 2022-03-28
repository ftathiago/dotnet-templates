#!/bin/bash
dotnet sln add ./**/*.EfInfraData*
find **/*.DapperInfraData* -delete
