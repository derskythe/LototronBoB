namespace DbLoader
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnBuildCache = new System.Windows.Forms.Button();
            this.btnMakeResultSet = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.btnSecondResultSet = new System.Windows.Forms.Button();
            this.txtWin = new System.Windows.Forms.RichTextBox();
            this.txtSecondResult = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "output.txt";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Open file";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnBuildCache
            // 
            this.btnBuildCache.Location = new System.Drawing.Point(93, 12);
            this.btnBuildCache.Name = "btnBuildCache";
            this.btnBuildCache.Size = new System.Drawing.Size(75, 23);
            this.btnBuildCache.TabIndex = 2;
            this.btnBuildCache.Text = "Build Cache";
            this.btnBuildCache.UseVisualStyleBackColor = true;
            this.btnBuildCache.Click += new System.EventHandler(this.btnBuildCache_Click);
            // 
            // btnMakeResultSet
            // 
            this.btnMakeResultSet.Location = new System.Drawing.Point(174, 12);
            this.btnMakeResultSet.Name = "btnMakeResultSet";
            this.btnMakeResultSet.Size = new System.Drawing.Size(98, 23);
            this.btnMakeResultSet.TabIndex = 2;
            this.btnMakeResultSet.Text = "Make ResultSet";
            this.btnMakeResultSet.UseVisualStyleBackColor = true;
            this.btnMakeResultSet.Click += new System.EventHandler(this.btnMakeResultSet_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(12, 41);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(364, 481);
            this.txtResult.TabIndex = 3;
            this.txtResult.Text = "";
            // 
            // btnSecondResultSet
            // 
            this.btnSecondResultSet.Location = new System.Drawing.Point(278, 12);
            this.btnSecondResultSet.Name = "btnSecondResultSet";
            this.btnSecondResultSet.Size = new System.Drawing.Size(98, 23);
            this.btnSecondResultSet.TabIndex = 2;
            this.btnSecondResultSet.Text = "Make ResultSet2";
            this.btnSecondResultSet.UseVisualStyleBackColor = true;
            this.btnSecondResultSet.Click += new System.EventHandler(this.btnSecondResultSet_Click);
            // 
            // txtWin
            // 
            this.txtWin.Location = new System.Drawing.Point(382, 41);
            this.txtWin.Name = "txtWin";
            this.txtWin.Size = new System.Drawing.Size(224, 481);
            this.txtWin.TabIndex = 4;
            this.txtWin.Text = "";
            // 
            // txtSecondResult
            // 
            this.txtSecondResult.Location = new System.Drawing.Point(612, 41);
            this.txtSecondResult.Name = "txtSecondResult";
            this.txtSecondResult.Size = new System.Drawing.Size(318, 481);
            this.txtSecondResult.TabIndex = 5;
            this.txtSecondResult.Text = "";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 528);
            this.Controls.Add(this.txtSecondResult);
            this.Controls.Add(this.txtWin);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnSecondResultSet);
            this.Controls.Add(this.btnMakeResultSet);
            this.Controls.Add(this.btnBuildCache);
            this.Controls.Add(this.btnOpen);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnBuildCache;
        private System.Windows.Forms.Button btnMakeResultSet;
        private System.Windows.Forms.RichTextBox txtResult;
        private System.Windows.Forms.Button btnSecondResultSet;
        private System.Windows.Forms.RichTextBox txtWin;
        private System.Windows.Forms.RichTextBox txtSecondResult;
    }
}

