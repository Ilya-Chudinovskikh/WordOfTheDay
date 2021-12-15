using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WordOfTheDay.Entities;
using System.Threading;
using WordOfTheDay.Domain;

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
            var words = await WordsServices.GetWords(_context);

            return Ok(words);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWord(Guid id)
        {
            var word = await WordsServices.GetWord(id, _context);

            if (word == null)
            {
                return NotFound();
            }

            return Ok(word);
        }

        [HttpGet("get-word-of-the-day")]
        public async Task<IActionResult> GetWordOfTheDay()
        {
            var wordOfTheDay = await WordsServices.WordOfTheDay(_context);

            if (wordOfTheDay == null)
            {
                return NotFound();
            }

            return Ok(wordOfTheDay);
        }

        [HttpGet("get-amount-word-of-the-day")]
        public async Task<IActionResult> GetAmountWordOfTheDay()
        {
            long amount = await WordsServices.GetAmountWordOfTheDay(_context);

            if (amount < 1)
            {
                return NotFound();
            }

            return Ok(amount);
        }

        [HttpGet("get-amount/{id}")]
        public async Task<IActionResult> GetAmount(Guid id)
        {
            var amount = await WordsServices.GetAmount(id, _context);

            if(amount < 1)
            {
                return NotFound();
            }

            return Ok(amount);
        }

        [HttpGet("get-closest-words/{id}")]
        public async Task<IActionResult> ClosestWords(Guid id)
        {
            var closestWords = await WordsServices.ClosestWords(id, _context);

            return Ok(closestWords);
        }

        [HttpPost]
        public async Task<IActionResult> PostWord(Word word)
        {

            if (word == null)
            {
                return BadRequest();
            }

            if (await WordsServices.AlreadyExist(word, _context))
                ModelState.AddModelError("Email", "Users with the same email address can add only one word per day!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await WordsServices.PostWord(word, _context);

            return Ok(word);
        }
    }
}
