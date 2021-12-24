using System.Collections.Generic;
using System.Threading.Tasks;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository.Models;


namespace WordOfTheDay.Domain
{
    public interface IWordsServices
    {
        Task<WordCount> WordOfTheDay();

        List<WordCount> CloseWords(string word);

        Task PostWord(Word word);

        Task<bool> IsAlreadyExist(Word word);

    }
}
