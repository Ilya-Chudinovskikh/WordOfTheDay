using Microsoft.Extensions.DependencyInjection;
using MassTransit;

namespace WordOfTheDay.Domain
{
    public static class DomainServicesConfiguration
    {
        public static void AddDomain(this IServiceCollection services)
        {
            services.AddScoped<IWordsServices, WordsServices>();
        }
        public static void AddConfiguredMassTransit(this IServiceCollection services, string host)
        {
            services.AddMassTransit(Configuration =>
            {
                Configuration.UsingRabbitMq((context, config) =>
                {
                    config.Host(host);
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
