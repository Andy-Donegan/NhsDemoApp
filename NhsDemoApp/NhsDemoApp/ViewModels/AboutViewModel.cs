using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NhsDemoApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://mvcdemoappajd.azurewebsites.net/Home/About"));
        }

        public ICommand OpenWebCommand { get; }
    }
}