using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace TransportApiSharpSampleXamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = App.Locator.MainPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = App.Locator.MainPageViewModel;

            viewModel.OnAppearing();
        }
    }
}