using TransportApiSharpSample.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TransportApiSharpSample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BusRouteDetailPage : Page
    {
        private BusRouteDetailViewModel Vm
        {
            get
            {
                return (BusRouteDetailViewModel)DataContext;
            }
        }

        public BusRouteDetailPage()
        {
            this.InitializeComponent();
        }
    }
}