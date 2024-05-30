using System;
using System.Collections.Generic;

namespace Project_K.Models
{
    public class DataJson
    {
        public List<Cell> Cells { get; set; }
    }

    public class Cell
    {
        public DateTime UnformattedStartTime;
        public string GroupName { get; set; }
        public string Subject { get; set; }
        public string Teacher { get; set; }
        public string ClassRoom { get; set; }
        public string MeetUrl { get; set; }
        public string TimeTo { get; set; }

        public string StartTime
        {
            get => UnformattedStartTime.ToString("yyyy-MM-ddTHH:mm:ss");
            set
            {
                if (DateTime.TryParse(value, out DateTime startTime))
                    UnformattedStartTime = startTime;
            }
        }

        public string FormattedStartTime
        {
            get => UnformattedStartTime.ToString("HH:mm");
            set
            {
                if (DateTime.TryParse(value, out DateTime formattedStartTime))
                    UnformattedStartTime = formattedStartTime;

            }
        }
    }
}