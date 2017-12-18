using System.Windows;
using Lototron.Containers;
using NLog;

namespace Lototron
{
    /// <summary>
    /// Interaction logic for PageStart.xaml
    /// </summary>
    public partial class PageStart
    {
        private MainWindow _FormMain;
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageStart()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            _FormMain = (MainWindow)Window.GetWindow(this);
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Select);
        }        
    }
}
