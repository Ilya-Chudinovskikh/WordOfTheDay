using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository;
using WordOfTheDay.Repository.Models;
using MassTransit;
using SharedModelsLibrary;

namespace WordOfTheDay.Domain
{
    internal sealed class WordsServices : IWordsServices
    {
        private readonly IWordsRepository _wordsRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        public WordsServices (IWordsRepository wordsRepository, IPublishEndpoint publishEndpoint)
        {
            _wordsRepository = wordsRepository;
            _publishEndpoint = publishEndpoint;
        }

        public Task<WordCount> WordOfTheDay()
        {
            var wordOfTheDay = _wordsRepository.WordOfTheDay();

            return wordOfTheDay;
        }
        
        public Task<List<WordCount>> CloseWords(string email)
        {
            var closeWords = _wordsRepository.CloseWords(email.ToLower());

            return closeWords;
        }
        public async Task<Word> PostWord(Word word)
        {
            word.AddTime = DateTime.Today.ToUniversalTime();

            word.Text = word.Text.ToLower();
            word.Email = word.Email.ToLower();

            await PostAndPublishWord(word);

            return word;
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
        public async Task PostAndPublishWord(Word word)
        {
            _wordsRepository.PostWord(word);

            await _publishEndpoint.Publish(new WordInfo(word.Id, word.Email, word.Text, word.AddTime, word.LocationLongitude, word.LocationLatitude));
        }
    }
}
