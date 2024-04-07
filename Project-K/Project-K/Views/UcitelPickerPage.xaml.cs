using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CoreFoundation.DispatchSource;
using static ZXing.QrCode.Internal.Mode;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Project_K.Views
{
    public partial class UcitelPickerPage : ContentPage
    {
        public UcitelPickerPage()
        {

            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var selectedItem = picker.SelectedItem as string;
            if (selectedItem != null)
            {
                // Do something with the selected item
                DisplayAlert("Selected Item", $"Vybral sis: {selectedItem}", "OK");
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}
    
