using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using crypto;
using DbLoader.Properties;
using Org.BouncyCastle.Security;
using SQLiteDb;
using SQLiteDb.Containers;

namespace DbLoader
{
    public partial class FormMain : Form
    {
        private FormProgress _FormProgress;

        private delegate void IncProgressBarCallback();

        private delegate void SetTxtTotalCallback(string text);

        private List<string> _FileContent = new List<string>();
        private Thread _CheckThread;

        private int _TotalCount;

        public int TotalCount
        {
            get
            {
                if (_TotalCount == 0)
                {
                    var list = LocalDb.ListInputBox();
                    _TotalCount = list.Count;
                    return _TotalCount;
                }
                else
                {
                    return _TotalCount;
                }
            }
            set
            {
                _TotalCount = value;
            }
        }

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(Settings.Default.InitDir))
                {
                    Settings.Default.InitDir = Environment.SpecialFolder.MyComputer.ToString();
                }

                openFileDialog.InitialDirectory = Settings.Default.InitDir;
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Settings.Default.InitDir = Path.GetDirectoryName(openFileDialog.FileName);
                    Settings.Default.Save();

                    string fileContent;
                    using (var file = new StreamReader(openFileDialog.FileName, Encoding.UTF8))
                    {
                        fileContent = file.ReadToEnd();
                        file.Close();
                    }

                    _FileContent = new List<String>(fileContent.Split('\n'));
                    if (_CheckThread != null &&
                        _CheckThread.ThreadState == ThreadState.Running)
                    {
                        return;
                    }

                    _FormProgress = new FormProgress(_FileContent.Count);
                    _CheckThread = new Thread(CountThread);
                    _CheckThread.Start();

