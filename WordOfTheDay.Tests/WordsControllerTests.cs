using System;
using Xunit;
using Moq;
using WordOfTheDay.Api.Controllers;
using WordOfTheDay.Domain;
using WordOfTheDay.Repository.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WordOfTheDay.Tests
{
    public class WordsControllerTests
    {
        [Fact]
        public async void PostWord_Null_BadRequest_Test()
        {
            var word = new Word();

            word = null;

            var wordsServices = new Mock<IWordsServices>();

            var wordsController = new WordsController(wordsServices.Object);

            var result = await wordsController.PostWord(word);

            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async void GetWordOfTheDay_Null_NotFound_Test()
        {
            var wordsServices = new Mock<IWordsServices>();

            var wordsController = new WordsController(wordsServices.Object);

            var result = await wordsController.GetWordOfTheDay();

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
