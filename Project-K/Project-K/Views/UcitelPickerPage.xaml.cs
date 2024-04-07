using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project_K.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UcitelPickerPage : ContentPage
    {
        public static string teacherRealName;
        public UcitelPickerPage()
        {
            //PickerOfTeachers.IsVisible = true;
            //AcceptButton.IsVisible = true;
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
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
                    { "Igor Ročí", "RO" },
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
                    DisplayAlert("Něco je špatně", $"Něco je špatně", "OK");
                    return;
                }
                //PickerOfTeachers.IsVisible = false;
                //AcceptButton.IsVisible = false;
                //TeacherView.IsVisible = true;
            }
        }
    }
}

