using Project_K.Services;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Kyberna_k.ViewModel;
namespace Project_K.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UcebnaFinder : ContentPage
    {
        public static TimeSpan from;
        public static TimeSpan to;
        private ViewModel viewModel;
        public UcebnaFinder()
        {
            InitializeComponent();
            viewModel = new ViewModel();
            BindingContext = viewModel;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ResetView();
        }
        protected override bool OnBackButtonPressed()
        {
            if (!AcceptButton.IsVisible)
                ResetView();
            else
                Shell.Current.GoToAsync("//RozvrhPage").Wait();
            return true;
        }
        private void ResetView()
        {
            label1.IsVisible = true;
            label2.IsVisible = true;
            startTimePicker.IsVisible = true;
            endTimePicker.IsVisible = true;
            AcceptButton.IsVisible = true;
            ClassesView.IsVisible = false;
            MainLabel.IsVisible = false;
            viewModel.IsLoading = false;
        }
        private async void AcceptButton_Clicked(object sender, EventArgs e)
        {
            viewModel.IsLoading = true;
            from = startTimePicker.Time;
            to = endTimePicker.Time;
            string result = await Task.Run(() => GetClassrooms.ClassroomsRefresh());
            if (result != "good")
            {
                if (result == "badlyselected")
                    await DisplayAlert("Varování", $"Špatně vybraný datum", "OK");
                if (result == "zadnavolnaucebna")
                    await DisplayAlert("Varování", $"Ve vybraném čase není volná učebna", "OK");
                if (result == "nointernet")
                    await DisplayAlert("Varování", $"Není připojení k internetu", "OK");
                ResetView();
                return;
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                ClassesView.ItemsSource = GetClassrooms.Classes;
            });

            label1.IsVisible = false;
            label2.IsVisible = false;
            startTimePicker.IsVisible = false;
            endTimePicker.IsVisible = false;
            AcceptButton.IsVisible = false;
            ClassesView.IsVisible = true;
            MainLabel.IsVisible = true;
            viewModel.IsLoading = false;
        }
        void startOnTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Time")
            {
                var timePicker = sender as TimePicker;
                var time = timePicker.Time;
                if (time.Hours < 7 || time.Hours > 21)
                    timePicker.Time = new TimeSpan(7, 0, 0);
            }
            if (args.PropertyName == TimePicker.TimeProperty.PropertyName)
                endTimePicker.Focus();
        }
        void endOnTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Time")
            {
                var timePicker = sender as TimePicker;
                var time = timePicker.Time;
                if (time.Hours < 7 || time.Hours > 21)
                    timePicker.Time = new TimeSpan(21, 0, 0);
            }
        }
    }
}