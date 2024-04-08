using System;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Project_K.Services;
using Project_K.Views;

namespace Project_K.ViewModels
{
    public class RozvrhViewModel : BaseViewModel
    {
        public Command RefreshRozvrh { get; }
        public RozvrhViewModel()
        {
            Title = "Osobní Rozvrh";
            RefreshRozvrh = new Command(async () => await Call());
      //      OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }
        async Task Call()
        {
            IsBusy = true;
            GetRozvrh.RefreshRozvrh();
            IsBusy = false;
        }
        public ICommand OpenWebCommand { get; }
    }
}