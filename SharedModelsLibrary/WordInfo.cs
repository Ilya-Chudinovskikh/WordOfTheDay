using System;

namespace SharedModelsLibrary
{
    public class WordInfo
    {
        public string Email { get; set; }
        public string Text { get; set; }
        public DateTime AddTime { get; set; }
        public double LocationLongitude { get; set; }
        public double LocationLatitude { get; set; }
        public WordInfo(string email, string text, DateTime addTime, double locationLongitude, double locationLatitude)
        {
            Email = email;
            Text = text;
            AddTime = addTime;
            LocationLongitude = locationLongitude;
            LocationLatitude = locationLatitude;
        }
    }
}
