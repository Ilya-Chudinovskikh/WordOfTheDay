using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Diagnostics;
using System.Threading;

namespace WordOfTheDay.PerformanceCheck
{
    public class Requester
    {
        private int AmountOfQueries { get; set; }
        private string WordsFile { get; set; }
        public Requester(int amountOfQueries, string wordsFile)
        {
            AmountOfQueries = amountOfQueries;
            WordsFile = wordsFile;
        }
        private List<string> GetWords()
        {
            var words = File.ReadAllText(WordsFile).Split().ToList();

            words.RemoveAll(x => x == "");

            return words;
        }
        private static string GetRandomWord(List<string> words)
        {
            Random random = new();

            var randomString = words[random.Next(0, words.Count - 1)];

            return randomString;
        }
        private static string GetRandomEmail()
        {
            Random random = new();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var domains = new string[] { "@email.com", "@itransition.com", "@yandex.ru" };
            var email = string.Empty;
            var domain = domains[random.Next(domains.Length)];

            for (int i = 0; i < random.Next(6, 13); i++)
                email += chars[random.Next(chars.Length)].ToString().ToLower();

            return $"{email}{domain}";
        }
        private List<Word> MakeWords()
        {
            var words = new List<Word> { };
            var wordsList = GetWords();
            for (int i = 0; i < AmountOfQueries; i++)
            {
                words.Add(new Word { Email = GetRandomEmail(), Text = GetRandomWord(wordsList) });
            }
            return words;
        }
        private async Task<List<float>> MakeAllRequests()
        {
            using var httpClient = new HttpClient();
            var tasks = new List<Task>();
            var measurements = new List<float>();
            var url = "https://localhost:5001/api/words";
            var maxThreads = 10;
            var watch = new Stopwatch();
            var throttler = new SemaphoreSlim(initialCount: maxThreads);

            foreach (var newWord in MakeWords())
            {
                await throttler.WaitAsync();
                watch.Start();
                tasks.Add(
                    Task.Run(async () =>
                    {
                        try
                        {
                            await httpClient.PostAsJsonAsync(url, newWord);
                        }
                        finally
                        {
                            throttler.Release();
                        }
                    }));
                watch.Stop();
                measurements.Add(watch.ElapsedMilliseconds);
            }
            await Task.WhenAll(tasks);
            return measurements;
        }
        public async Task ShowTimeMeasuruments()
        {
            var measurements = await MakeAllRequests();
            Console.WriteLine($"Max time for request: {measurements.Max()}");
            Console.WriteLine($"Min time for request: {measurements.Min()}");
            Console.WriteLine($"Average time for request: {measurements.Average()}");
            Console.WriteLine($"Total elapsed time: {measurements.Sum()} per {measurements.Count}");
        }
    }
}
