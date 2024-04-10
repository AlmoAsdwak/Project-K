using Project_K.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project_K.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UcebnaFinder : ContentPage
    {
        public static TimeSpan from;
        public static TimeSpan to;
        public UcebnaFinder()
        {
            Appearing += OnPageAppearing;
            InitializeComponent();
        }
        private void OnPageAppearing(object sender, EventArgs e)
        {
            ResetView();
        }
        protected override bool OnBackButtonPressed()
        {
            if (!AcceptButton.IsVisible)
                ResetView();
            else
                Shell.Current.GoToAsync("//RozvrhPage").RunSynchronously();
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
        }
        private void AcceptButton_Clicked(object sender, EventArgs e)
        {
            from = startTimePicker.Time;
            to = endTimePicker.Time;
            string result = GetClassrooms.ClassroomsRefresh();
            if (result != "good")
            {
                if (result == "badlyselected")
                    DisplayAlert("Varování", $"Špatně vybraný datum", "OK");
                if (result == "zadnavolnaucebna")
                    DisplayAlert("Varování", $"Ve vybraném čase není volná učebna", "OK");
                if (result == "nointernet")
                    DisplayAlert("Varování", $"Není připojení k internetu", "OK");
                return;
            }
            ClassesView.ItemsSource = GetClassrooms.Classes;
            label1.IsVisible = false;
            label2.IsVisible = false;
            startTimePicker.IsVisible = false;
            endTimePicker.IsVisible = false;
            AcceptButton.IsVisible = false;
            ClassesView.IsVisible = true;
        }
        void startOnTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Time")
            {
                var timePicker = sender as TimePicker;
                var time = timePicker.Time;
                if (time.Hours < 7 || time.Hours > 21)
                {
                    // Reset the time to the start of your range
                    timePicker.Time = new TimeSpan(7, 0, 0);
                }
            }
        }
        void endOnTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Time")
            {
                var timePicker = sender as TimePicker;
                var time = timePicker.Time;
                if (time.Hours < 7 || time.Hours > 21)
                {
                    // Reset the time to the start of your range
                    timePicker.Time = new TimeSpan(7, 0, 0);
                }
            }
        }
        private void startTimePicker_Unfocused(object sender, FocusEventArgs e)
        {
            endTimePicker.Focus();
        }
    }
}