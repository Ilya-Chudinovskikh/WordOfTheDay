using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WordOfTheDay.Repository.Entities;
using System.Threading;
using WordOfTheDay.Domain;
using System.Diagnostics;
using WordOfTheDay.Api.Models;

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

        [HttpGet("get-word-of-the-day")]
        public async Task<IActionResult> GetWordOfTheDay()
        {
            var tuple = await WordsServices.WordOfTheDay(_context);

            WordCount wordOfTheDay = new(tuple.Item1, tuple.Item2);

            if (wordOfTheDay == null)
            {
                return NotFound();
            }

            return Ok(wordOfTheDay);
        }

        

        [HttpPost]
        public async Task<IActionResult> PostWord(Word word)
        {
            var watch = new Stopwatch();
            watch.Start();

            if (word == null)
            {
                return BadRequest();
            }

            if (await WordsServices.IsAlreadyExist(word, _context))
                ModelState.AddModelError("Email", "Users with the same email address can add only one word per day!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await WordsServices.PostWord(word, _context);

            watch.Stop();
            Console.WriteLine($"PostWord: {watch.ElapsedMilliseconds}");

            return Ok(word);
        }
    }
}

