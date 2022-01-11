using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordOfTheDay.Repository.Entities;
using WordOfTheDay.Repository;
using WordOfTheDay.Repository.Models;
using FreeGeoIPCore;
using Microsoft.AspNetCore.Http;
using FreeGeoIPCore.AppCode;
using Microsoft.Extensions.Primitives;

namespace WordOfTheDay.Domain
{
    internal sealed class WordsServices : IWordsServices
    {
        private readonly IWordsRepository _wordsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WordsServices (IWordsRepository wordsRepository)
        {
            _wordsRepository = wordsRepository;
        }
        
        public Task<WordCount> WordOfTheDay()
        {
            var wordOfTheDay = _wordsRepository.WordOfTheDay();

            return wordOfTheDay;
        }
        
        public Task<List<WordCount>> CloseWords(string email)
        {
            var closeWords = _wordsRepository.CloseWords(email.ToLower());

            return closeWords;
        }
        public async Task<Task> PostWord(Word word)
        {
            word.AddTime = DateTime.UtcNow;

            word.Text = word.Text.ToLower();
            word.Email = word.Email.ToLower();

            var location = await GetLocation();
            word.LocationLongitude = location.Item1;
            word.LocationLatitude = location.Item2;

            return _wordsRepository.PostWord(word);
        }
        public Task<WordCount> UserWord(string email)
        {
            var userWord = _wordsRepository.UserWord(email.ToLower());

            return userWord;

        }
        public Task<bool> IsAlreadyExist(Word word)
        {
            var exist = _wordsRepository.IsAlreadyExist(word);

            return exist;
        }
        public async Task<(double, double)> GetLocation()
        {
            var ipClient = new FreeGeoIPClient();

            var ipAddress = GetRequestIP(_httpContextAccessor);

            if (ipAddress == null)
                return (0, 0);

            var location = await ipClient.GetLocation(ipAddress);

            var geoLocation = (location.Longitude, location.Latitude);

            return geoLocation;
        }
        public static string GetRequestIP(IHttpContextAccessor httpContextAccessor, bool tryUseXForwardHeader = true)
        {
            var ip = String.Empty;

            if (tryUseXForwardHeader)
                ip = GetHeaderValueAs<string>(httpContextAccessor, "X-Forwarded-For").SplitCsv().FirstOrDefault();

            if (ip.IsNullOrWhitespace() && httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>(httpContextAccessor, "REMOTE_ADDR");


            if (ip.IsNullOrWhitespace())
                return null;

            ip = ip.Substring(0, ip.IndexOf(":"));

            return ip;
        }
        public static T GetHeaderValueAs<T>(IHttpContextAccessor httpContextAccessor, string headerName)
        {
            var values = new StringValues();

            if (httpContextAccessor?.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                var rawValues = values.ToString();   

                if (!rawValues.IsNullOrWhitespace())
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default;
        }
    }
}
