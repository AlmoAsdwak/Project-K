using Project_K.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Project_K.Views
{
    public partial class RozvrhPage : ContentPage
    {
        public RozvrhPage()
        {
            MessagingCenter.Subscribe<object, string>(this, "DisplayAlert", async (sender, arg) =>
            {
                var result = await DisplayAlert("Varování", arg, "Ano","Ne");
                if (result)
                {
                    await Browser.OpenAsync("https://whoisalmo.cz/RozvrhAPP",BrowserLaunchMode.SystemPreferred);
                }
            });
            InitializeComponent();
            Day.Text = GetDate();
            RozvrhRefresh.Command = new Command(() =>
            {
                GetRozvrh.RefreshRozvrh();
                RozvrhRefresh.IsRefreshing = false;
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<object, string>(this, "DisplayAlert");
        }
        private string GetDate()
        {
            DayOfWeek now = DateTime.Now.DayOfWeek;
            switch (now)
            {
                case DayOfWeek.Monday: return "Pondělí";
                case DayOfWeek.Tuesday: return "Úterý";
                case DayOfWeek.Wednesday: return "Středa";
                case DayOfWeek.Thursday: return "Čtvrtek";
                case DayOfWeek.Friday: return "Pátek";
                case DayOfWeek.Saturday: return "Sobota\nNení rozvrh";
                case DayOfWeek.Sunday: return "Neděle\nNení rozvrh";
                default: return string.Empty;
            }
        }
    }
}