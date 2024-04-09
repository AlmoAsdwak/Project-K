using Project_K.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Project_K.Services
{
    public static class GetClassrooms
    {
        public static IList<string> Classes { get; private set; }
        static GetClassrooms()
        {

        }
        public static string ClassroomsRefresh()
        {
            try
            {

                var fromtime = Views.UcebnaFinder.from;
                var totime = Views.UcebnaFinder.to;
                if (fromtime == null || totime == null)
                    return "badlyselected";
                var today = DateTime.Today;
                var from = today.Add(fromtime).ToString("MM.dd.yyyy HH:mm");
                var to = today.Add(totime).ToString("MM.dd.yyyy HH:mm");
                HttpClient client = new HttpClient();
                var data = new { From = from, To = to };
                var dataJson = JsonSerializer.Serialize(data);
                var response = client.PostAsync("https://whoisalmo.cz/api/school/volneucebny",
                new StringContent(dataJson, Encoding.UTF8, "application/json")).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                if (responseString == "[]")
                    return "zadnavolnaucebna";
                var dataJson2 = JsonSerializer.Deserialize<string[]>(responseString);
                Classes = dataJson2.ToList();
                return "good";
            }
            catch (Exception)
            {
                Classes = null;
                return "nointernet";
            }
        }
    }
}
