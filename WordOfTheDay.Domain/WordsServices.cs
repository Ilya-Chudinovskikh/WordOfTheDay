using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository;
using WordOfTheDay.Repository.Models;
using FreeGeoIPCore;
using Microsoft.AspNetCore.Http;
using FreeGeoIPCore.AppCode;
using Microsoft.Extensions.Primitives;

namespace WordOfTheDay.Domain
{
    internal sealed class WordsServices : IWordsServices
    {
        private readonly IWordsRepository _wordsRepository;
        public WordsServices (IWordsRepository wordsRepository)
        {
            _wordsRepository = wordsRepository;
        }
        
        public Task<WordCount> WordOfTheDay()
        {
            var wordOfTheDay = _wordsRepository.WordOfTheDay();

            return wordOfTheDay;
        }
        
        public Task<IEnumerable<WordCount>> CloseWords(string email)
        {
            var closeWords = _wordsRepository.CloseWords(email.ToLower());

            return closeWords;
        }
        public Task PostWord(Word word)
        {
            word.AddTime = DateTime.Today.ToUniversalTime();

            word.Text = word.Text.ToLower();
            word.Email = word.Email.ToLower();

            return _wordsRepository.PostWord(word);
        }
        public Task<WordCount> UserWord(string email)
        {
            var userWord = _wordsRepository.UserWord(email.ToLower());

            return userWord;

        }
        public Task<bool> IsAlreadyExist(Word word)
        {
            var exist = _wordsRepository.IsAlreadyExist(word);

            return exist;
        }
    }
}
