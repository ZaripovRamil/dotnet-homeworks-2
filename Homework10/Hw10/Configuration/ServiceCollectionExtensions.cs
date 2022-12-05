using Hw10.DbModels;
using Hw10.Services;
using Hw10.Services.CachedCalculator;

using Hw10.Services.MathCalculator;
using Microsoft.Extensions.Caching.Memory;

namespace Hw10.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCachedMathCalculator(this IServiceCollection services)
    {
        return services.AddScoped<IMathCalculator>(s =>
            new MathCachedCalculator(
                s.GetRequiredService<ApplicationContext>(), s.GetRequiredService<IMemoryCache>()));
    }
}