namespace DisasterModel
{
    partial class FormDispatch
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
            this.txtEarthquakeName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFacilityLoc = new System.Windows.Forms.TextBox();
            this.txtIncidentLoc = new System.Windows.Forms.TextBox();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dtEarthquakeDate = new System.Windows.Forms.DateTimePicker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label7 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.paraPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // txtEarthquakeName
            // 
            this.txtEarthquakeName.Location = new System.Drawing.Point(132, 15);
            this.txtEarthquakeName.Name = "txtEarthquakeName";
            this.txtEarthquakeName.Size = new System.Drawing.Size(262, 21);
            this.txtEarthquakeName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "地震名称";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "物资贮备分布点";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "灾区位置分布点";
            // 
            // txtFacilityLoc
            // 
            this.txtFacilityLoc.Location = new System.Drawing.Point(132, 71);
            this.txtFacilityLoc.Name = "txtFacilityLoc";
            this.txtFacilityLoc.ReadOnly = true;
            this.txtFacilityLoc.Size = new System.Drawing.Size(262, 21);
            this.txtFacilityLoc.TabIndex = 0;
            this.txtFacilityLoc.Click += new System.EventHandler(this.txtFacilityLoc_Click);
            // 
            // txtIncidentLoc
            // 
            this.txtIncidentLoc.Location = new System.Drawing.Point(132, 102);
            this.txtIncidentLoc.Name = "txtIncidentLoc";
            this.txtIncidentLoc.ReadOnly = true;
            this.txtIncidentLoc.Size = new System.Drawing.Size(262, 21);
            this.txtIncidentLoc.TabIndex = 0;
            this.txtIncidentLoc.Click += new System.EventHandler(this.txtIncidentLoc_Click);
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(205, 320);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(75, 23);
            this.btnCalculate.TabIndex = 3;
            this.btnCalculate.Text = "计算(&O)";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(319, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "取消(&C)";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "地震时间";
            // 
            // dtEarthquakeDate
            // 
            this.dtEarthquakeDate.Location = new System.Drawing.Point(132, 42);
            this.dtEarthquakeDate.Name = "dtEarthquakeDate";
            this.dtEarthquakeDate.Size = new System.Drawing.Size(262, 21);
            this.dtEarthquakeDate.TabIndex = 4;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "输出目录";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(132, 134);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.ReadOnly = true;
            this.txtOutputFolder.Size = new System.Drawing.Size(262, 21);
            this.txtOutputFolder.TabIndex = 0;
            this.txtOutputFolder.Click += new System.EventHandler(this.txtOutputFolder_Click);
            // 
            // paraPanel
            // 
            this.paraPanel.Location = new System.Drawing.Point(14, 171);
            this.paraPanel.Name = "paraPanel";
            this.paraPanel.Size = new System.Drawing.Size(382, 132);
            this.paraPanel.TabIndex = 5;
            // 
            // FormDispatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 353);
            this.Controls.Add(this.paraPanel);
            this.Controls.Add(this.dtEarthquakeDate);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.txtIncidentLoc);
            this.Controls.Add(this.txtFacilityLoc);
            this.Controls.Add(this.txtEarthquakeName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormDispatch";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "饮用水供给方案";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEarthquakeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFacilityLoc;
        private System.Windows.Forms.TextBox txtIncidentLoc;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtEarthquakeDate;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Panel paraPanel;
    }
}