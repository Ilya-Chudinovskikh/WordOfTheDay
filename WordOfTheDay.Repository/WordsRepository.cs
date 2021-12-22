using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository.Models;
using LinqKit;

namespace WordOfTheDay.Repository
{
    public static class WordsRepository
    {
        public static async Task<WordCount> WordOfTheDay(WordContext context)
        {
            var wordOfTheDayType = await context.Words.GroupBy(word => word.Text, (text, words)=> new { text, words = words.Count(word=>word.Text==text)})
                .OrderByDescending(el => el.words).FirstOrDefaultAsync();

            var wordOfTheDay = new WordCount(wordOfTheDayType.text, wordOfTheDayType.words);

            return wordOfTheDay;
        }
        public static async Task<int> CountWord(WordContext context, string text)
        {
            var count = await context.Words.CountAsync(_word => _word.Text == text);

            return count;
        }
        public static IQueryable<Word> CloseWords(WordContext context, string word)
        {
            var keys = new List<string>();
            var len = word.Length;


            for (var i = 0; i < len; i++)
                if (i == 0)
                {
                    keys.Add($"%{word.Substring(1, len - 1)}");
                }
                else if (i == len - 1)
                {
                    keys.Add($"{word.Substring(0, i)}%");
                }
                else
                {
                    keys.Add($"{word.Substring(0, i)}%{word.Substring(i + 1, len - i - 1)}");
                }

            var predicate = PredicateBuilder.New<Word>();

            foreach (string key in keys)
            {
                predicate = predicate.Or(
                    closeWord => EF.Functions.Like(closeWord.Text, key) 
                    && closeWord.Text.Length <= len + 1 
                    && closeWord.Text != word);
            }

            var result =  context.Words.AsExpandable().Where(predicate);

            return result;
        }
       
        public static async Task PostWord(Word word, WordContext context)
        {
            context.Words.Add(word);
            await context.SaveChangesAsync();
        }
        public static async Task<bool> IsAlreadyExist(Word word, WordContext context)
        {
            var exist = await context.Words.AnyAsync(w => w.Email == word.Email);

            return exist;
        }
    }
}

