using System;
using System.Windows.Forms;

namespace DbLoader
{
    public partial class FormProgress : Form
    {
        private readonly int _Total;

        public FormProgress(int total)
        {
            _Total = total;
            InitializeComponent();
        }

        public void AddValue()
        {
            if (progressTotal.Value + 1 > _Total)
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            } 
            progressTotal.Value += 1;
            lblTotal.Text = @"Line " + progressTotal.Value + @" of " + _Total;
            if (progressTotal.Value >= _Total)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void FormProgress_Load(object sender, EventArgs e)
        {
            progressTotal.Visible = true;
            progressTotal.Maximum = _Total;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
