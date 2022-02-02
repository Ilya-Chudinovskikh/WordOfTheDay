using System.Collections.Generic;
using System.Threading.Tasks;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository.Models;


namespace WordOfTheDay.Domain
{
    public interface IWordsServices
    {
        Task<WordCount> WordOfTheDay();

        Task<List<WordCount>> CloseWords(string word);

        Task<Word> PostWord(Word word);

        Task<WordCount> UserWord(string email);

        Task<bool> IsAlreadyExist(Word word);

        Task PostAndPublishWord(Word word);
    }
}
