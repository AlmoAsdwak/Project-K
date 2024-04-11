using System;
using System.Text.Json.Serialization;

namespace Project_K.Models
{
    public class TeacherDatas
    {
        public TeacherCell[] Cells { get; set; }
    }
    public class TeacherCell
    {
        private DateTime _startTime;
        //[JsonPropertyName("classRoom")]
        public string ClassRoom { get; set; }
        //[JsonPropertyName("subjectName")]
        public string SubjectName { get; set; }
        public string StudyGroupName { get; set; }
        public string Time
        {
            get => _startTime.ToString("HH:mm:ss");
            set
            {
                if (DateTime.TryParse(value, out DateTime startTime))
                    _startTime = startTime;
            }
        }
        public string Teacher { get; set; }
        public string FormattedStartTime => _startTime.ToString("HH:mm");
        public string TimeTo => _startTime.AddMinutes(45).ToString("HH:mm");
    }
}
