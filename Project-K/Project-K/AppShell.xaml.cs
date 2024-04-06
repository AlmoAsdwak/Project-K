using Project_K.ViewModels;
using Project_K.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Project_K
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
