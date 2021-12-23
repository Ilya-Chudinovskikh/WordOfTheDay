using System.Linq;
using System.Threading.Tasks;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository.Models;

namespace WordOfTheDay.Repository
{
    public interface IWordsRepository
    {
        Task<WordCount> WordOfTheDay();

        Task<int> CountWord(string text);

        IQueryable<Word> CloseWords(string word);

        Task PostWord(Word word);

        Task<bool> IsAlreadyExist(Word word);
    }
}
