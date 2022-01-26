using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NhsDemoApp.ViewModels;

namespace NhsDemoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentPage : ContentPage
    {
        AppointmentViewModel _viewModel;
        public AppointmentPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new AppointmentViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

    }
}