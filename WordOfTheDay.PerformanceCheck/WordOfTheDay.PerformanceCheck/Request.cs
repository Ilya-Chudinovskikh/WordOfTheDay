﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Diagnostics;

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
        private static async Task MakeRequest(string email, string text, HttpClient httpClient)
        {
            var url = "https://localhost:5001/api/words";
            var newWord = new Word { Email = email, Text = text };
            await httpClient.PostAsJsonAsync(url, newWord);
        }
        private async Task<List<float>> MeasureRequestTime()
        {
            using var httpClient = new HttpClient();
            var wordsList = GetWords();
            var measurements = new List<float>();
            for (int i = 0; i < AmountOfQueries; i++)
            {
                var email = GetRandomEmail();
                var text = GetRandomWord(wordsList);
                var watch = new Stopwatch();
                watch.Start();
                await MakeRequest(email, text, httpClient);
                watch.Stop();
                measurements.Add(watch.ElapsedMilliseconds);
            }
            return measurements;
        }
        public async Task ShowTimeMeasuruments()
        {
            var measurements = await MeasureRequestTime();
            Console.WriteLine($"Max time for request: {measurements.Max()}");
            Console.WriteLine($"Min time for request: {measurements.Min()}");
            Console.WriteLine($"Average time for request: {measurements.Average()}");
            Console.WriteLine($"Total elapsed time: {measurements.Sum()} per {measurements.Count}");
        }
    }
}
