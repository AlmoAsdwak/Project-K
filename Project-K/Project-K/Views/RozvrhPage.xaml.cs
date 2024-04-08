using Project_K.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project_K.Views
{
    public partial class RozvrhPage : ContentPage
    {
        public RozvrhPage()
        {
            MessagingCenter.Subscribe<object, string>(this, "DisplayAlert", async (sender, arg) =>
            {
                await DisplayAlert("Varování", arg, "OK");
            });
            InitializeComponent();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<object, string>(this, "DisplayAlert");
        }

    }
}