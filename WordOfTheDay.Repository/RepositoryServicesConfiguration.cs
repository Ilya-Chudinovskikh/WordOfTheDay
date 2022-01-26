using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WordOfTheDay.Repository.Entities;
using System;
using MassTransit;

namespace WordOfTheDay.Repository
{
    public static class RepositoryServicesConfiguration
    {
        public static void AddRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WordContext>(options =>
                    options.UseSqlServer(connectionString).LogTo(Console.WriteLine));

            services.AddScoped<IWordsRepository, WordsRepository>();
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
