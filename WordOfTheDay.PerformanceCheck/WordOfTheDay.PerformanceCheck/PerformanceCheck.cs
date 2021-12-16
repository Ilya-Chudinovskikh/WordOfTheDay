using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;
using WordOfTheDay.Entities;

namespace PerformanceCheck
{
    class PerformanceCheck
    {
        static async Task Main()
        {
            Request request = new(1, @"C:\Users\Илья\OneDrive\Документы\check.txt");
            await Request.MakeRequest(Request.GetRandomEmail(), Request.GetRandomWord(Request.GetWords(request)));
        }
        public class Request
        {
            int amountOfQueries { get; set; }
            string wordsFile { get; set; }
            public Request(int _amountOfQueries, string _wordsFile)
            {
                amountOfQueries = _amountOfQueries;
                wordsFile = _wordsFile;
            }
            public static List<string> GetWords(Request request)
            {
                var words = new List<string>();

                using (StreamReader sr = new(request.wordsFile))
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
                var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                using (var httpClient = new HttpClient())
                {
                    var newWord = new Word() { Email = email, Text = text };
                    var response = await httpClient.PostAsJsonAsync(url, newWord);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Request failed.");
                    }
                }
            }
        }
    }
}
