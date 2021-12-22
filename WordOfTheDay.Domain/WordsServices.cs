using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository;
using WordOfTheDay.Repository.Models;

namespace WordOfTheDay.Domain
{
    public static class WordsServices
    {
        
        public static  Task<WordCount> WordOfTheDay(WordContext context)
        {
            var wordOfTheDay = WordsRepository.WordOfTheDay(context);

            return wordOfTheDay;
        }
        
        public static async Task<List<WordCount>> CloseWords(WordContext context, string word)
        {
            var closeWords = WordsRepository.CloseWords(context, word);
            var closeWordCounts = new List<WordCount>();

            foreach (var w in closeWords)
            {
                closeWordCounts.Add(new WordCount(w.Text, await WordsRepository.CountWord(context, w.Text)));
            }

            return closeWordCounts;
        }
        public static async Task PostWord(Word word, WordContext context)
        {
            word.AddTime = DateTime.Now;

            await WordsRepository.PostWord(word, context);
        }
        public static Task<bool> IsAlreadyExist(Word word, WordContext context)
        {
            var exist = WordsRepository.IsAlreadyExist(word, context);

            return exist;
        }
    }
}

