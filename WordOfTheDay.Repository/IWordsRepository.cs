using System.Collections.Generic;
using System.Threading.Tasks;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository.Models;

namespace WordOfTheDay.Repository
{
    public interface IWordsRepository
    {
        Task<WordCount> WordOfTheDay();

        Task<IEnumerable<WordCount>> CloseWords(string word);

        Task PostWord(Word word);

        Task<WordCount> UserWord(string email);

        Task<bool> IsAlreadyExist(Word word);
    }
}
