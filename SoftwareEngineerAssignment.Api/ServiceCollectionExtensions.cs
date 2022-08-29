using SoftwareEngineerAssignment.Api.Constants;
using SoftwareEngineerAssignment.Api.Services;

namespace SoftwareEngineerAssignment.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, LocalMemoryCacheService>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient<AdviceSlipService>(client =>
                {
                    client.BaseAddress = new Uri(RouteConstants.AdviceSlipServiceBaseUrl);
                });

            services.AddSingleton<IAdviceSlipService>(
                x => new CachedAdviceSlipService(
                    x.GetRequiredService<ILogger<CachedAdviceSlipService>>(),
                    x.GetRequiredService<ICacheService>(),
                    x.GetRequiredService<AdviceSlipService>()));

            return services;
        }
    }
}