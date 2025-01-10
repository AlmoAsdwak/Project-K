using Kyberna_k.ViewModel;
using Project_K.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project_K.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UcitelPickerPage : ContentPage
    {
        public static string teacherRealName;
        private ViewModel viewModel;
        public UcitelPickerPage()
        {
            InitializeComponent();
            Day.Text = RozvrhPage.GetDate();
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Day.Text = RozvrhPage.GetDate();
            ResetView();
        }
        private void ResetView()
        {
            viewModel.IsLoading = true;
            PickerOfTeachers.IsVisible = true;
            AcceptButton.IsVisible = true;
            label1.IsVisible = true;
            TeacherView.IsVisible = false;
            TeacherName.Text = null;
            TeacherName.IsVisible = false;
            TeacherView.ItemsSource = null;
            viewModel.IsLoading = false;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            string selectedItem = PickerOfTeachers.SelectedItem as string;
            if (selectedItem == null) return;
            viewModel.IsLoading = true;
            Dictionary<string, string> teachers = new Dictionary<string, string>
            {
                { "Beneš Libor", "BE" },
                { "Bezstarosti Pavel", "BZ" },
                { "Bursová Dagmar", "BS" },
                { "Ducháčková Michaela", "DU" },
                { "Filip Nathaniel", "FI" },
                { "Hertnon Shane", "CI" },
                { "Hloušek Jiří", "HU" },
                { "Hloušek Milan", "HS" },
                { "Hývl Jaroslav", "HY" },
                { "Janko Michal", "JA" },
                { "Jungová Miluše", "JU" },
                { "Karpíšková Zlata", "KR" },
                { "Kuntová Eva", "KT" },
                { "Lang Jan", "LA" },
                { "Langová Kateřina", "LV" },
                { "Langová Martina", "LG" },
                { "Lenc Tomáš", "LE" },
                { "Loskot Roman", "LO" },
                { "Mach Štěpán", "MH" },
                { "Markoš Martin", "MR" },
                { "Matějus Josef", "MJ" },
                { "Maťátko Jaroslav", "MA" },
                { "Mayerová Ilona", "MV" },
                { "Mercl Martin", "MC" },
                { "Penc Miloslav", "PN" },
                { "Petera Jiří", "PE" },
                { "Petera Ondřej", "PT" },
                { "Podzimek David", "PZ" },
                { "Porter Lucie", "PO" },
                { "Radoňová Jana", "RA" },
                { "Rejthar Richard", "RE" },
                { "Ročín Igor", "RO" },
                { "Silná Petra", "PS" },
                { "Šolc Miloš", "SO" },
                { "Špičan Jiří", "SP" },
                { "Tichý Miroslav", "TI" },
                { "Trnka Pavel", "TR" },
                { "Trnková Simona", "TK" },
                { "Záhořík Tomáš", "TZ" },
                { "Zelba Josef", "ZE" },
                { "Žemličková Šárka", "ZM" },
                { "Ženíšková Eva", "ZN" }
            };

            if (!teachers.TryGetValue(selectedItem, out teacherRealName))
            {
                await DisplayAlert("Něco je špatně", $"Nenašli jsme učitele, prosím kontaktujte vývojáře", "OK");
                ResetView();
                return;
            }
            switch (await Task.Run(() => GetTeacher.TeacherRefresh()))
            {
                case 0:
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
                    break;
                case 1:
                    await DisplayAlert("Varování", $"Teacher je null neco se stalo", "OK");
                    ResetView();
                    break;
                case 2:
                    await DisplayAlert("Varování", $"Učitel dneska neučí", "OK");
                    ResetView();
                    break;
                case 3:
                    await DisplayAlert("Varování", $"Není připojení k internetu", "OK");
                    ResetView();
                    break;

            }
        }

        private async void ButtonAdder(object sender, EventArgs e)
        {
            viewModel.IsLoading = true;
            RozvrhPage.Days++;
            Day.Text = RozvrhPage.GetDate();
            await Task.Run(() => GetTeacher.TeacherRefresh());
            viewModel.IsLoading = false;
        }
        private async void ButtonSubtracter(object sender, EventArgs e)
        {
            viewModel.IsLoading = true;
            RozvrhPage.Days--;
            Day.Text = RozvrhPage.GetDate();
            await Task.Run(() => GetTeacher.TeacherRefresh());
            viewModel.IsLoading = false;
        }
    }
}

