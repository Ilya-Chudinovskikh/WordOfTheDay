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
        private readonly IWordsServices _iWordsServices;
        public WordsController(IWordsServices iWordsServices)
        {
            _iWordsServices = iWordsServices;
        }

        [HttpGet("get-word-of-the-day")]
        public async Task<IActionResult> GetWordOfTheDay()
        {
            var wordOfTheDay = await _iWordsServices.WordOfTheDay();

            if (wordOfTheDay == null)
            {
                return NotFound();
            }

            return Ok(wordOfTheDay);
        }

        [HttpGet("get-closest-words")]
        public async Task<IActionResult> GetClosestWords()
        {
            var wordOfTheDay = await _iWordsServices.WordOfTheDay();
            var closestWords = await _iWordsServices.CloseWords(wordOfTheDay.Word);

            return Ok(closestWords);
        }

        [HttpPost]
        public async Task<IActionResult> PostWord(Word word)
        {
            if (word == null)
            {
                return BadRequest();
            }

            if (await _iWordsServices.IsAlreadyExist(word))
                ModelState.AddModelError("Email", "Users with the same email address can add only one word per day!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _iWordsServices.PostWord(word);

            return Ok(word);
        }
    }
}
