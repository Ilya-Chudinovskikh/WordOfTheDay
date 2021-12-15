using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WordOfTheDay.Entities;
using System.Threading;

namespace WordOfTheDay.Repository
{
    public static class WordsRepository
    {
        public static async Task<List<Word>> AllWords(WordContext _context)
        {
            var allWords = await _context.Words.ToListAsync();

            return allWords;
        }

        public static async Task<Word> GetWordById(Guid id, WordContext _context)
        {
            var word = await _context.Words.FindAsync(id);

            return word;
        }
        public static async Task<Word> PostWord(Word word, WordContext _context)
        {
            _context.Words.Add(word);
            await _context.SaveChangesAsync();

            return word;
        }
    }
}
