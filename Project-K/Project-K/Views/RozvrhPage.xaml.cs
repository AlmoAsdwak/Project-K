using Kyberna_k.ViewModel;
using Project_K.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Project_K.Views
{
    public partial class RozvrhPage : ContentPage
    {
        readonly static HttpClient client = new HttpClient();
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
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<object, string>(this, "DisplayAlert");
            viewModel.IsLoading = false;
        }
        [Obsolete]
        void OnPrivacyPolicyButtonClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("http://whoisalmo.cz/soukromi"));
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