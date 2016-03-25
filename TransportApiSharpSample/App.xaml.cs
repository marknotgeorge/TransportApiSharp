using GalaSoft.MvvmLight.Threading;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
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

        public static TravelineOperatorCodes OperatorCodes { get; set; }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            DispatcherHelper.Initialize();
            OperatorCodes = getOperatorCodes().Result;

            NavigationService.Navigate(typeof(Views.MainPage));
            return Task.CompletedTask;
        }

        private async Task<TravelineOperatorCodes> getOperatorCodes()
        {
            TravelineOperatorCodes returnVal = null;

            StorageFile file = await Package.Current.InstalledLocation.GetFileAsync("OperatorsandPublicNames.xml");
            using (Stream readStream = await file.OpenStreamForReadAsync())
            {
                var serializer = new XmlSerializer(typeof(TravelineOperatorCodes));
                returnVal = (TravelineOperatorCodes)serializer.Deserialize(readStream);
            }

            return returnVal;
        }
    }
}