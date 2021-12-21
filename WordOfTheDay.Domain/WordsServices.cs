using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WordOfTheDay.Repository.Entities;
using System.Threading;
using WordOfTheDay.Repository;

namespace WordOfTheDay.Domain
{
    public static class WordsServices
    {
        
        public static async Task<(string, int)> WordOfTheDay(WordContext _context)
        {
            var wordOfTheDay = await WordsRepository.WordOfTheDay(_context);

            return wordOfTheDay;
        }
        
        public static async Task<Word> PostWord(Word word, WordContext _context)
        {
            word.AddTime = DateTime.Now;

            await WordsRepository.PostWord(word, _context);

            return word;
        }
        public static async Task<bool> IsAlreadyExist(Word word, WordContext _context)
        {
            var exist = await WordsRepository.IsAlreadyExist(word, _context);

            return exist;
        }
        
    }
}

