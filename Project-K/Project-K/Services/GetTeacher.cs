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
        public static IList<TeacherCell> Teacher { get; private set; }
        public static string TeacherRefresh()
        {
            try
            {
                var teacher = Views.UcitelPickerPage.teacherRealName;
                if (teacher == null)
                    return "noteacherselected";
                HttpClient client = new HttpClient();
                var data = new { Teacher = teacher };
                var dataJson = JsonSerializer.Serialize(data);
                var response = client.PostAsync("https://whoisalmo.cz/api/school/kdeucitel",
                new StringContent(dataJson, Encoding.UTF8, "application/json")).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                if (responseString == "[]")
                    return "ucitelneuci";
                var dataJson2 = JsonSerializer.Deserialize<TeacherCell[]>(responseString,new JsonSerializerOptions() { PropertyNameCaseInsensitive=true});
                Teacher = dataJson2.OrderBy(cell => cell.FormattedStartTime).ToList();

                return "good";
            }
            catch (Exception)
            {
                Teacher = null;
                return "nointernet";
            }
        }
    }
}
