# BFT Dotnet templates
Hello!

I began this repository as a ['blog post toy-code'](https://www.blogdoft.com.br/2021/03/04/criando-templates-para-dotnet-new/). But, with passing time, this project became a little bit serious. So I decide to format and share as a Nuget package (in process).
You can use this template in your projects freely. I just ask you to add some copyright in your code and/or any final artifact (as a "thank's to" page, for instance).

## What is inside the box?

### WebApi 

A template to build your WebAPIs. We project this thinking in a micro-services scenario, working with kubernetes at all. So maybe you will miss some components or ask your self "why this is here?". You may open a issue when this occurs.
This template contains:
- Warmup: As you may know, ASP.Net Core has a warmup issue. This project does not solve completely this issue, but minimize this problem. Our warmup process do:
  - Pre-loading all objects at DependencyInjection - including any singleton (attention: None generic class was warmed up);
  - Make a dummy call to database, thus the connection pull will be filled with minimum connections number
  - Call any external api call, but you need this manually. Create a class implementing `WebApi.WarmUp.Abstractions.IWarmUpCommand` interface. The `Execute` method will be called during warmup process;
- Dapper as a data access layer;
- Notification strategy: you may fell free to use Exceptions everywhere. But I do not encourage this. Then I share to you a notification pattern for your business errors;
- Serilog as log machinery. I desire to design a "log wrapper", since Serilog should be a over engineering in micro services (you can use other ways to load logs from stdout);
- Code coverage report generator: If you wanna see you code coverage before sending code to your repo, you may call - from project root - `.\coverage_report.ps1` and a coverage report will be generated;

For future I want to:
- Bump version to .net 6;
- Add [Open telemetry](https://opentelemetry.io/) support (and parametrize this);;
- Add Mongo DB support (and parametrize this);
- Add Entity Framework core support (and parametrize this);
- Add Kafka Producing support (and parametrize this);

### WorkerApp

### ConsoleApp



# Thank's to:

I wanna say "thank you" to @timheuer and @sayedihashimi fo the help tha made it possible for me to write this article to portuguese readers.
Maybe you wanna see the @sayedihashimi works (I advice it). [So, just open his repository!](https://github.com/sayedihashimi/template-sample). You will see more tips and examples there.

And should go to [ASP.Net Core Project Templates repository](https://github.com/dotnet/aspnetcore/tree/main/src/ProjectTemplates) too to see others examples with more complexity.

Be welcome to use, distribute, fork and improve this repo!
