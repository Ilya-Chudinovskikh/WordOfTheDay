using System;
using Xunit;
using Moq;
using WordOfTheDay.Repository;
using WordOfTheDay.Api.Controllers;
using WordOfTheDay.Domain;
using System.Collections.Generic;
using WordOfTheDay.Repository.Entities;
using System.Linq;
using MockQueryable.Moq;
using Microsoft.AspNetCore.Mvc;

namespace WordOfTheDay.Tests
{
    public class WordsRepositoryTests
    {
        private static List<Word> Words 
        {
            get
            {
                int date = 11;
                return new List<Word>
                {
                    new Word {Id = Guid.NewGuid(), Text = "abc", Email = "123@abc", AddTime = new DateTime(2022, 1, date, 10, 10, 10) },
                    new Word {Id = Guid.NewGuid(), Text = "wsx", Email = "1234@abc", AddTime = new DateTime(2022, 1, date, 10, 10, 11)},
                    new Word {Id = Guid.NewGuid(), Text = "qaz", Email = "12345@abc", AddTime = new DateTime(2022, 1, date, 10, 10, 12)},
                    new Word {Id = Guid.NewGuid(), Text = "abc", Email = "wsx@abc", AddTime = new DateTime(2022, 1, date, 10, 10, 19)},
                    new Word {Id = Guid.NewGuid(), Text = "qaz", Email = "qaz@abc", AddTime = new DateTime(2022, 1, date, 10, 10, 20)},
                    new Word {Id = Guid.NewGuid(), Text = "qaz", Email = "qwe@abc", AddTime = new DateTime(2022, 1, date, 10, 09, 10)},
                    new Word {Id = Guid.NewGuid(), Text = "ddd", Email = "asd@abc", AddTime = new DateTime(2022, 1, date, 09, 10, 10)}
                };
            }
        }
        [Fact]
        public async void WordOfTheDay_Test()
        {
            var words = Words.AsQueryable();

            var set = words.BuildMockDbSet();

            var context = new Mock<WordContext>();
            context.Setup(c => c.Words).Returns(set.Object);

            var wordsRepository = new WordsRepository(context.Object);

            var wordofTheDay = await wordsRepository.WordOfTheDay();

            Assert.Equal(3, wordofTheDay.Count);
            Assert.Equal("qaz", wordofTheDay.Word);
        }
        [Fact]
        public async void UserWord_Test()
        {
            var words = Words.AsQueryable();

            var email = "123@abc";

            var set = words.BuildMockDbSet();

            var context = new Mock<WordContext>();
            context.Setup(c => c.Words).Returns(set.Object);

            var wordsRepository = new WordsRepository(context.Object);

            var userWord = await wordsRepository.UserWord(email);

            Assert.Equal("abc", userWord.Word);
            Assert.Equal(2, userWord.Count);
        }
        [Fact]
        public void GetKeysTest()
        {
            var word = "abc";

            var listKeys = WordsRepository.GetKeys(word);

            Assert.Equal(3, listKeys.Count);
            Assert.Equal("%bc", listKeys[0]);
            Assert.Equal("a%c", listKeys[1]);
            Assert.Equal("ab%", listKeys[2]);
        }
        [Fact]
        public void LaterThan_Extensions_Test()
        {
            var today = DateTime.Today.ToUniversalTime();

            var query = Words.AsQueryable();

            var newWords = Words;
            newWords.Add(new Word { Id = Guid.NewGuid(), Text = "abc", Email = "123@abc", AddTime = new DateTime(2022, 1, 9, 10, 10, 10) });
            var newQuery = newWords.AsQueryable();

            var laterThanQuery = query.LaterThan(today);

            Assert.Equal(query, laterThanQuery);
            Assert.NotEqual(newQuery, laterThanQuery);
        }
    }
}
