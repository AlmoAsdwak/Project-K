using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project_K.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage() =>
            _ = InitializeLoginPageAsync();
        private async Task InitializeLoginPageAsync()
        {
            var Id = await SecureStorage.GetAsync("Id");
            if (Id != null)
            {
                var confirmed = await DisplayAlert("Logout Confirmation", "Jste si jisti že se chcete odhlásit?", "Ano", "Ne");
                if (!confirmed)
                {
                    Application.Current.MainPage = new AppShell();
                    return;
                }
            }
            SecureStorage.RemoveAll();
            Shell.SetTabBarIsVisible(this, false);
            InitializeComponent();
            scannerView.OnScanResult += Result;
        }
        private async void OnPrivacyPolicyButtonClicked(object sender, EventArgs e) =>
            await Launcher.OpenAsync(new Uri("http://whoisalmo.cz/soukromi"));

        private void Result(ZXing.Result result) =>
            Device.BeginInvokeOnMainThread(() =>
            {
                if (result != null && result.BarcodeFormat == ZXing.BarcodeFormat.QR_CODE)
                    Label.Text = result.Text;
            });

        private void HelpButton_Clicked(object sender, EventArgs e) =>
            Label.Text = "Musíš jít do školního informačního systému, do svého profilu, úplně dolů. Kde načteš svůj QR kód.";

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!Regex.Match(Label.Text, "{\"FamilyName\":\"[\\p{L}´ˇ]*\",\"GivenName\":\"[\\p{L}´ˇ]*\",\"Email\":\"\\w+.\\w+@ssakhk.cz\",\"UserId\":\\d+}").Success)
            {
                DisplayAlert("Wrong format", "Youre QR code have bad format", "OK");
                return;
            }

            var Informations = JsonSerializer.Deserialize<LoginInformation>(Label.Text);

            SecureStorage.SetAsync("FamilyName", Informations.FamilyName).Wait();
            SecureStorage.SetAsync("GivenName", Informations.GivenName).Wait();
            SecureStorage.SetAsync("Mail", Informations.Email).Wait();
            SecureStorage.SetAsync("Id", Informations.UserId.ToString()).Wait();

            Application.Current.MainPage = new AppShell();
        }
        class LoginInformation
        {
            public string FamilyName { get; set; }
            public string GivenName { get; set; }
            public string Email { get; set; }
            public int UserId { get; set; }
        }
    }
}