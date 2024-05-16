using Project_K.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Project_K.Services
{
    public static class GetRozvrh
    {
        public static ObservableCollection<Models.Cell> Rozvrh { get; private set; } = new ObservableCollection<Models.Cell>();
        readonly static HttpClient client = new HttpClient();
        static GetRozvrh() => RefreshRozvrh();
        public static void RefreshRozvrh()
        {
            try
            {
                Rozvrh.Clear();
                var storage = SecureStorage.GetAsync("Id").Result;
                int id = Convert.ToInt32(storage);
                DateTime date = DateTime.Now.AddDays(Views.RozvrhPage.Days);
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    return;
                var data = new { userid = id.ToString(), dateTime = date };
                var dataJson = JsonSerializer.Serialize(data);
                var response = client.PostAsync("https://sis.ssakhk.cz/api/v1/getTimeTableByUserId", new StringContent(dataJson, System.Text.Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode) return;
                var responseString = response.Content.ReadAsStringAsync().Result;
                var dataJson2 = JsonSerializer.Deserialize<DataJson>(responseString);
                if (dataJson2 == null) return;

                Device.BeginInvokeOnMainThread(() =>
                {
                    Rozvrh.Clear();
                    foreach (var cell in dataJson2.Cells.OrderBy(cell => cell.StartTime))
                        Rozvrh.Add(cell);
                    for (int i = 0; i < Rozvrh.Count - 2; i++)
                    {
                        var idk2 = TimeSpan.Parse(Rozvrh[i].TimeTo);
                        var idk1 = TimeSpan.Parse(Rozvrh[i + 1].FormattedStartTime);
                        var time = idk1.Subtract(idk2).TotalMinutes;
                        if (time >= 35)
                        {
                            if (Rozvrh[i+1].Subject == "Volno") continue;
                            Models.Cell schoolbreak = new Models.Cell();
                            schoolbreak.StartTime = Rozvrh[i].TimeTo;
                            schoolbreak.Subject = "Volno";
                            Rozvrh.Insert(i, schoolbreak);
                        }
                    }
                });

            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
