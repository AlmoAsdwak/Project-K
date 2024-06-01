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
        public static ObservableCollection<JidlaModels.Day> Jidla { get; private set; } = new ObservableCollection<JidlaModels.Day>();
        public static string RefreshJidlo()
        {
            HttpClient client = new HttpClient();
            var data = new { sid = SecureStorage.GetAsync("JidelnaSid").Result };
            var dataJson = JsonSerializer.Serialize(data);
            var response = client.PostAsync("https://jidelna.farhive.cz/api/v1/Orders", new StringContent(dataJson, Encoding.UTF8, "application/json")).Result;
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                SecureStorage.Remove("JidelnaSid");
                var errorcode = LoginJidlo();
                switch(errorcode)
                {
                    case "0":
                        var exitcode = RefreshJidlo();
                        return exitcode;
                    case "1":
                        return "1";
                    case "2":
                        SecureStorage.Remove("JidelnaUsername");
                        SecureStorage.Remove("JidelnaPassword");
                        return "2";
                }
            }
            var responseString = response.Content.ReadAsStringAsync().Result;
            var dataJson2 = JsonSerializer.Deserialize<JidlaModels.Rootobject>(responseString);
            if (dataJson2 == null) return "3";

            List<JidlaModels.Day> list = dataJson2.days.ToList();
            Jidla = new ObservableCollection<JidlaModels.Day>(list);
            return "0";
        }
        public static string LoginJidlo()
        {
            var username = SecureStorage.GetAsync("JidelnaUsername").Result;
            var password = SecureStorage.GetAsync("JidelnaPassword").Result;
            if (username == null || password == null)
                return "1";
            HttpClient client = new HttpClient();
            var data = new { userId = username, password };
            var dataJson = JsonSerializer.Serialize(data);
            var response = client.PostAsync("https://jidelna.farhive.cz/api/v1/Login", new StringContent(dataJson, Encoding.UTF8, "application/json")).Result;
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return "2";
            var responseString = response.Content.ReadAsStringAsync().Result;
            var dataJson2 = JsonSerializer.Deserialize<DataJsonSID>(responseString);
            SecureStorage.SetAsync("JidelnaSid", dataJson2.sid).Wait();
            return "0";
        }
        class DataJsonSID
        {
            public string sid { get; set; }
        }
    }
}
