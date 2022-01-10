using System;
using Xunit;
using Moq;
using WordOfTheDay.Domain;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository;

namespace WordOfTheDay.Tests
{
    public class WordsServicesTests
    {
        [Fact]
        public async void PostWord_Test()
        {
            var word = new Word { Id = Guid.NewGuid(), Text = "Ddd", Email = "aSd@abc" };

            var wordsRepository = new Mock<IWordsRepository>();

            var wordsSevices = new WordsServices(wordsRepository.Object);

            await wordsSevices.PostWord(word);

            Assert.NotNull(word.AddTime);
            Assert.Equal("ddd", word.Text);
            Assert.Equal("asd@abc", word.Email);
        }
    }
}
