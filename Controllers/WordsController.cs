using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WordOfTheDay.Models;
using System.Threading;

namespace WordOfTheDay.Controllers
{
    [Route("api/words")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly WordContext _context;

        public WordsController(WordContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetWords()
        {
            var words = await AllWords();
            return Ok(words);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWord(Guid id)
        {
            var word = await GetWordById(id);

            if (word == null)
            {
                return NotFound();
            }

            return Ok(word);
        }

        [HttpGet("get-word-of-the-day")]
        public async Task<IActionResult> GetWordOfTheDay()
        {
            var wordOfTheDay = await GetWotd();

            if (wordOfTheDay == null)
            {
                return NotFound();
            }

            return Ok(wordOfTheDay);
        }

        [HttpGet("get-amount-wotd")]
        public async Task<IActionResult> GetAmountWotd()
        {
            var wotd = await GetWotd();
            var amount = AllWords().Result.Where(w => w.Text == wotd).Count();

            if (amount < 1)
            {
                return NotFound();
            }

            return Ok(amount);
        }

        [HttpGet("get-amount/{id}")]
        public async Task<IActionResult> GetAmount(Guid id)
        {
            var word = await GetWordById(id);
            var amount = AllWords().Result.Where(w => w.Text == word.Text).Count();

            if(amount < 1)
            {
                return NotFound();
            }

            return Ok(amount);
        }

        [HttpGet("get-closest-words/{id}")]
        public async Task<IActionResult> ClosestWords(Guid id)
        {
            var word = await GetWordById(id);
            var closestWords = AllWords().Result.Where(w => IsClose(word.Text, w.Text))
                .Select(word => word.Text == word.Text + " - " + GetAmount(word.Id).ToString())/*.Select(word => word.Text)*/.Distinct();

            if (closestWords == null)
            {
                return Ok();
            }

            return Ok(closestWords);
            
            static bool IsClose (string word, string compare)
            {
                word = word.ToLower(); compare = compare.ToLower();
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

        [HttpPost]
        public async Task<IActionResult> PostWord(Word word)
        {
            word.AddTime = DateTime.Now;

            if (word == null)
            {
                return BadRequest();
            }

            if (_context.Words.Any(w => w.Email == word.Email))
                ModelState.AddModelError("Email", "Users with the same email address can add only one word per day!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Words.Add(word);
            await _context.SaveChangesAsync();

            return Ok(word);
        }

        public async Task<List<Word>> AllWords()
        {
            var allWords = await _context.Words.ToListAsync();
            return allWords;
        }

        public async Task<Word> GetWordById(Guid id)
        {
            var word = await _context.Words.FindAsync(id);

            if (word == null)
            {
                throw new NullReferenceException();
            }

            return word;
        }

        public async Task<String> GetWotd()
        {
            var words = await AllWords();
            var wordOfTheDay = words.GroupBy(word => word.Text).OrderByDescending(el => el.Count()).First().Key;

            return wordOfTheDay;
        }
    }
}
