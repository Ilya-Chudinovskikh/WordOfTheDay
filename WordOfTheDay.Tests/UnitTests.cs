using System;
using Xunit;
using Moq;
using WordOfTheDay.Repository;
using WordOfTheDay.Api.Controllers;
using WordOfTheDay.Domain;
using System.Collections.Generic;
using WordOfTheDay.Repository.Models;
using WordOfTheDay.Repository.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data.Entity.Infrastructure;
using MockQueryable.EntityFrameworkCore;

namespace WordOfTheDay.Tests
{
    public class UnitTests
    {
        [Fact]
        public void WordsOfTheDayTest()
        {
            var words = new List<Word>
            {
                new Word {Id = Guid.NewGuid(), Text = "abc", Email = "123@abc"},
                new Word {Id = Guid.NewGuid(), Text = "wsx", Email = "1234@abc"},
                new Word {Id = Guid.NewGuid(), Text = "qaz", Email = "12345@abc"},
                new Word {Id = Guid.NewGuid(), Text = "abc", Email = "wsx@abc"},
                new Word {Id = Guid.NewGuid(), Text = "qaz", Email = "qaz@abc"},
                new Word {Id = Guid.NewGuid(), Text = "qaz", Email = "qwe@abc"},
                new Word {Id = Guid.NewGuid(), Text = "ddd", Email = "asd@abc"}
            }.AsQueryable();

            var set = new Mock<DbSet<Word>>();

            set.As<IQueryable<Word>>().Setup(w => w.Provider).Returns(words.Provider);
            set.As<IQueryable<Word>>().Setup(w => w.Expression).Returns(words.Expression);
            set.As<IQueryable<Word>>().Setup(w => w.ElementType).Returns(words.ElementType);
            set.As<IQueryable<Word>>().Setup(w => w.GetEnumerator()).Returns(words.GetEnumerator());

            var context = new Mock<WordContext>();
            context.Setup(w => w.Words).Returns(set.Object);

            var wordsRepository = new WordsRepository(context.Object);

            var wordofTheDay = wordsRepository.WordOfTheDay().Result;

            Assert.Equal(3, wordofTheDay.Count);
            Assert.Equal("qaz", wordofTheDay.Word);
        }
    }
}
