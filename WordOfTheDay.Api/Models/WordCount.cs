using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordOfTheDay.Api.Models
{
    public class WordCount
    {
        public string Word { get; set; }
        public int Count { get; set; }
        public WordCount(string word, int count)
        {
            Word = word;
            Count = count;
        }
    }
}
