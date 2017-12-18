using System.Collections.Generic;
using System.Windows;
using Lototron.Containers;
using NLog;

namespace Lototron
{
    /// <summary>
    /// Interaction logic for PageSelect.xaml
    /// </summary>
    public partial class PageSelect
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        private MainWindow _FormMain;

        public PageSelect()
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

        private void ButtonSeyahet_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Seyahet selected");
            _FormMain.SelectedTypeValues = new List<SelectedTypeValue>();
            _FormMain.SelectedType = SelectedType.Seyahet;
            _FormMain.OpenForm(FormEnum.Main);
        }

        private void ButtonPul_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Pul selected");
            _FormMain.SelectedTypeValues = new List<SelectedTypeValue>();
            _FormMain.SelectedType = SelectedType.Pul;
            _FormMain.OpenForm(FormEnum.Main);
        }        

        private void ButtonBal_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Bal selected");
            _FormMain.SelectedTypeValues = new List<SelectedTypeValue>();
            _FormMain.SelectedType = SelectedType.Bal;
            _FormMain.OpenForm(FormEnum.Main);
        }
    }
}
