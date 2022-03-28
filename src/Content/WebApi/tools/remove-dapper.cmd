rmdir /s /q src\WebApi.DapperInfraData
rmdir /s /q __tests__\WebApi.DapperInfraData.Tests
dotnet sln add src\WebApi.EfInfraData
dotnet sln add __tests__\WebApi.EfInfraData.Tests