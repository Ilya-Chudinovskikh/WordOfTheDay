using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository.Models;
using LinqKit;

namespace WordOfTheDay.Repository
{
    internal sealed class WordsRepository : IWordsRepository
    {
        private readonly WordContext _context;
        public WordsRepository(WordContext context)
        {
            _context = context;
        }
        public async Task<WordCount> WordOfTheDay()
        {
            var wordOfTheDayType = await _context.Words
                .Where(word=>word.AddTime > DateTime.Today)
                .GroupBy(word => word.Text, (text, words) => new { text, words = words.Count(word => word.Text == text) })
                .OrderByDescending(el => el.words).FirstOrDefaultAsync();

            if (wordOfTheDayType == null)
                return null;

            var wordOfTheDay = new WordCount(wordOfTheDayType.text, wordOfTheDayType.words);
            
            return wordOfTheDay;
        }
        public Task<List<WordCount>> CloseWords(string word)
        {
            var keys = GetKeys(word);

            var predicate = PredicateBuilder.New<Word>();

            foreach (string key in keys)
            {
                predicate = predicate.Or(
                    closeWord => EF.Functions.Like(closeWord.Text, key) 
                    && closeWord.Text.Length <= word.Length + 1 
                    && closeWord.Text != word);
            }

            var closeWords = _context.Words
                .AsExpandable().Where(predicate)
                .GroupBy(word => word.Text, (text, words) => new WordCount(text, words.Count(word => word.Text == text)));

            return Task.FromResult(closeWords.ToList());
        }
        public async Task PostWord(Word word)
        {
            _context.Words.Add(word);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsAlreadyExist(Word word)
        {
            var exist = await _context.Words.AnyAsync(w => w.Email == word.Email && w.AddTime > DateTime.Today);

            return exist;
        }
        public async Task<WordCount> UserWord(string email)
        {
            var word = await _context.Words
                .Where(word => word.AddTime > DateTime.Today)
                .SingleOrDefaultAsync(w=>w.Email==email);

            var userWordAmount = await _context.Words
                .Where(w => w.AddTime > DateTime.Today && w.Text == word.Text)
                .CountAsync();

            var userWord = new WordCount(word.Text, userWordAmount);

            return userWord;
        }
        public static List<string> GetKeys(string word)
        {
            var keys = new List<string>();
            var len = word.Length;

            for (var i = 0; i < len; i++)
            {
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
            }

            return keys;
        }
    }
}
