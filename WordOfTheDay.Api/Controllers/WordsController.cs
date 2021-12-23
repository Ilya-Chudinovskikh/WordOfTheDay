using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WordOfTheDay.Repository.Entities;
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

        [HttpGet("get-closest-words")]
        public async Task<IActionResult> GetClosestWords()
        {
            var wordOfTheDay = await WordsServices.WordOfTheDay(_context);
            var closestWords = await WordsServices.CloseWords(_context, wordOfTheDay.Word);

            return Ok(closestWords);
        }

        [HttpPost]
        public async Task<IActionResult> PostWord(Word word)
        {
            if (word == null)
            {
                return BadRequest();
            }

            if (await WordsServices.IsAlreadyExist(word, _context))
                ModelState.AddModelError("Email", "Users with the same email address can add only one word per day!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await WordsServices.PostWord(word, _context);

            return Ok(word);
        }
    }
}

