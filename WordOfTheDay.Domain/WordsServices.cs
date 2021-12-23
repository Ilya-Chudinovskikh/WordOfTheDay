using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository;
using WordOfTheDay.Repository.Models;

namespace WordOfTheDay.Domain
{
    public class WordsServices : IWordsServices
    {
        private readonly IWordsRepository _iWordsRepository;
        public WordsServices (IWordsRepository iWordsRepository)
        {
            _iWordsRepository = iWordsRepository;
        }
        
        public Task<WordCount> WordOfTheDay()
        {
            var wordOfTheDay = _iWordsRepository.WordOfTheDay();

            return wordOfTheDay;
        }
        
        public async Task<List<WordCount>> CloseWords(string word)
        {
            var closeWords = _iWordsRepository.CloseWords(word);
            var closeWordCounts = new List<WordCount>();

            foreach (var w in closeWords)
            {
                closeWordCounts.Add(new WordCount(w.Text, await _iWordsRepository.CountWord(w.Text)));
            }

            return closeWordCounts;
        }
        public Task PostWord(Word word)
        {
            word.AddTime = DateTime.Now;

            return _iWordsRepository.PostWord(word);
        }
        public Task<bool> IsAlreadyExist(Word word)
        {
            var exist = _iWordsRepository.IsAlreadyExist(word);

            return exist;
        }
    }
}
