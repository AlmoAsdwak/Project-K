using Kyberna_k.ViewModel;
using Project_K.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Project_K.Views
{
    public partial class RozvrhPage : ContentPage
    {
        public static int Days = 0;
        private ViewModel viewModel;
        public RozvrhPage()
        {
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Day.Text = GetDate();
        }
        protected override void OnDisappearing() => viewModel.IsLoading = false;
        async void OnPrivacyPolicyButtonClicked(object sender, EventArgs e) => await Launcher.OpenAsync("https://whoisalmo.cz/soukromi");

        public static string GetDate()
        {
            switch (DateTime.Now.AddDays(Days).DayOfWeek)
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

        private async void ButtonAdder(object sender, EventArgs e)
        {
            viewModel.IsLoading = true;
            Days++;
            await Task.Run(() => GetRozvrh.RefreshRozvrh());
            Day.Text = GetDate();
            viewModel.IsLoading = false;
        }
        private async void ButtonSubtracter(object sender, EventArgs e)
        {
            viewModel.IsLoading = true;
            Days--;
            await Task.Run(() => GetRozvrh.RefreshRozvrh());
            Day.Text = GetDate();
            viewModel.IsLoading = false;
        }
    }
}