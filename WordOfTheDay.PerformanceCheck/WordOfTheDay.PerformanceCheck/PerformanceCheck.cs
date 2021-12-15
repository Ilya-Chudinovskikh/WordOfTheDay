using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace WordOfTheDay.PerformanceCheck
{
    class PerformanceCheck
    {
        static void Main()
        {
            Request request = new(1, @"C:\Users\Илья\OneDrive\Документы\check.txt");
            Console.WriteLine(Request.MakeRequest(Request.GetRandomEmail(), Request.GetRandomWord(Request.GetWords(request))).Result);

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
            public static async Task<string> MakeRequest(string _email, string _text)
            {
                WebRequest request = WebRequest.Create("https://localhost:5001/api/words");
                string postData = @"{{""email"" : "_email"}, {""text"" : "_text"}}";
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                StreamWriter requestWriter = new(request.GetRequestStream());
                requestWriter.Write(postData);
                WebResponse response = await request.GetResponseAsync();
                string result = null;

                using (Stream stream = response.GetResponseStream())
                {
                    using StreamReader reader = new(stream);
                    result = reader.ReadToEnd();
                }
                response.Close();

                return result;
            }
        }
    }
}
