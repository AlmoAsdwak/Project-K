using Kyberna_k.ViewModel;
using Project_K.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Android.App.Assist.AssistStructure;

namespace Project_K.Views
{
    public partial class RozvrhPage : ContentPage
    {
        public static int Days = 0;
        private ViewModel viewModel;
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
            viewModel = new ViewModel();
            BindingContext = viewModel;
            Day.Text = GetDate();
            RozvrhRefresh.Command = new Command(() =>
            {
                GetRozvrh.RefreshRozvrh();
                Day.Text = GetDate();
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
            DayOfWeek now = DateTime.Now.AddDays(Days).DayOfWeek;
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

        private void ButtonAdder(object sender, EventArgs e)
        {
            viewModel.IsLoading = true;
            Days++;
            GetRozvrh.RefreshRozvrh();
            Day.Text = GetDate();
            viewModel.IsLoading = false;
        }
        private void ButtonSubtracter(object sender, EventArgs e)
        {
            viewModel.IsLoading = true;
            Days--;
            GetRozvrh.RefreshRozvrh();
            Day.Text = GetDate();
            viewModel.IsLoading = false;
        }
    }
}