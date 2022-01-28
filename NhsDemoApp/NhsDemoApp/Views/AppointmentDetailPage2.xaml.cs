using NhsDemoApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NhsDemoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentDetailPage2 : ContentPage
    {
        public AppointmentDetailPage2()
        {
            InitializeComponent();
            BindingContext = new AppointmentDetailViewModel2();
        }

        //void OnArrivalTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        //{
        //    if (args.PropertyName == "Time")
        //    {
        //        DisplayAlert("Timer Alert", "The timer has changed", "OK");
        //        Console.WriteLine("fdfdfdffdfsdfsdfsdfsdfsdf");
        //    }
        //}

    }
}