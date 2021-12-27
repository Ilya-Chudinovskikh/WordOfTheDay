using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WordOfTheDay.Repository.Entities;

namespace WordOfTheDay.Repository
{
    public static class RepositoryServicesConfiguration
    {
        public static void AddRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WordContext>(options =>
                    options.UseSqlServer(connectionString));

            services.AddScoped<IWordsRepository, WordsRepository>();
        }
    }
}