                    if (_FormProgress.ShowDialog() == DialogResult.Cancel)
                    {
                        _CheckThread.Abort();
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void IncProgressBar()
        {
            if (_FormProgress.InvokeRequired)
            {
                IncProgressBarCallback d = IncProgressBar;
                Invoke(d, null);
                return;
            }

            _FormProgress.AddValue();
        }

        private void CountThread(object obj)
        {
            if (_FileContent == null ||
                _FileContent.Count == 0)
            {
                return;
            }

            LocalDb.CleanRowBox();
            LocalDb.CleanInputBox();

            int i = 0;
            foreach (var line in _FileContent)
            {
                var item = line.Split(';');
                if (i == 0)
                {
                    i++;
                    continue;
                }

                IncProgressBar();
                if (line.Length < 2)
                {
                    continue;
                }
                if (LocalDb.HasRecord(item[0].Trim()))
                {
                    var rec = LocalDb.GetRecord(item[0].Trim());
                    SetTxtTotal(rec.pan + "\r\n");
                    continue;
                    //if (MessageBox.Show(
                    //    String.Format("Record: {0}, {1}. New Record: {2}, {3}", rec.pan, rec.chance, item[0], item[1]),
                    //    @"Warning!",
                    //    MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //{
                        
                    //    continue;
                    //}
                    //else
                    //{
                    //    // TODO:
                    //}
                }

                LocalDb.InsertInputBox(item[0], Convert.ToDecimal(item[1].Trim()));
                i++;
            }
            IncProgressBar();

            //LocalDb.CleanRowBox();

            //List<RawItem> items = new List<RawItem>();

            //for (int i = 1; i < _FileContent.Count; i++)
            //{
            //    IncProgressBar();
            //    var line = _FileContent[i].Split(';');
            //    if (line.Length < 3)
            //    {
            //        continue;
            //    }

            //    var pan = line[2].Substring(0, 4) + line[2].Substring(10);
            //    decimal amount = 0;
            //    try
            //    {
            //        amount = decimal.Parse(line[3], NumberStyles.Any, CultureInfo.CurrentCulture);
            //    }
            //    catch (FormatException)
            //    {

            //    }
            //    items.Add(new RawItem(pan, amount));
            //}
            //IncProgressBar();
            //LocalDb.InsertRawBox(items);
        }

        private void BuildCache()
        {
            LocalDb.CleanInputBox();
            var list = LocalDb.ListRawBox();

            foreach (ds.raw_boxRow item in list)
            {
                IncProgressBar();
                if (LocalDb.HasRecord(item.pan))
                {
                    continue;
                }
                var sum = Math.Round(Math.Ceiling(LocalDb.SumPanInRawBox(item.pan) / 20), 0);
                LocalDb.InsertInputBox(item.pan, sum);
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LocalDb.Init(Settings.Default.DbPath);
        }

        private void btnBuildCache_Click(object sender, EventArgs e)
        {
            if (_CheckThread != null &&
                       _CheckThread.ThreadState == ThreadState.Running)
            {
                return;
            }

            _FormProgress = new FormProgress(LocalDb.CountRawBox());
            _CheckThread = new Thread(BuildCache);
            _CheckThread.Start();

            if (_FormProgress.ShowDialog() == DialogResult.Cancel)
            {
                _CheckThread.Abort();
            }
        }

        private void btnMakeResultSet_Click(object sender, EventArgs e)
        {
            txtResult.Text = String.Empty;
            var rawList = LocalDb.ListInputBox();
            //var alreadyMixed = new HashSet<int>();

            var rnd = new SecureRandom(Encoding.ASCII.GetBytes(Wrapper.GetRandomMac(512)));
            TotalCount = rawList.Count;
            for (int i = 0; i < rawList.Count; i++)
            {
                int newIdx = -1;

                /*do
                {*/
                newIdx = rnd.Next(0, TotalCount - 1);
                /*}
                while (alreadyMixed.Contains(newIdx));

                alreadyMixed.Add(newIdx);*/

                var removedValue = rawList[newIdx];
                rawList[newIdx] = rawList[i];
                rawList[i] = removedValue;
            }


            var str = new StringBuilder();
            foreach (var item in rawList)
            {
                str.Append(item).Append("\r\n");
            }

            txtResult.Text = str.ToString();
        }

        private void btnSecondResultSet_Click(object sender, EventArgs e)
        {
            txtWin.Text = String.Empty;
            txtSecondResult.Text = String.Empty;
            var rnd = new SecureRandom(Encoding.ASCII.GetBytes(Wrapper.GetRandomMac(512)));
            var winNumber = new Dictionary<String, int>();
            var rawList = LocalDb.ListInputBox();
            TotalCount = rawList.Count;

            var str = new StringBuilder();
            for (int i = 0; i < 200; i++)
            {
                int newIdx;
                do
                {
                    newIdx = rnd.Next(0, TotalCount - 1);
                }
                while (winNumber.ContainsKey(rawList[newIdx]));
                
                winNumber.Add(rawList[newIdx], 1);
                str.Append(rawList[newIdx]).Append("\r\n");
            }

            txtWin.Text = str.ToString();

            rawList = LocalDb.ListInputBox(winNumber);
            TotalCount = rawList.Count;

            for (int i = 0; i < rawList.Count; i++)
            {
                int newIdx = -1;

                /*do
                {*/
                newIdx = rnd.Next(0, TotalCount - 1);
                /*}
                while (alreadyMixed.Contains(newIdx));

                alreadyMixed.Add(newIdx);*/

                var removedValue = rawList[newIdx];
                rawList[newIdx] = rawList[i];
                rawList[i] = removedValue;
            }

            str = new StringBuilder();
            foreach (var item in rawList)
            {
                str.Append(item).Append("\r\n");
            }

            txtSecondResult.Text = str.ToString();
        }

        private void SetTxtTotal(string value)
        {
            if (txtResult.InvokeRequired)
            {
                var d = new SetTxtTotalCallback(SetTxtTotal);
                txtResult.Invoke(d, value);
                return;
            }

            txtResult.Text += value;
        }
    }
}
