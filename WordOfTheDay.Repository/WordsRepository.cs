using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WordOfTheDay.Repository.Entities;
using System.Threading;

namespace WordOfTheDay.Repository
{
    public static class WordsRepository
    {
        public static async Task<(string, int)> WordOfTheDay(WordContext _context)
        {
            var wordOfTheDayType = await _context.Words.GroupBy(word => word.Text, (text, words)=> new { text, words = words.Count(word=>word.Text==text)})
                .OrderByDescending(el => el.words).FirstOrDefaultAsync();

            var wordOfTheDay = (wordOfTheDayType.text, wordOfTheDayType.words);

            return wordOfTheDay;
        }
        public static async Task<int> CountWord(WordContext _context, string text)
        {
            var count = await _context.Words.CountAsync(_word => _word.Text == text);

            return count;
        }
        public static IQueryable<Word> CloseWords(WordContext _context, Word word)
        {
            List<(string, int)> closeWords = new();
            List<string> keys = new();
            IQueryable<Word> query = _context.Words;
            string text = word.Text;
            int len = text.Length;


            for (int i = 0; i < len; i++)
                if (i == 0)
                    keys.Add('%' + text.Substring(1, len - 1));
                else if (i == len - 1)
                    keys.Add(text.Substring(0, i) + '%');
                else
                    keys.Add(text.Substring(0, i) + '%' + text.Substring(i + 1, len - i - 1));


            foreach (string key in keys)
                query = query.Where(closeWord => EF.Functions.Like(text, key) && closeWord.Text.Length <= text.Length + 1);

            return query;
        }
       
        public static async Task<Word> PostWord(Word word, WordContext _context)
        {
            _context.Words.Add(word);
            await _context.SaveChangesAsync();

            return word;
        }
        public static async Task<bool> IsAlreadyExist(Word word, WordContext _context)
        {
            bool exist = await _context.Words.AnyAsync(w => w.Email == word.Email);

            return exist;
        }
    }
}
