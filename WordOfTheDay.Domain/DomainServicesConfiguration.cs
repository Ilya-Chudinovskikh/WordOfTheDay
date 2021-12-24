using Microsoft.Extensions.DependencyInjection;

namespace WordOfTheDay.Domain
{
    public static class DomainServicesConfiguration
    {
        public static void AddDomain(this IServiceCollection services)
        {
            services.AddScoped<IWordsServices, WordsServices>();
        }
    }
}
