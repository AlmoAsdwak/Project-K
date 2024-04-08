using Project_K.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Project_K.ViewModels
{
	public class UcitelViewModel : ContentPage
	{
        public Command RefreshCommand { get; }
        public UcitelViewModel()
		{
            RefreshCommand = new Command(() => RefreshTeacher());
        }

        private void RefreshTeacher()
        {
            IsBusy = true;
            GetTeacher.TeacherRefresh();
            IsBusy = false;
        }
    }
}