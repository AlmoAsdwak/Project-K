using Project_K.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Project_K.Services
{
    public static class GetRozvrh
    {
        public static ObservableCollection<Models.Cell> Rozvrh { get; private set; } = new ObservableCollection<Models.Cell>();
        static GetRozvrh()
        {
            RefreshRozvrh();
        }
        readonly static HttpClient client = new HttpClient();
        public static void RefreshRozvrh()
        {
            try
            {
                Rozvrh.Clear();
                var storage = SecureStorage.GetAsync("Id").Result;
                int id = Convert.ToInt32(storage);
                DateTime date = DateTime.Now;
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
                    {
                        Rozvrh.Add(cell);
                    }
                });
                Check();
            }
            catch (Exception)
            {

                return;
            }   
        }
        public static async Task Check()
        {
            var result = client.GetAsync("https://whoisalmo.cz/api/school/check").Result;
            if (App.version != result.Content.ReadAsStringAsync().Result)
            {
                var dummySender = new object();
                MessagingCenter.Send<object, string>(dummySender, "DisplayAlert", $"Máte starou verzi");
            }
            await Task.CompletedTask;
        }
    }
}
