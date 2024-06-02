using Project_K.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Cell = Project_K.Models.Cell;

namespace Project_K.Services
{
    public static class GetRozvrh
    {
        public static ObservableCollection<Cell> Rozvrh { get; private set; } = new ObservableCollection<Cell>(); 
        static GetRozvrh() => RefreshRozvrh();

        public static void RefreshRozvrh()
        {
            HttpClient client = new HttpClient();
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
                var response = client.PostAsync("https://sis.ssakhk.cz/api/v1/getTimeTableByUserId",
                    new StringContent(dataJson, System.Text.Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode) return;
                var responseString = response.Content.ReadAsStringAsync().Result;
                var dataJson2 = JsonSerializer.Deserialize<DataJson>(responseString);
                if (dataJson2 == null) return;
                ObservableCollection<Cell> tempRozvrh = new ObservableCollection<Cell>();
                Device.BeginInvokeOnMainThread(() =>
                {
                    Rozvrh.Clear();

                    foreach (var cell in dataJson2.Cells.OrderBy(x => x.StartTime))
                    {
                        cell.TimeTo = cell.UnformattedStartTime.AddMinutes(45).ToString("HH:mm");
                        tempRozvrh.Add(cell);
                    }
                    int startCount = tempRozvrh.Count-1;
                    for (var i = 0; i < startCount; i++)
                    {
                        var item = TimeSpan.Parse(tempRozvrh[i].TimeTo);
                        var itemnext = TimeSpan.Parse(tempRozvrh[i + 1].FormattedStartTime);
                        var compare = itemnext.TotalMinutes - item.TotalMinutes;
                        if (compare < 35) continue;
                        var tmp = new Cell
                        {
                            UnformattedStartTime = tempRozvrh[i].UnformattedStartTime.AddMinutes(45),
                            TimeTo = tempRozvrh[i + 1].FormattedStartTime,
                            Subject = "Volno"
                        };
                        tempRozvrh.Add(tmp);

                    }

                    foreach (var cell in tempRozvrh.OrderBy(x=>x.UnformattedStartTime))
                        Rozvrh.Add(cell);
                });
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}