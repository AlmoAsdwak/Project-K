using Project_K.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Project_K.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}