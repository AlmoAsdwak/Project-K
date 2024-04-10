using System;

namespace Project_K.Models
{
    public class DataJson
    {
        public Cell[] Cells { get; set; }
    }
    public class Cell
    {
        private DateTime _startTime;
        public string GroupName { get; set; }
        public string Subject { get; set; }
        public string Teacher { get; set; }
        public string ClassRoom { get; set; }
        public string MeetUrl { get; set; }
        public string StartTime
        {
            get { return _startTime.ToString("yyyy-MM-ddTHH:mm:ss"); }
            set
            {
                if (DateTime.TryParse(value, out DateTime startTime))
                    _startTime = startTime;
            }
        }
        public string FormattedStartTime => _startTime.ToString("HH:mm");
        public string TimeTo => _startTime.AddMinutes(45).ToString("HH:mm");
    }
}
