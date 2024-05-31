using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Project_K.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Project_K.Services
{
    internal class GetPapu
    {
        public static ObservableCollection<Models.JidlaModels.Day> Jidla { get; private set; } = new ObservableCollection<Models.JidlaModels.Day>();
        static GetPapu() => RefreshJidlo();
        public static void RefreshJidlo()
        {
            HttpClient client = new HttpClient();
            var data = new { sid = SecureStorage.GetAsync("JidelnaSid").Result };
            var dataJson = JsonSerializer.Serialize(data);
            var response = client.PostAsync("https://jidelna.farhive.cz/api/v1/Orders", new StringContent(dataJson, System.Text.Encoding.UTF8, "application/json")).Result;
            if (response.StatusCode != HttpStatusCode.OK)
                return;
            var responseString = response.Content.ReadAsStringAsync().Result;
            var dataJson2 = JsonSerializer.Deserialize<JidlaModels.Rootobject>(responseString);
            if (dataJson2 == null) return;

            List<Models.JidlaModels.Day> list = dataJson2.days.ToList();
            Jidla = new ObservableCollection<JidlaModels.Day>(list);
        }
    }
}
