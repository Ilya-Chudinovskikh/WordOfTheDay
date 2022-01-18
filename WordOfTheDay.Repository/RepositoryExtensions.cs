using System;
using System.Collections.Generic;
using System.Linq;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace WordOfTheDay.Repository
{
    internal static class RepositoryExtensions
    {
        public static IQueryable<Word> LaterThan(this IQueryable<Word> query, DateTime today)
        {
            query = query.Where(word => word.AddTime >= today);

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
        public static IQueryable<WordCount> GroupByWordCount(this IQueryable<Word> query)
        {
            var result = query.GroupBy(word => word.Text, (text, words) => new WordCount { Word = text, Count = words.Count(word => word.Text == text) });

            return result;
        }
    }
}
