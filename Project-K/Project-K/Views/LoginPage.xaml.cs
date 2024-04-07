using Project_K.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UIKit;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace Project_K.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeLoginPageAsync();
        }

        private async Task InitializeLoginPageAsync()
        {
            var Id = await SecureStorage.GetAsync("Id");
            if (Id != null)
            {
                var confirmed = await DisplayAlert("Logout Confirmation", "Jste si jisti že se chcete odhlásit?", "Ano", "Ne");
                if (!confirmed)
                {
                    App.Current.MainPage = new AppShell();
                    return;
                }
            }

            SecureStorage.RemoveAll();
            Shell.SetTabBarIsVisible(this, false);
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
            scannerView.OnScanResult += Result;
        }

        public void Result(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (result != null && result.BarcodeFormat == ZXing.BarcodeFormat.QR_CODE)
                {
                    // Display QR code content
                    Label.Text = result.Text;

                }
            });
        }

        private void HelpButton_Clicked(object sender, EventArgs e)
        {
            Label.Text = "Musíš jít do školního informačního systému, do svého profilu, úplně dolů. Kde načteš svůj QR kód.";
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var pattern = "{\"FamilyName\":\"[\\p{L}´ˇ]*\",\"GivenName\":\"[\\p{L}´ˇ]*\",\"Email\":\"\\w+.\\w+@ssakhk.cz\",\"UserId\":\\d+}";
            var match = Regex.Match(Label.Text, pattern);
            if (!match.Success)
            {
                DisplayAlert("Wrong format", "Youre QR code have bad format", "OK");
                return;
            }

            var GetFamilyName = Regex.Match(Label.Text, "(?<=\"FamilyName\":\")[\\p{L}´ˇ]*");
            var GetGivenName = Regex.Match(Label.Text, "(?<=\"GivenName\":\")[\\p{L}´ˇ]*");
            var GetMail = Regex.Match(Label.Text, "(?<=\\\"Email\\\":\")\\w+.\\w+@ssakhk.cz");
            var GetId = Regex.Match(Label.Text, "(?<=\"UserId\":)\\d+");

            SecureStorage.SetAsync("FamilyName", GetFamilyName.ToString()).Wait();
            SecureStorage.SetAsync("GivenName", GetGivenName.ToString()).Wait();
            SecureStorage.SetAsync("Mail", GetMail.ToString()).Wait();
            SecureStorage.SetAsync("Id", GetId.ToString()).Wait();

            var userData = SecureStorage.GetAsync("Id").Result;
            if (userData == null)
            {
                DisplayAlert("Wrong format", "Youre QR code have bad format", "OK");
                SecureStorage.RemoveAll();
                return;
            }

            App.Current.MainPage = new AppShell();
        }
    }

}