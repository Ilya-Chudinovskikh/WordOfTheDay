using System;
using System.Linq;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository.Models;

namespace WordOfTheDay.Repository
{
    public static class RepositoryExtensions
    {
        public static IQueryable<Word> LaterThan(this IQueryable<Word> query)
        {
            var date = DateTime.Today.ToUniversalTime();

            query = query.Where(word => word.AddTime > date);

            return query;
        }
    }
}
