using Project_K.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xamarin.Forms;

namespace Project_K.Services
{
    public static class GetTeacher
    {
        public static ObservableCollection<TeacherCell> Teacher { get; private set; } = new ObservableCollection<TeacherCell>();
        static GetTeacher() => TeacherRefresh();
        public static int TeacherRefresh()
        {
            try
            {
                Teacher.Clear();
                var teacher = Views.UcitelPickerPage.teacherRealName;
                if (teacher == null)
                    return 1;
                DateTime date = DateTime.Now.AddDays(Views.RozvrhPage.Days);
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    return 2;
                HttpClient client = new HttpClient();
                var data = new { teacherAbbreviation = teacher, dateTime = date.Date };
                var dataJson = JsonSerializer.Serialize(data);
                var response = client.PostAsync("https://sis.ssakhk.cz/api/v1/getTimeTableByTeacherAbbreviation",
                new StringContent(dataJson, Encoding.UTF8, "application/json")).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                if (responseString == null)
                    return 2;
                var dataJson2 = JsonSerializer.Deserialize<TeacherDatas>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                foreach (var item in dataJson2.Cells.OrderBy(cell => cell.FormattedStartTime))
                    Teacher.Add(item);

                return 0;
            }
            catch (Exception)
            {
                Teacher = new ObservableCollection<TeacherCell>();
                return 3;
            }
        }

    }
}
