using System;

namespace Project_K.Models
{
    public class TeacherDatas
    {
        public TeacherCell[] Cells { get; set; }
    }
    public class TeacherCell
    {
        private DateTime _startTime;
        public string ClassRoom { get; set; }
        public string Subject { get; set; }
        public string GroupName { get; set; }
        public string StartTime
        {
            get => _startTime.ToString("HH:mm:ss");
            set
            {
                if (DateTime.TryParse(value, out DateTime startTime))
                    _startTime = startTime;
            }
        }
        public string MeetUrl { get; set; }
        public string Teacher { get; set; }
        public string FormattedStartTime => _startTime.ToString("HH:mm");
        public string TimeTo => _startTime.AddMinutes(45).ToString("HH:mm");
    }
}
