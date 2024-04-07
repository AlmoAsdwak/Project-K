using Project_K.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace Project_K.Services
{
    public static class GetRozvrh
    {
        public static IList<Models.Cell> Rozvrh { get; private set; }
        static GetRozvrh()
        {
            
            var storage = SecureStorage.GetAsync("Id").Result;
            int id = Convert.ToInt32(storage);
            DateTime date = DateTime.Now;
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return;
            HttpClient client = new HttpClient();
            var data = new { userid = id.ToString(), dateTime = date };
            var dataJson = JsonSerializer.Serialize(data);
            var response = client.PostAsync("https://sis.ssakhk.cz/api/v1/getTimeTableByUserId",
                new StringContent(dataJson, System.Text.Encoding.UTF8, "application/json")).Result;
            if (!response.IsSuccessStatusCode) return;
            var responseString = response.Content.ReadAsStringAsync().Result;
            var dataJson2 = JsonSerializer.Deserialize<DataJson>(responseString);
            if (dataJson2 == null) return;
            Rozvrh = dataJson2.Cells.OrderBy(cell => cell.StartTime).ToList();

        }
    }
}
