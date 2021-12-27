using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository;
using WordOfTheDay.Repository.Models;

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
        
        public Task<List<WordCount>> CloseWords(string word)
        {
            var closeWords = _wordsRepository.CloseWords(word);

            return closeWords;
        }
        public Task PostWord(Word word)
        {
            word.AddTime = DateTime.UtcNow;

            return _wordsRepository.PostWord(word);
        }
        public Task<WordCount> UserWord(string email)
        {
            var userWord = _wordsRepository.UserWord(email);

            return userWord;

        }
        public Task<bool> IsAlreadyExist(Word word)
        {
            var exist = _wordsRepository.IsAlreadyExist(word);

            return exist;
        }
    }
}
