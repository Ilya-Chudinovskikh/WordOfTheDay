using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Domain;
using MassTransit;
using SharedModelsLibrary;

namespace WordOfTheDay.Api.Controllers
{
    [Route("api/words")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly IWordsServices _wordsServices;
        private readonly IPublishEndpoint _publishEndpoint;
        public WordsController(IWordsServices wordsServices, IPublishEndpoint publishEndpoint)
        {
            _wordsServices = wordsServices;
            _publishEndpoint = publishEndpoint;
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

        [HttpGet("get-closest-words/{email}")]
        public async Task<IActionResult> GetClosestWords(string email)
        {
            var closestWords = await _wordsServices.CloseWords(email);

            return Ok(closestWords);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserWord(string email)
        {
            var userWord = await _wordsServices.UserWord(email);

            return Ok(userWord);
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

            await _publishEndpoint.Publish(new WordInfo(word.Id, word.Email, word.Text, word.AddTime, word.LocationLongitude, word.LocationLatitude));

            return Ok(word);
        }
    }
}
