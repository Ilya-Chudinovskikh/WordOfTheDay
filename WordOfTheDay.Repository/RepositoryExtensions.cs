using System;
using System.Collections.Generic;
using System.Linq;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace WordOfTheDay.Repository
{
    internal static class RepositoryExtensions
    {
        public static IQueryable<Word> LaterThan(this IQueryable<Word> query, DateTime today)
        {
            query = query.Where(word => word.AddTime > today);

            return query;
        }
        public static IQueryable<Word> ByEmail(this IQueryable<Word> query, string email)
        {
            query = query.Where(word => word.Email == email);

            return query;
        }
        public static IQueryable<Word> ByText(this IQueryable<Word> query, string text)
        {
            query = query.Where(word => word.Text == text);

            return query;
        }
        public static IQueryable<Word> FilterClosestWords(this IQueryable<Word> query, List<string> keys, string word)
        {
            var predicate = PredicateBuilder.New<Word>();

            foreach (string key in keys)
            {
                predicate = predicate.Or(
                    closeWord => EF.Functions.Like(closeWord.Text, key)
                    && closeWord.Text.Length <= word.Length + 1
                    && closeWord.Text != word);
            }

            query = query.Where(predicate);

            return query;
        }
        public static async Task<WordCount> FindWordOfTheDay(this IQueryable<Word> query)
        {
            var result = await query.GroupBy(word => word.Text, (text, amount) => new { text, amount = amount.Count(word => word.Text == text)})
                .OrderByDescending(w => w.amount).FirstOrDefaultAsync();

            var wordOfTheDay = new WordCount(null, 0);

            if (result != null)
                wordOfTheDay = new WordCount(result.text, result.amount);
            else
                return null;

            return wordOfTheDay;
        }
        public static IQueryable<WordCount> GroupByWordCount(this IQueryable<Word> query)
        {
            var result = query.GroupBy(word => word.Text, (text, words) => new WordCount(text, words.Count(word => word.Text == text)));

            return result;
        }
    }
}
