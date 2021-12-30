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
    public class WordsRepository : IWordsRepository
    {
        private readonly WordContext _context;
        public WordsRepository(WordContext context)
        {
            _context = context;
        }
        public async Task<WordCount> WordOfTheDay()
        {
            var time = DateTime.Today.ToUniversalTime();

            var wordOfTheDayType = await _context.Words
                .Where(word=>word.AddTime > time)
                .GroupBy(word => word.Text, (text, words) => new { text, words = words.Count(word => word.Text == text) })
                .OrderByDescending(el => el.words).FirstOrDefaultAsync();

            if (wordOfTheDayType == null)
                return null;

            var wordOfTheDay = new WordCount(wordOfTheDayType.text, wordOfTheDayType.words);
            
            return wordOfTheDay;
        }
        public async Task<List<WordCount>> CloseWords(string email)
        {
            var time = DateTime.Today.ToUniversalTime();

            var word = (await UserWord(email)).Word;

            var keys = GetKeys(word);

            var predicate = PredicateBuilder.New<Word>();

            foreach (string key in keys)
            {
                predicate = predicate.Or(
                    closeWord => EF.Functions.Like(closeWord.Text, key) 
                    && closeWord.Text.Length <= word.Length + 1 
                    && closeWord.Text != word
                    && closeWord.AddTime > time);
            }

            var closeWords = _context.Words
                .AsExpandable().Where(predicate)
                .GroupBy(word => word.Text, (text, words) => new WordCount(text, words.Count(word => word.Text == text)));

            return await closeWords.ToListAsync();
        }
        public async Task PostWord(Word word)
        {
            _context.Words.Add(word);
            await _context.SaveChangesAsync();
        }
        public Task<bool> IsAlreadyExist(Word word)
        {
            var time = DateTime.Today.ToUniversalTime();

            var exist = _context.Words
                .AnyAsync(w => w.Email == word.Email && w.AddTime > time);

            return exist;
        }
        public async Task<WordCount> UserWord(string email)
        {
            var time = DateTime.Today.ToUniversalTime();

            var word = await _context.Words
                .Where(word => word.AddTime > time)
                .SingleOrDefaultAsync(w=>w.Email==email);

            var userWordAmount = await _context.Words
                .Where(w => w.AddTime > time && w.Text == word.Text)
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
