using Microsoft.Extensions.DependencyInjection;
#if(isDapper)
using WebApi.DapperInfraData.Extensions;
#endif
using System.Diagnostics.CodeAnalysis;
using WebApi.Domain.Extensions;
using WebApi.Domain.Notifications;
#if(isEf)
using WebApi.EfInfraData.Extensions;
#endif
#if(!isDapper && !isEf)
using WebApi.DapperInfraData.Extensions;
#endif

namespace WebApi.IoC
{
    [ExcludeFromCodeCoverage]
    public static class IoC
    {
        public static IServiceCollection ProjectsIocConfig(this IServiceCollection services) =>
            services
                .AddBusiness()
#if(!isDapper && !isEf)
                .AddDapperInfraData()
#endif
#if (isDapper)
                .AddDapperInfraData()  
#endif
#if (isEf)
                .AddEfInfraData()
#endif
                .AddScoped<INotification, Notification>();
    }
}
