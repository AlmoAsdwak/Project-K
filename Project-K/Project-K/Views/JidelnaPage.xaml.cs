using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net.Http;
using System.Text.Json;
using Project_K.Services;
using System.Net;
using Project_K.Models;

namespace Project_K.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JidelnaPage : ContentPage
    {
        public JidelnaPage()
        {
            InitializeComponent();
            var JidelnaSid = SecureStorage.GetAsync("JidelnaSid").Result;
            if (JidelnaSid == null)
            {
                LoginText.IsVisible = true;
                loginForm.IsVisible = true;
                submitForm.IsVisible = true;
                return;
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    JidlaView.ItemsSource = GetPapu.Jidla;
                });
                JidlaView.IsVisible = true;
            }

        }

        private void loginButton_Clicked(object sender, System.EventArgs e)
        {
            if (usernameEntry.Text == null || passwordEntry.Text == null)
            {
                DisplayAlert("Error", "You must fill all fields", "OK");
                return;
            }
            HttpClient client = new HttpClient();
            var data = new { userId = usernameEntry.Text, password = passwordEntry.Text };
            var dataJson = JsonSerializer.Serialize(data);
            var response = client.PostAsync("https://jidelna.farhive.cz/api/v1/Login", new StringContent(dataJson, System.Text.Encoding.UTF8, "application/json")).Result;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                DisplayAlert("Error", "Wrong username or password", "OK");
                return;
            }
            var responseString = response.Content.ReadAsStringAsync().Result;
            var dataJson2 = JsonSerializer.Deserialize<DataJson>(responseString);
            SecureStorage.SetAsync("JidelnaSid", dataJson2.sid).Wait();
            if (rememberMe.IsChecked)
            {
                SecureStorage.SetAsync("JidelnaUsername", usernameEntry.Text).Wait();
                SecureStorage.SetAsync("JidelnaPassword", passwordEntry.Text).Wait();
            }
            Shell.Current.GoToAsync("//JidelnaPage").Wait();
        }
        class DataJson
        {
            public string sid { get; set; }
        }

        public class Rootobject
        {
            public Day[] days { get; set; }
        }

        public class Day
        {
            public string date { get; set; }
            public Food[] foods { get; set; }
        }

        public class Food
        {
            public int type { get; set; }
            public string name { get; set; }
            public bool ordered { get; set; }
        }

    }
}