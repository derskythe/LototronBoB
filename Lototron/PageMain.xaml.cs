using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using crypto;
using Lototron.Containers;
using NLog;
using Org.BouncyCastle.Security;

namespace Lototron
{
    /// <summary>
    /// Interaction logic for PageMain.xaml
    /// </summary>
    public partial class PageMain
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        private const String FORMAT_VALUE = "0000000000";
        private Storyboard _Story;
        private Storyboard _OneDigitStory;
        private Storyboard _EndStory;
        private bool _Start;
        private readonly Queue<Image> _Images = new Queue<Image>(6);
        private readonly SecureRandom _Random;
        private string _CurrentValue = "0";
        private string _PrevValue = String.Empty;
        private int _FinishedCounters;
        private int _ResultIteration = 1;
        private int _VisibleGridElements;
        private int _MaxIteration;
        private MainWindow _FormMain;
        private StreamWriter _Stream;
        private StreamWriter _StreamMarquee;

        private int _FirstButtonValue;
        private int _SecondButtonValue;
        private int _ThirdButtonValue;
        private int _FourthButtonValue;
        private bool _LongStory;
        private bool _FirstButtonLongStory;
        private bool _SecondButtonLongStory;
        private bool _ThirdButtonLongStory;
        private bool _FourthButtonLongStory;

        private String _FirstButtonFile;
        private String _SecondButtonFile;
        private String _ThirdButtonFile;
        private string _FourthButtonFile;
        private String _CurrentFile;

        public PageMain()
        {
            InitializeComponent();
            _Random = new SecureRandom(Encoding.ASCII.GetBytes(Wrapper.GetRandomMac(512)));
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            _FormMain = (MainWindow)Window.GetWindow(this);

            if (_FormMain == null)
            {
                Log.Fatal("FormMain is null. FATAL ERROR");
                return;
            }

            _FormMain.CurrentSelectedValues.Clear();

            try
            {
                switch (_FormMain.SelectedType)
                {
                    case SelectedType.Seyahet:
                        Header.Source = FindResource("HeaderSeyahet") as BitmapImage;
                        ButtonFirst.Visibility = Visibility.Hidden;
                        ButtonSecond.Visibility = Visibility.Hidden;
                        ButtonThird.Visibility = Visibility.Hidden;
                        ButtonFourth.Visibility = Visibility.Hidden;
                        _MaxIteration = 1;
                        _LongStory = true;
                        _FormMain.ReturnForm = FormEnum.Select;

                        _CurrentFile = _FirstButtonFile = "seyahet.txt";
                        break;

                    case SelectedType.Pul:
                        Header.Source = FindResource("HeaderPul") as BitmapImage;
                        ButtonFirst.Visibility = Visibility.Visible;
                        ButtonSecond.Visibility = Visibility.Visible;
                        ButtonThird.Visibility = Visibility.Visible;
                        ButtonFourth.Visibility = Visibility.Hidden;
                        _FirstButtonValue = 9;
                        _SecondButtonValue = 30;
                        _ThirdButtonValue = 60;
                        ButtonFirst.Style = FindResource("TwoHundredButtonStyle") as Style;
                        ButtonSecond.Style = FindResource("HundredButtonStyle") as Style;
                        ButtonThird.Style = FindResource("FiftyButtonStyle") as Style;
                        _FirstButtonLongStory = _SecondButtonLongStory = _ThirdButtonLongStory = _FourthButtonLongStory = false;

                        _FirstButtonFile = "pul200.txt";
                        _SecondButtonFile = "pul100.txt";
                        _ThirdButtonFile = "pul50.txt";
                        break;

                    case SelectedType.Bal:
                        Header.Source = FindResource("HeaderBal") as BitmapImage;
                        ButtonFirst.Visibility = Visibility.Visible;
                        ButtonSecond.Visibility = Visibility.Visible;
                        ButtonThird.Visibility = Visibility.Visible;
                        ButtonFourth.Visibility = Visibility.Visible;
                        _FirstButtonValue = 50;
                        _SecondButtonValue = 50;
                        _ThirdButtonValue = 150;
                        _FourthButtonValue = 250;
                        ButtonFirst.Style = FindResource("FiftyButtonStyle") as Style;
                        ButtonSecond.Style = FindResource("ThirtyButtonStyle") as Style;
                        ButtonThird.Style = FindResource("TwentyButtonStyle") as Style;
                        ButtonFourth.Style = FindResource("TenButtonStyle") as Style;
                        _FirstButtonLongStory = _SecondButtonLongStory = _ThirdButtonLongStory = _FourthButtonLongStory = false;

                        _FirstButtonFile = "bal50.txt";
                        _SecondButtonFile = "bal30.txt";
                        _ThirdButtonFile = "bal20.txt";
                        _FourthButtonFile = "bal10.txt";

                        break;
                }

                _Stream =
                    new StreamWriter(
                        Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "result.txt", true,
                        Encoding.UTF8);
                SaveToFile("Starting...");
            }
            catch (Exception exp)
            {
                Log.Error(exp, exp.Message);
            }
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_Stream != null)
                {
                    _Stream.Close();
                    _Stream.Dispose();
                }
            }
            catch (Exception exp)
            {
                Log.Error(exp, exp.Message);
            }
        }

        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            StopRotating();
        }

        private void StopRotating()
        {
            if (!_Start)
            {
                return;
            }
            ButtonStop.IsEnabled = false;
            _Start = true;
            _FinishedCounters = 0;
            Randomize();

            _OneDigitStory.Stop();
            _OneDigitStory.RepeatBehavior = new RepeatBehavior(2);
            _OneDigitStory.Begin(Img0);

            _Images.Enqueue(Img1);
            _Images.Enqueue(Img2);
            _Images.Enqueue(Img3);
            _Images.Enqueue(Img4);
            _Images.Enqueue(Img5);
            _Images.Enqueue(Img6);
            _Images.Enqueue(Img7);
            _Images.Enqueue(Img8);
            _Images.Enqueue(Img9);
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            if (_MaxIteration != 0)
            {
                _FormMain.PrepareInputBox();

                SaveToFile("Starting " + _MaxIteration + " iteration");
                ButtonStart.IsEnabled = false;
                ButtonStop.IsEnabled = true;
                Log.Info("Starting " + _MaxIteration + " iteration");
                StartRotation();
            }
        }

        private void StartRotation()
        {
            if (!_Start)
            {
                _Start = true;

                if (_LongStory)
                {
                    _Story = (Storyboard)FindResource(StoryBoards.NormalRoutingLong);
                    _OneDigitStory = (Storyboard)FindResource(StoryBoards.OneRoutingLong);
                }
                else
                {
                    _Story = (Storyboard)FindResource(StoryBoards.NormalRouting);
                    _OneDigitStory = (Storyboard)FindResource(StoryBoards.OneRouting);
                }

                _OneDigitStory.RepeatBehavior = RepeatBehavior.Forever;
                _OneDigitStory.Begin(Img0);

                _Story.RepeatBehavior = RepeatBehavior.Forever;
                _Story.Begin(Img1);
                _Story.Begin(Img2);
                _Story.Begin(Img3);
                _Story.Begin(Img4);
                _Story.Begin(Img5);
                _Story.Begin(Img6);
                _Story.Begin(Img7);
                _Story.Begin(Img8);
                _Story.Begin(Img9);
            }
        }

