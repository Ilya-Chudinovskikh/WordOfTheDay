using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Diagnostics;
using WordOfTheDay.Entities;

namespace PerformanceCheck
{
    class PerformanceCheck
    {
        static async Task Main()
        {
            Request request = new(20, @"C:\Users\Илья\OneDrive\Документы\check.txt");
            await request.MinMaxAvgTime();
        }
    }
    public class Request
    {
        int AmountOfQueries { get; set; }
        string WordsFile { get; set; }
        public Request(int amountOfQueries, string wordsFile)
        {
            AmountOfQueries = amountOfQueries;
            WordsFile = wordsFile;
        }
        public List<string> GetWords()
        {
            var words = new List<string>();

            using (StreamReader sr = new(WordsFile))
            {
                words = sr.ReadToEnd().Split().ToList();
            }

            return words;
        }
        public static string GetRandomWord(List<string> words)
        {
            Random random = new();

            string randomString = words[random.Next(0, words.Count)];

            return randomString;
        }
        public static string GetRandomEmail()
        {
            Random random = new();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string[] emails = { "@email.com", "@itransition.com", "@yandex.ru" };
            string email = null;

            for (int i = 0; i < random.Next(6, 13); i++)
                email += chars[random.Next(chars.Length)].ToString().ToLower();

            return email += emails[random.Next(emails.Length)];
        }
        public static async Task MakeRequest(string email, string text)
        {
            var url = "https://localhost:5001/api/words";
            using var httpClient = new HttpClient();
            var newWord = new Word() { Email = email, Text = text };
            var response = await httpClient.PostAsJsonAsync(url, newWord);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Request failed.");
            }
        }
        public async Task<List<float>> MeasureRequestTime()
        {
            var measurements = new List<float>();
            for (int i = 0; i < AmountOfQueries; i++)
            {
                var watch = new Stopwatch();
                string email = GetRandomEmail();
                string text = GetRandomWord(GetWords());
                watch.Start();
                await MakeRequest(email, text);
                watch.Stop();
                measurements.Add(watch.ElapsedMilliseconds);
            }
            return measurements;
        }
        public async Task MinMaxAvgTime()
        {
            var measurements = await MeasureRequestTime();
            Console.WriteLine("Max time for request: " + measurements.Max() + "\n" +
                "Min time for request: " + measurements.Min() + "\n" +
                "Average time for request: " + measurements.Average() + "\n");
        }
    }
}