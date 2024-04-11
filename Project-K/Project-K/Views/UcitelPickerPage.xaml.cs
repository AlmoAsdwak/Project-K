using Kyberna_k.ViewModel;
using Project_K.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.App.Assist.AssistStructure;

namespace Project_K.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UcitelPickerPage : ContentPage
    {
        public static string teacherRealName;
        private ViewModel viewModel;
        public UcitelPickerPage()
        {
            Appearing += OnPageAppearing;
            InitializeComponent();
            viewModel = new ViewModel();
            BindingContext = viewModel;
        }
        protected override bool OnBackButtonPressed()
        {
            if (!AcceptButton.IsVisible)
                ResetView();
            else
                Shell.Current.GoToAsync("//RozvrhPage").Wait();
            return true;
        }
        private void OnPageAppearing(object sender, EventArgs e) => ResetView();
        private void ResetView()
        {
            PickerOfTeachers.IsVisible = true;
            AcceptButton.IsVisible = true;
            label1.IsVisible = true;
            TeacherView.IsVisible = false;
            TeacherName.Text = null;
            TeacherName.IsVisible = false;
            TeacherView.ItemsSource = null;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            viewModel.IsLoading = true;
            var selectedItem = PickerOfTeachers.SelectedItem as string;
            if (selectedItem != null)
            {
                Dictionary<string, string> teachers = new Dictionary<string, string>
                {
                    { "Jan Lang", "LA" },
                    { "Richard Rejthar", "RE" },
                    { "Matěj Lang", "LN" },
                    { "Libor Beneš", "BE" },
                    { "Michaela Ducháčková", "DU" },
                    { "Milan Hloušek", "HS" },
                    { "Jaroslav Hývl", "HY" },
                    { "Zlata Karpíšková", "KR" },
                    { "Eva Kuntová", "KT" },
                    { "Roman Loskot", "LO" },
                    { "Jaroslav Maťátko", "MA" },
                    { "Martin Mercl", "MC" },
                    { "Štěpán Mach", "MH" },
                    { "Josef Matějus", "MJ" },
                    { "Martin Markoš", "MR" },
                    { "Ilona Mayerová", "MV" },
                    { "Jiří Petera", "PE" },
                    { "Miloslav Penc", "PN" },
                    { "Lucie Porter", "PO" },
                    { "David Podzimek", "PZ" },
                    { "Jana Radoňová", "RA" },
                    { "Igor Ročín", "RO" },
                    { "Jiří Špičan", "SP" },
                    { "Simona Trnková", "TK" },
                    { "Tomáš Záhořík", "TR" },
                    { "Pavel Trnka", "TZ" },
                    { "Josef Zelba", "ZE" },
                    { "Šárka Žemličková", "ZM" },
                    { "Shane Hertnon", "CI" },
                    { "Michal Janko", "JA" },
                    { "Tadeáš Němec", "NE" },
                    { "Michal Macinka", "MI" },

                };
                bool selection = teachers.TryGetValue(selectedItem, out teacherRealName);
                if (!selection)
                {
                    DisplayAlert("Něco je špatně", $"Nenašli jsme učitele, prosím kontaktujte vývojáře", "OK");
                    return;
                }
                string result = await Task.Run(() => GetTeacher.TeacherRefresh());
                if (result != "good")
                {
                    if (result == "noteacherselected")
                        await DisplayAlert("Varování", $"Teacher je null neco se stalo idk wtf pls posli mi to dik<3", "OK");
                    if (result == "ucitelneuci")
                        await DisplayAlert("Varování", $"Učitel dneska neučí", "OK");
                    if (result == "nointernet")
                        await DisplayAlert("Varování", $"Není připojení k internetu", "OK");
                    ResetView();
                    return;
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    TeacherView.ItemsSource = GetTeacher.Teacher;
                });
                PickerOfTeachers.IsVisible = false;
                AcceptButton.IsVisible = false;
                label1.IsVisible = false;
                TeacherView.IsVisible = true;
                TeacherName.Text = selectedItem;
                TeacherName.IsVisible = true;
                viewModel.IsLoading = false;
            }
        }
    }
}

