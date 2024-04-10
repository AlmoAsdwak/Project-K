using System;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Project_K.Services;
using Project_K.Views;
using System.Net.Cache;
using System.Net.Http;

namespace Project_K.ViewModels
{
    public class RozvrhViewModel : BaseViewModel
    {
        public Command RefreshRozvrh { get; }
        public RozvrhViewModel()
        {
            Title = "Osobní Rozvrh";
            RefreshRozvrh = new Command(async () => await Call());
        }
        async Task Call()
        {
            IsBusy = true;
            GetRozvrh.RefreshRozvrh();
            IsBusy = false;
            await Task.CompletedTask;
        }
    }
}