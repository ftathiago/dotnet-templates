# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

## [1.0.1](https://github.com/ftathiago/dotnet-templates/compare/v1.0.0...v1.0.1) (2021-06-06)
### Features

#### refact: Alter MessageHolder to correspond to Notification pattern

- Change MessageHolder ErrorCode to int: As notification pattern it's about domain logic, keep error code as HttpStatusCode is a SOLID offense and also a "Ports and Adapters" offense.
SOLID because, violates the Single Responsibility Principle, keeping notifications and resolving http status returns.
Ports and Adapters because the core layer (generally, the domain layer or "Business in our case") has to presume what is the driver adapter (HTTP in this case).

- Change Notification from Shared to Business layer: Despistes driven layers have their it specific errors and exceptions, they should be translated to Domain errors.
In this way, domain can handle it and/or by passing it.
Considering that, make sense the notification mechanism stay in Business layer and ErrorCode specification be there too.
Thus, every driver or driven layer can know what error domain can handle.

- Map error codes to HttpStatusCodes: API layers should map any ErroCodes

- Rename MessageHolder to Notification: This denomination seems to be more appropriate.

## 1.0.0 (2021-06-05)


### Features

* initial commit ([e52ee1f](https://github.com/ftathiago/dotnet-templates/commit/e52ee1fbc4f332d6720d54169d0a254f9bea93c5))

This version works on Project templates, compatible with any dotnet CLI and Visual Studio, containing:
- FluentMigrations: A template for using with [FluentMigrator](https://www.nuget.org/packages/FluentMigrator/)
- FluentValidatorTemplate: A template for using with [FluentValidator](https://www.nuget.org/packages/FluentValidator/)
- WebApi-Custom: A template for WebApi, supporting:
  - Dapper
  - Warmup capabilities
  - Serilog
  - Static code analysis
    - StyleCop
    - Roslynator
  - API Versioning
  - Swagger