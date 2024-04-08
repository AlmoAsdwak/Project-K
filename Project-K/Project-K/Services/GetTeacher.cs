using Project_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Project_K.Services
{
    public static class GetTeacher
    {
        public static IList<Models.TeacherCell> Teacher { get; private set; }
        static GetTeacher()
        {
        }
        public static void TeacherRefresh()
        {
            var teacher = Views.UcitelPickerPage.teacherRealName;
            if (teacher == null)
                return;
            HttpClient client = new HttpClient();
            var data = new { Teacher = teacher };
            var dataJson = JsonSerializer.Serialize(data);
            var response = client.PostAsync("https://whoisalmo.cz/api/school/kdeucitel",
            new StringContent(dataJson, Encoding.UTF8, "application/json")).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            var dataJson2 = JsonSerializer.Deserialize<TeacherDatas>(responseString);
            Teacher = dataJson2.Cells.OrderBy(cell => cell.FormattedStartTime).ToList();
        }
    }
}
