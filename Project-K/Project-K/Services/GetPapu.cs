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
        public static string lasterrorcode = "";
        public static ObservableCollection<JidlaModels.Day> Jidla { get; private set; } = new ObservableCollection<JidlaModels.Day>();
        static GetPapu() => RefreshJidlo();
        public static string RefreshJidlo()
        {
            HttpClient client = new HttpClient();
            try
            {
                var data = new { sid = SecureStorage.GetAsync("JidelnaSid").Result };
                var dataJson = JsonSerializer.Serialize(data);
                var response = client.PostAsync("https://jidelna.farhive.cz/api/v1/Orders", new StringContent(dataJson, Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    SecureStorage.Remove("JidelnaSid");
                    var errorcode = LoginJidlo();
                    switch (errorcode)
                    {
                        case "0":
                            var exitcode = RefreshJidlo();
                            lasterrorcode = "0";
                            return exitcode;
                        case "1":
                            lasterrorcode = "1";
                            return "1";
                        case "2":
                            SecureStorage.Remove("JidelnaUsername");
                            SecureStorage.Remove("JidelnaPassword");
                            lasterrorcode = "2";
                            return "2";
                    }
                }
                var responseString = response.Content.ReadAsStringAsync().Result;
                var dataJson2 = JsonSerializer.Deserialize<JidlaModels.Rootobject>(responseString);
                if (dataJson2 == null)
                {
                    lasterrorcode = "3";
                    return "3";
                }
                Jidla = new ObservableCollection<JidlaModels.Day>(dataJson2.days.ToList());
                lasterrorcode = "0";
                return "0";
            }
            catch (System.Exception)
            {
                lasterrorcode = "3";
                return "3";
            }
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
