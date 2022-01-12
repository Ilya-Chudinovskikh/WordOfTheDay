
namespace WordOfTheDay.Repository.Models
{
    public class WordCount
    {
        public string Word { get; set; }
        public int Count { get; set; }
        public WordCount() { }
        public WordCount(string word, int count)
        {
            Word = word;
            Count = count;
        }
    }
}
