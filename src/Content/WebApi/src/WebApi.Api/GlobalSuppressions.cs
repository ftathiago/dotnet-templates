// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This is not really necessary here", Scope = "member", Target = "M:WebApi.Startup.ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This is not really necessary here", Scope = "member", Target = "M:WebApi.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder,Microsoft.AspNetCore.Hosting.IWebHostEnvironment,Microsoft.AspNetCore.Mvc.ApiExplorer.IApiVersionDescriptionProvider)")]
