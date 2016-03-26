using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TransportAPISharp;
using TransportApiSharpSample.Models;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Streams;

namespace TransportApiSharpSample
{
    /// Documentation on APIs used in this page: https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public static List<BusOperator> OperatorCodes { get; set; }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            DispatcherHelper.Initialize();
            NavigationService.Navigate(typeof(Views.MainPage));
            return Task.CompletedTask;
        }
    }
}