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
            var pattern = "{\\\"FamilyName\\\":\\\"\\w+\\\",\\\"GivenName\\\":\\\"\\w+\\\",\\\"Email\\\":\\\"\\w+\\W\\w+@ssakhk\\.cz\\\",\\\"UserId\\\":\\d+}";
            var match = Regex.Match(Label.Text, pattern);
            if (!match.Success)
            {
                DisplayAlert("Wrong format", "Youre QR code have bad format", "OK");
                return;
            }
            SecureStorage.SetAsync("User", Label.Text).Wait();
            var userData = SecureStorage.GetAsync("User").Result;
            Debug.WriteLine($"UserData: {userData}");

            Process.GetCurrentProcess().Kill();
        }
    }

}