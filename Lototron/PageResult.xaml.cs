using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Lototron.Containers;
using NLog;

namespace Lototron
{
    /// <summary>
    /// Interaction logic for PageResult.xaml
    /// </summary>
    public partial class PageResult
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        private MainWindow _FormMain;
        private int _Page;
        private int _TotalPages;
        private const int MAX_ELEMENTS = 25;


        public PageResult()
        {
            InitializeComponent();
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {

        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            _FormMain = (MainWindow)Window.GetWindow(this);

            try
            {

                ///////////////////////////////////
                //

                //var rnd = new Random();
                //for (int i = 0; i < 500; i++)
                //{
                //    _FormMain.CurrentSelectedValues.Add(rnd.Next(1, 150000).ToString(CultureInfo.InvariantCulture));
                //}
                ////////////////////////////////////

                if (_FormMain.CurrentSelectedValues.Count == 0)
                {
                    _FormMain.OpenForm(FormEnum.Select);
                    return;
                }

                _TotalPages =
                    Convert.ToInt32(
                        Math.Round(Math.Ceiling(_FormMain.CurrentSelectedValues.Count / (double)MAX_ELEMENTS)));

                StackPages.Children.Clear();
                if (_TotalPages > 1)
                {
                    for (int i = 0; i < _TotalPages; i++)
                    {
                        var label = new Label
                            {
                                Style = FindResource("PageNumberLabelStyle") as Style,
                                Content = (i + 1).ToString(CultureInfo.InvariantCulture),
                                Tag = i
                            };
                        label.MouseDown += LabelOnMouseDown;
                        StackPages.Children.Add(label);
                    }
                }

                CheckPageButtons();
                AddButtons();

                switch (_FormMain.SelectedType)
                {
                    case SelectedType.Seyahet:
                        TypeLogo.Source = FindResource("IconSeyahet") as BitmapImage;
                        TypeName.Content = Properties.Resources.TitleSeyahet;
                        OneResult.Visibility = Visibility.Visible;
                        break;

                    case SelectedType.Pul:
                        TypeLogo.Source = FindResource("IconPul") as BitmapImage;
                        TypeName.Content = Properties.Resources.TitlePul;
                        break;

                    //case SelectedType.Gold:
                    //    TypeLogo.Source = FindResource("IconGold") as BitmapImage;
                    //    break;


                    case SelectedType.Bal:
                        TypeLogo.Source = FindResource("IconBal") as BitmapImage;
                        TypeName.Content = Properties.Resources.TitleBal;
                        break;
                }
            }
            catch (Exception exp)
            {
                Log.Error(exp, exp.Message);
                
            }
        }

        private void LabelOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            try
            {
                var lblSender = sender as Label;
                if (lblSender == null)
                {
                    return;
                }
                _Page = Convert.ToInt32(lblSender.Tag);
                CheckPageButtons();
                AddButtons(); 
            }
            catch (Exception exp)
            {
                Log.Error(exp, exp.Message);
            }
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(
                String.IsNullOrEmpty(_FormMain.ReturnForm)
                    ? FormEnum.Select
                    : _FormMain.ReturnForm);
        }

        private void NavPrev_Click(object sender, RoutedEventArgs e)
        {
            _Page--;
            CheckPageButtons();
            AddButtons();                     
        }

        private void NavNext_Click(object sender, RoutedEventArgs e)
        {
            _Page++;
            CheckPageButtons();
            AddButtons();
        }

        private void CheckPageButtons()
        {
            if (_TotalPages == 1)
            {
                ButtonNavNext.Visibility = Visibility.Hidden;
                ButtonNavPrev.Visibility = Visibility.Hidden;
                return;
            }

            ButtonNavNext.IsEnabled = (_Page + 1) < _TotalPages;
            ButtonNavPrev.IsEnabled = _Page != 0;

            try
            {
                for (int i = 0; i < StackPages.Children.Count; i++)
                {
                    var lblSender = StackPages.Children[i] as Label;
                    if (lblSender != null)
                    {
                        lblSender.Style = FindResource("PageNumberLabelStyle") as Style;
                        
                    }
                }
                var label = StackPages.Children[_Page] as Label;
                if (label != null)
                {
                    label.Style = FindResource("PageNumberSelectedLabelStyle") as Style;
                }
            }
            catch (Exception exp)
            {
                Log.Error(exp, exp.Message);
            } 
        }

        private void AddButtons()
        {
            try
            {
                Grid.Children.Clear();

                int i = 0;
                int numToSkip = _Page * MAX_ELEMENTS;
                foreach (var item in _FormMain.CurrentSelectedValues)
                {
                    if (_FormMain.SelectedType == SelectedType.Seyahet)
                    {
                        Grid.Visibility = Visibility.Hidden;
                        OneResult.Visibility = Visibility.Visible;
                        OneResult.Content = item;                        
                    }
                    else
                    {
                        if (i < numToSkip)
                        {
                            i++;
                            continue;
                        }

                        if (i > numToSkip + MAX_ELEMENTS - 1)
                        {
                            break;
                        }

                        var label = new Label
                        {
                            Style = FindResource("ResultLabelStyle") as Style,
                            Content = item
                        };
                        Grid.Children.Add(label);
                    }

                    i++;
                }
            }
            catch (Exception exp)
            {
                Log.Error(exp, exp.Message);
            }
        }
    }
}
