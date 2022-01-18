using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WordOfTheDay.Repository.Entities;
using System;

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
    }
}
