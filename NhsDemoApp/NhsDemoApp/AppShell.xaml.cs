using NhsDemoApp.ViewModels;
using NhsDemoApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NhsDemoApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AppointmentDetailPage), typeof(AppointmentDetailPage));
            Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));
        }

    }
}
