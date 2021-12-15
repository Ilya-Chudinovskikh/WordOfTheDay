using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WordOfTheDay.Entities;
using System.Threading;
using WordOfTheDay.Repository;

namespace WordOfTheDay.Domain
{
    public static class WordsServices
    {
        public static async Task<List<Word>> GetWords(WordContext _context)
        {
            var words = await WordsRepository.AllWords(_context);

            return words;
        }
        public static async Task<Word> GetWord(Guid id, WordContext _context)
        {
            var word = await WordsRepository.GetWordById(id, _context);

            return word;
        }
        public static async Task<string> WordOfTheDay(WordContext _context)
        {
            var words = await WordsRepository.AllWords(_context);
            var wordOfTheDay = words.GroupBy(word => word.Text).OrderByDescending(el => el.Count()).First().Key;

            return wordOfTheDay;
        }
        public static async Task<long> GetAmountWordOfTheDay(WordContext _context)
        {
            var wordOfTheDay = await WordOfTheDay(_context);
            var words = await WordsRepository.AllWords(_context);

            long amount = words.Where(w => w.Text == wordOfTheDay).Count();

            return amount;
        }
        public static async Task<long> GetAmount(Guid id, WordContext _context)
        {
            var word = await WordsRepository.GetWordById(id, _context);

            var words = await WordsRepository.AllWords(_context);
            long amount = words.Where(w => w.Text == word.Text).Count();

            return amount;
        }
        public static async Task<IEnumerable<string>> ClosestWords(Guid id, WordContext _context)
        {
            var word = await WordsRepository.GetWordById(id, _context);
            var words = await WordsRepository.AllWords(_context);

            var closestWords = words.Where(w => IsClose(word.Text, w.Text))
                .Select(w => w.Text).Distinct();

            return closestWords;
        }
        public static async Task<Word> PostWord(Word word, WordContext _context)
        {
            word.AddTime = DateTime.Now;

            await WordsRepository.PostWord(word, _context);

            return word;
        }
        public static async Task<bool> AlreadyExist(Word word, WordContext _context)
        {
            var words = await WordsRepository.AllWords(_context);
            bool exist = words.Any(w => w.Email == word.Email);

            return exist;
        }
        private static bool IsClose(string word, string compare)
        {
            word = word.ToLower();
            compare = compare.ToLower();
            if (word == compare) return false;
            int error = 0, wlen = word.Length, clen = compare.Length, delta = wlen - clen;
            if (Math.Abs(delta) > 1) return false;
            for (int i = 0, j = 0; i < Math.Min(wlen, clen); i++, j++)
            {
                if (word[i] != compare[j])
                {
                    error++;
                    if (error > 1) return false;
                    if (delta > 0) j--;
                    if (delta < 0) i--;
                }
            }
            return true;
        }
    }
}

