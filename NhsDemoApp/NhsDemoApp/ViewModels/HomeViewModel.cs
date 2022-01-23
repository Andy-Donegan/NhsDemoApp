using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NhsDemoApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {

        public HomeViewModel()
        {
            Title = "Home";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://mvcdemoajd.azurewebsites.net/Home/About"));
        }

        public ICommand OpenWebCommand { get; }
    }
}


