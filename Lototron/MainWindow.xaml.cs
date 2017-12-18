using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using crypto;
using Lototron.Containers;
using Lototron.Properties;
using NLog;
using Org.BouncyCastle.Security;
using SQLiteDb;

namespace Lototron
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        private String _CurrentForm = FormEnum.Start;
        private String _PrevForm = FormEnum.Start;
        private Dictionary<String, int> _SelectedValues = new Dictionary<String, int>();
        private List<SelectedTypeValue> _SelectedTypeValues = new List<SelectedTypeValue>();
        private List<String> _CurrentSelectedValues = new List<string>();
        private String _ReturnForm = FormEnum.Select;
        private List<String> _InputBox = new List<string>();

        public List<SelectedTypeValue> SelectedTypeValues
        {
            get
            {
                return _SelectedTypeValues;
            }
            set
            {
                _SelectedTypeValues = value;
            }
        }

        public List<string> InputBox
        {
            get
            {
                return _InputBox;
            }
        }

        public string ReturnForm
        {
            get
            {
                return _ReturnForm;
            }
            set
            {
                _ReturnForm = value;
            }
        }

        public Dictionary<String, int> SelectedValues
        {
            get
            {
                return _SelectedValues;
            }
            set
            {
                _SelectedValues = value;
            }
        }

        public void AddSelectedValue(string value)
        {
            if (_SelectedValues.ContainsKey(value))
            {
                _SelectedValues[value] += 1;
            }
            else
            {
                _SelectedValues.Add(value, 1);
            }

            _CurrentSelectedValues.Add(value);
            _SelectedTypeValues.Add(new SelectedTypeValue(SelectedType, value));
        }

        public List<string> CurrentSelectedValues
        {
            get
            {
                return _CurrentSelectedValues;
            }            
        }

        public SelectedType SelectedType { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavigationWindowLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info("Loaded");
            ShowsNavigationUI = false;
            LocalDb.Init(Settings.Default.DbPath);
        }

        private void NavigationWindowClosing(object sender, CancelEventArgs e)
        {
            Log.Info("Closing");
        }

        public void OpenForm(String f)
        {
            lock (_CurrentForm)
            {
                Log.Debug("Current From: {0}, New form: {1}", _CurrentForm, f);
                _CurrentForm = f;
            }

            Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<string>(Navigate),
                f);
        }

        public bool PrepareInputBox()
        {
            try
            {
                _CurrentSelectedValues = new List<string>();

                var rnd = new SecureRandom(Encoding.ASCII.GetBytes(Wrapper.GetRandomMac(512)));

                _InputBox = LocalDb.ListInputBox(_SelectedValues);
                var totalCount = _InputBox.Count;
#if !DEBUG
                for (int i = 0; i < totalCount; i++)
                {
                    var newIdx = rnd.Next(0, totalCount - 1);
                    var removedValue = _InputBox[newIdx];
                    _InputBox[newIdx] = _InputBox[i];
                    _InputBox[i] = removedValue;
                }
#endif

                return true;
            }
            catch (Exception exp)
            {
                Log.Error(exp, exp.Message);
            }

            return false;
        }

        private void Navigate(string f)
        {
            try
            {
                _PrevForm = CurrentSource.OriginalString;
                Log.Debug("PrevUri: {0}", _PrevForm);
                Navigate(new Uri(f, UriKind.RelativeOrAbsolute));
            }
            catch (Exception exp)
            {
                Log.Error(exp, exp.Message);
            }
        }
    }
}
