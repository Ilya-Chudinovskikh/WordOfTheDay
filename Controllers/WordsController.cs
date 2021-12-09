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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutWord(Guid id, Word word)
        {
            if (id != word.Id)
            {
                return BadRequest();
            }

            _context.Entry(word).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WordExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostWord(Word word)
        {
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
            //return CreatedAtAction(nameof(GetWord), new { id = word.Id }, word);
            return Ok(word);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWord(Guid id)
        {
            var word = await _context.Words.FindAsync(id);
            if (word == null)
            {
                return NotFound();
            }

            _context.Words.Remove(word);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WordExists(Guid id)
        {
            return _context.Words.Any(e => e.Id == id);
        }
    }
}