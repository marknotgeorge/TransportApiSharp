using System;
using TransportApiSharpSample.ViewModels;
using Windows.UI.Xaml.Controls;

namespace TransportApiSharpSample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel Vm
        {
            get
            {
                return (MainPageViewModel)DataContext;
            }
        }

        public MainPage()
        {
            InitializeComponent();
        }
    }
}