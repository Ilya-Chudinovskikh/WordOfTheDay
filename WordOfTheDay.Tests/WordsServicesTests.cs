using System;
using Xunit;
using Moq;
using WordOfTheDay.Domain;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository;
using MassTransit;

namespace WordOfTheDay.Tests
{
    public class WordsServicesTests
    {
        [Fact]
        public async void PostWord_Test()
        {
            var word = new Word { Id = Guid.NewGuid(), Text = "Ddd", Email = "aSd@abc" };

            var wordsRepository = new Mock<IWordsRepository>();

            var publishEndpoint = new Mock<IPublishEndpoint>();

            var wordsSevices = new WordsServices(wordsRepository.Object, publishEndpoint.Object);

            await wordsSevices.PostWord(word);

            Assert.NotNull(word.AddTime);
            Assert.NotEqual(0, word.LocationLongitude);
            Assert.NotEqual(0, word.LocationLatitude);
            Assert.Equal("ddd", word.Text);
            Assert.Equal("asd@abc", word.Email);
        }
        [Fact]
        public void MockLocation_Test()
        {
            var location = WordsServices.MockLocation();
            var longtitude = location.longtitude;
            var latitude = location.latitude;

            Assert.InRange(longtitude, -180, 180);
            Assert.InRange(latitude, -90, 90);
        }
    }
}
