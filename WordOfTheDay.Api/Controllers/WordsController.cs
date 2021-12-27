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
        private readonly IWordsServices _wordsServices;
        public WordsController(IWordsServices wordsServices)
        {
            _wordsServices = wordsServices;
        }

        [HttpGet("get-word-of-the-day")]
        public async Task<IActionResult> GetWordOfTheDay()
        {
            var wordOfTheDay = await _wordsServices.WordOfTheDay();

            if (wordOfTheDay == null)
            {
                return NotFound();
            }

            return Ok(wordOfTheDay);
        }

        [HttpGet("get-closest-words")]
        public async Task<IActionResult> GetClosestWords()
        {
            var wordOfTheDay = await _wordsServices.WordOfTheDay();
            var closestWords = _wordsServices.CloseWords(wordOfTheDay.Word);

            return Ok(closestWords);
        }

        [HttpPost]
        public async Task<IActionResult> PostWord(Word word)
        {
            if (word == null)
            {
                return BadRequest();
            }

            if (await _wordsServices.IsAlreadyExist(word))
                ModelState.AddModelError("Email", "Users with the same email address can add only one word per day!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _wordsServices.PostWord(word);

            return Ok(word);
        }
    }
}
