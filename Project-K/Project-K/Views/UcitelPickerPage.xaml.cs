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
            Device.BeginInvokeOnMainThread(() => TeacherView.ItemsSource = GetTeacher.Teacher);
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
                    { "Bezstarosti Pavel", "PB" },
                    { "Bursová Dagmar", "BS" },
                    { "Ducháčková Michaela", "DU" },
                    { "Filip Nathaniel", "NF" },
                    { "Hertnon Shane", "CI" },
                    { "Hloušek Jiří", "HJ" },
                    { "Hloušek Milan", "HS" },
                    { "Hývl Jaroslav", "HY" },
                    { "Janko Michal", "JA" },
                    { "Karpíšková Zlata", "KR" },
                    { "Kuntová Eva", "KT" },
                    { "Lang Matěj", "LN" },
                    { "Lang Jan", "LA" },
                    { "Lenc Tomáš", "LT" },
                    { "Loskot Roman", "LO" },
                    { "Mach Štěpán", "MH" },
                    { "Markoš Martin", "MR" },
                    { "Matějus Josef", "MJ" },
                    { "Maťátko Jaroslav", "MA" },
                    { "Mayerová Ilona", "MV" },
                    { "Mercl Martin", "MC" },
                    { "Macinka Michal", "MI" },
                    { "Tichý Miroslav", "TI" },
                    { "Němec Tadeáš", "NE" },
                    { "Petera Jiří", "PE" },
                    { "Petera Ondřej", "OP" },
                    { "Penc Miroslav", "PN" },
                    { "Podzimek David", "PZ" },
                    { "Porter Lucie", "PO" },
                    { "Radoňová Jana", "RA" },
                    { "Rejthar Richard", "RE" },
                    { "Ročín Igor", "RO" },
                    { "Šolc Miloš", "MS" },
                    { "Špičan Jiří", "SP" },
                    { "Trnková Simona", "TK" },
                    { "Trnka Pavel", "TR" },
                    { "Zelba Josef", "ZE" },
                    { "Záhořík Tomáš", "TZ" },
                    { "Žemličková Šárka", "ZM" },
                    { "Ženíšková Eva", "EZ" }
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

