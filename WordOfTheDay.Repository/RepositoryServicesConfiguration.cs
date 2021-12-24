using Microsoft.Extensions.DependencyInjection;

namespace WordOfTheDay.Repository
{
    public static class RepositoryServicesConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IWordsRepository, WordsRepository>();
        }
    }
}
