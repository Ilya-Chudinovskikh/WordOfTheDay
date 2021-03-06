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
        private static DateTime DateToday
        {
            get { return DateTime.Today.ToUniversalTime(); }
        }
        public async Task<WordCount> WordOfTheDay()
        {
            var wordOfTheDay = await _context.Words
                .LaterThan(DateToday)
                .GroupByWordCount()
                .OrderByDescending(w => w.Count)
                .FirstOrDefaultAsync();

            if (wordOfTheDay == null)
                return null;

            return wordOfTheDay;
        }
        public async Task<List<WordCount>> CloseWords(string email)
        {
            var word = (await UserWord(email)).Word;

            var keys = GetKeys(word);

            var closeWords = _context.Words
                .AsExpandable().FilterClosestWords(keys, word)
                .LaterThan(DateToday)
                .GroupByWordCount();

            return closeWords.ToList();
        }
        public async Task PostWord(Word word)
        {
            _context.Words.Add(word);
            await _context.SaveChangesAsync();
        }
        public Task<bool> IsAlreadyExist(Word word)
        {
            var exist = _context.Words
                .LaterThan(DateToday)
                .ByEmail(word.Email)
                .AnyAsync();

            return exist;
        }
        public async Task<WordCount> UserWord(string email)
        {
            var word = await _context.Words
                .LaterThan(DateToday)
                .ByEmail(email)
                .SingleOrDefaultAsync();

            var userWordAmount = await _context.Words
                .LaterThan(DateToday)
                .ByText(word.Text)
                .CountAsync();

            var userWord = new WordCount { Word = word.Text, Count = userWordAmount };

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
