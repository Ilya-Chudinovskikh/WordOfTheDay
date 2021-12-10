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
    [Route("api/Words")]
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
            var words = await _context.Words.ToListAsync();
            return Ok(words);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWord(Guid id)
        {
            var word = await _context.Words.FindAsync(id);

            if (word == null)
            {
                return NotFound();
            }

            return Ok(word);
        }


        [HttpPost]
        public async Task<IActionResult> PostWord(Word word)
        {
            word.AddTime = DateTime.Now;

            if (word == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_context.Words.Any(w => w.Email == word.Email))
                return BadRequest(ModelState);

            _context.Words.Add(word);
            await _context.SaveChangesAsync();

            return Ok(word);
        }
    }
}