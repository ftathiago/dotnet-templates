rmdir /s /q src\WebApi.EfInfraData
rmdir /s /q __tests__\WebApi.EfInfraData.Tests
dotnet sln add src\WebApi.DapperInfraData
dotnet sln add __tests__\WebApi.DapperInfraData.Tests