#if DEBUG
        private int _I;
#endif
        private void Randomize()
        {
            try
            {
                do
                {
#if DEBUG                    
                    _CurrentValue = _FormMain.InputBox[_I];
                    _I++;
#else
                    _CurrentValue = "4000000000";
                    //_CurrentValue = _FormMain.InputBox[_Random.Next(0, _FormMain.InputBox.Count - 1)];
#endif
                }
                while (
                    _FormMain.CurrentSelectedValues.FindAll(s => s == _CurrentValue).Count > 0 ||
                    _FormMain.SelectedTypeValues.Exists(
                        item => item.Type == _FormMain.SelectedType && item.Value == _CurrentValue)
                    );
            }
            catch (Exception exp)
            {
                _CurrentValue = FORMAT_VALUE;
                Log.Error(exp, exp.Message);
            }
        }

        private void NextElement()
        {
            Debug.Print("NextElement");
            lock (_Images)
            {
                if (_Images.Count > 0)
                {
                    var img = _Images.Dequeue();

                    int pos = Convert.ToInt32(img.Tag);
                    string storyToRouting = GetStory(pos);

                    _EndStory = (Storyboard)FindResource(storyToRouting);
                    //_EndStory.Completed += EndStoryOnCompleted;
                    _Story.Stop(img);
                    _EndStory.Begin(img);
                }
                //else
                //{
                //    //MessageBox.Show("Done");
                //}
            }
        }

        #region GetStory

        private string GetStory(int pos)
        {
            string returnValue = String.Empty;
            if (_LongStory)
            {
                switch (_CurrentValue.Substring(pos, 1))
                {
                    case "0":
                        returnValue = StoryBoards.ToZeroRoutingLong;
                        break;

                    case "1":
                        returnValue = StoryBoards.ToOneRoutingLong;
                        break;

                    case "2":
                        returnValue = StoryBoards.ToTwoRoutingLong;
                        break;

                    case "3":
                        returnValue = StoryBoards.ToThreeRoutingLong;
                        break;

                    case "4":
                        returnValue = StoryBoards.ToFourRoutingLong;
                        break;

                    case "5":
                        returnValue = StoryBoards.ToFiveRoutingLong;
                        break;

                    case "6":
                        returnValue = StoryBoards.ToSixRoutingLong;
                        break;

                    case "7":
                        returnValue = StoryBoards.ToSevenRoutingLong;
                        break;

                    case "8":
                        returnValue = StoryBoards.ToEightRoutingLong;
                        break;

                    case "9":
                        returnValue = StoryBoards.ToNineRoutingLong;
                        break;
                }
            }
            else
            {
                switch (_CurrentValue.Substring(pos, 1))
                {
                    case "0":
                        returnValue = StoryBoards.ToZeroRouting;
                        break;

                    case "1":
                        returnValue = StoryBoards.ToOneRouting;
                        break;

                    case "2":
                        returnValue = StoryBoards.ToTwoRouting;
                        break;

                    case "3":
                        returnValue = StoryBoards.ToThreeRouting;
                        break;

                    case "4":
                        returnValue = StoryBoards.ToFourRouting;
                        break;

                    case "5":
                        returnValue = StoryBoards.ToFiveRouting;
                        break;

                    case "6":
                        returnValue = StoryBoards.ToSixRouting;
                        break;

                    case "7":
                        returnValue = StoryBoards.ToSevenRouting;
                        break;

                    case "8":
                        returnValue = StoryBoards.ToEightRouting;
                        break;

                    case "9":
                        returnValue = StoryBoards.ToNineRouting;
                        break;
                }
            }

            return returnValue;
        }

        #endregion

        private void OneDigitStoryOnCompleted(object sender, EventArgs eventArgs)
        {
            Debug.Print("OneDigitStoryOnCompleted");

            if (_CurrentValue.StartsWith("4"))
            {
                _EndStory =
                    (Storyboard)
                    FindResource(_LongStory ? StoryBoards.OneToOneRoutingLong : StoryBoards.OneToOneRouting);
                _EndStory.Begin(Img0);
            }
            else
            {
                _EndStory =
                    (Storyboard)
                    FindResource(_LongStory ? StoryBoards.OneToZeroRoutingLong : StoryBoards.OneToZeroRouting);
                _EndStory.Begin(Img0);
            }

            //while (_Images.Count > 0)
            //{
            //    NextElement();
            //} 
        }

        private void EndStoryOnCompleted(object sender, EventArgs eventArgs)
        {
            Debug.Print("EndStoryOnCompleted");
            var current = Interlocked.Increment(ref _FinishedCounters);

            Debug.Print("Counter value: {0}", current);

            if (current >= 10)
            {
                if (_CurrentValue != _PrevValue)
                {
                    _PrevValue = _CurrentValue;
                    _Start = false;
                    //LabelResultValues.Content = _CurrentValue;
                    Log.Info("Winner value: {0}", _CurrentValue);
                    SaveToFile(String.Format("Winner value: {0}", _CurrentValue));
                    SaveToAnotherFile(_CurrentValue);
                    _FormMain.AddSelectedValue(_CurrentValue);

                    var thread = new Thread(ShowCurrentResultThread);
                    thread.Start();
                }
            }
            else
            {
                NextElement();
            }
        }

        private void ShowCurrentResultThread()
        {
            Thread.Sleep(1000);
            Dispatcher.Invoke(DispatcherPriority.Normal,
                              new Action(ShowCurrentResult));
        }

        private void ShowCurrentResult()
        {
            Debug.Print("StoryShowResult_OnCompleted");
            /*if (_LongStory)
            {
                _ResultIteration++;
                LabelResultValues.Content = _CurrentValue;
                ((Storyboard)FindResource("StoryFadeInResult")).Begin(GridResultLong);
            }
            else
            {*/
            var txtOrder = new TextBox
                {
                    Style = FindResource("LottoTextStyle") as Style,
                    Name = "TxtOrder" + _ResultIteration,
                    Text = _ResultIteration.ToString(CultureInfo.InvariantCulture),
                    IsReadOnly = true,
                    Visibility = Visibility.Hidden
                };

            var txtValue = new TextBox
                {
                    Style = FindResource("LottoTextResultStyle") as Style,
                    Name = "TxtValue" + _ResultIteration,
                    Text = _CurrentValue,
                    IsReadOnly = true,
                    Visibility = Visibility.Hidden
                };

            _VisibleGridElements++;
            GridResult.Children.Insert(GridResult.Children.Count, txtOrder);
            GridResult.Children.Insert(GridResult.Children.Count, txtValue);

            ((Storyboard)FindResource("StoryFadeIn")).Begin(txtOrder);
            ((Storyboard)FindResource("StoryFadeIn")).Begin(txtValue);

            _ResultIteration++;

            if (_ResultIteration <= _MaxIteration)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                                  new Action(StartRotation));
            }

            if (_VisibleGridElements > 5)
            {
                var idx = GridResult.Children.Count > 12 ? GridResult.Children.Count - 12 : 0;
                ((Storyboard)FindResource("StoryFadeOutCollapsed")).Begin((TextBox)GridResult.Children[idx]);
                ((Storyboard)FindResource("StoryFadeOutCollapsed")).Begin((TextBox)GridResult.Children[idx + 1]);
                _VisibleGridElements = 5;
            }
            /*}*/
        }

        private void StoryFadeIn_OnCompleted(object sender, EventArgs e)
        {
            Debug.Print("StoryFadeIn_OnCompleted");
            var startUpThread = new Thread(DoTimeout);
            startUpThread.Start();
        }

        private void DoTimeout()
        {
            if (_ResultIteration <= _MaxIteration)
            {
                //Thread.Sleep(1 * 1000);
                Dispatcher.Invoke(DispatcherPriority.Normal,
                                  new Action(StopRotating));
            }
            else
            {
                ShowResult();
            }
        }

        private void ShowResult()
        {
            SaveToFile("**************************** End of iteration");
            Thread.Sleep(2 * 1000);
            _FormMain.OpenForm(FormEnum.Result);
        }

        private void StoryFadeOut_OnCompleted(object sender, EventArgs e)
        {

        }

        private void SaveToFile(string value)
        {
            try
            {
                _Stream.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + value);
                _Stream.Flush();
            }
            catch (Exception exp)
            {
                Log.Error(exp, exp.Message);
            }
        }

        private void SaveToAnotherFile(string value)
        {
            try
            {
                _StreamMarquee = new StreamWriter(
                    Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + _CurrentFile, true,
                    Encoding.UTF8);

                _StreamMarquee.WriteLine(value);
                _StreamMarquee.Flush();
                _StreamMarquee.Close();
                _StreamMarquee.Dispose();
            }
            catch (Exception exp)
            {
                Log.Error(exp, exp.Message);
            }
        }

        private void ButtonFirst_Click(object sender, RoutedEventArgs e)
        {
            if (_Start)
            {
                return;
            }
            ButtonFirst.IsEnabled = false;
            ButtonSecond.IsEnabled = true;
            ButtonThird.IsEnabled = true;
            ButtonFourth.IsEnabled = true;

            _MaxIteration = _FirstButtonValue;
            _LongStory = _FirstButtonLongStory;

            _CurrentFile = _FirstButtonFile;

            _FormMain.ReturnForm = FormEnum.Main;
        }

        private void ButtonSecond_Click(object sender, RoutedEventArgs e)
        {
            if (_Start)
            {
                return;
            }
            ButtonFirst.IsEnabled = true;
            ButtonSecond.IsEnabled = false;
            ButtonThird.IsEnabled = true;
            ButtonFourth.IsEnabled = true;

            _MaxIteration = _SecondButtonValue;
            _LongStory = _SecondButtonLongStory;

            _CurrentFile = _SecondButtonFile;
            _FormMain.ReturnForm = FormEnum.Main;
        }

        private void ButtonThird_Click(object sender, RoutedEventArgs e)
        {
            if (_Start)
            {
                return;
            }
            ButtonFirst.IsEnabled = true;
            ButtonSecond.IsEnabled = true;
            ButtonThird.IsEnabled = false;
            ButtonFourth.IsEnabled = true;

            _MaxIteration = _ThirdButtonValue;
            _LongStory = _ThirdButtonLongStory;

            _CurrentFile = _ThirdButtonFile;
            _FormMain.ReturnForm = ButtonFourth.Visibility == Visibility.Visible
                ? FormEnum.Main
                : FormEnum.Select;
        }

        private void ButtonFourth_Click(object sender, RoutedEventArgs e)
        {
            if (_Start)
            {
                return;
            }
            ButtonFirst.IsEnabled = true;
            ButtonSecond.IsEnabled = true;
            ButtonThird.IsEnabled = true;
            ButtonFourth.IsEnabled = false;

            _MaxIteration = _FourthButtonValue;
            _LongStory = _FourthButtonLongStory;

            _CurrentFile = _FourthButtonFile;
            _FormMain.ReturnForm = FormEnum.Select;
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Select);
        }
    }
}
