using GalaSoft.MvvmLight.Messaging;
using System;
using System.Diagnostics;
using TransportApiSharpSample.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

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
            Messenger.Default.Register<GeoboundingBoxMessage>(this, async (message) =>
            {
                Debug.WriteLine("Message Received!");

                // Sometimes TrySetViewBoundsAsync() doesn't work because the map is not yet loaded.
                // Keep trying until it works...
                bool zoomWasSuccessful = false;
                while (!zoomWasSuccessful)
                {
                    zoomWasSuccessful = await mapControl.TrySetViewBoundsAsync(message.Payload, new Thickness(20), MapAnimationKind.Bow);
                }
            });
        }
    }
}