using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project_K.Services
{
    public static class GetClassrooms
    {
        public static IList<string> Classes { get; private set; }
        public static async Task<int> ClassroomsRefresh()
        {
            try
            {
                var fromtime = Views.UcebnaFinder.from;
                var totime = Views.UcebnaFinder.to;
                if (fromtime == null || totime == null)
                    return 1;
                List<string> pattern = new List<string> {
                    "202",
                    "205",
                    "206",
                    "208",
                    "211",
                    "213",
                    "214",
                    "215",
                    "302",
                    "306",
                    "309",
                    "317",
                    "320",
                    "403",
                    "406",
                    "409",
                    "B13"
                };
                var today = DateTime.Today;
                var from = today.Add(fromtime).AddDays(Views.RozvrhPage.Days);
                HttpClient client = new HttpClient();
                var howlong = Convert.ToInt32(today.Add(totime).AddDays(Views.RozvrhPage.Days).Subtract(from).TotalMinutes);

                HashSet<string> writeline = new HashSet<string>(pattern);
                StringBuilder classes = new StringBuilder();
                int howmuch = howlong / 45;
                var tasks = new List<Task<string>>();
                for (int i = 1; i <= howmuch; i++)
                    tasks.Add(getClassroomAsync(from.AddMinutes(45 * i).ToString(), client));
                tasks.Add(getClassroomAsync(from.AddMinutes(45 * howmuch + howlong % 45).ToString(), client));
                foreach (var result in await Task.WhenAll(tasks))
                    classes.Append(result);
                foreach (var item in pattern)
                    foreach (Match m in Regex.Matches(classes.ToString(), item))
                        writeline.Remove(item);
                if (writeline.Count <= 0)
                    return 2;

                Classes = writeline.ToList();
                return 0;
            }
            catch (Exception)
            {
                Classes = null;
                return 3;
            }
        }
        public static async Task<string> getClassroomAsync(string dt, HttpClient client) => await (await client.PostAsync("https://sis.ssakhk.cz/api/v1/getClassRoomsStates", new StringContent(JsonSerializer.Serialize(new { dateTime = dt }), Encoding.UTF8, "application/json"))).Content.ReadAsStringAsync();

    }
}
