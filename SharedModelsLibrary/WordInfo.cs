using System;

namespace SharedModelsLibrary
{
    public class WordInfo
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public DateTime AddTime { get; set; }
        public double LocationLongitude { get; set; }
        public double LocationLatitude { get; set; }
        public WordInfo(Guid id, string email, string text, DateTime addTime, double locationLongitude, double locationLatitude)
        {
            Id = id;
            Email = email;
            Text = text;
            AddTime = addTime;
            LocationLongitude = locationLongitude;
            LocationLatitude = locationLatitude;
        }
    }
}
