namespace DisasterModel
{
    partial class FormDispatchInput
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
            this.label2 = new System.Windows.Forms.Label();
            this.nmDaysInShort = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFacilityLoc = new System.Windows.Forms.TextBox();
            this.txtIncidentLoc = new System.Windows.Forms.TextBox();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dtEarthquakeDate = new System.Windows.Forms.DateTimePicker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.nmDaysInShort)).BeginInit();
            this.SuspendLayout();
            // 
            // txtEarthquakeName
            // 
            this.txtEarthquakeName.Location = new System.Drawing.Point(132, 35);
            this.txtEarthquakeName.Name = "txtEarthquakeName";
            this.txtEarthquakeName.Size = new System.Drawing.Size(262, 21);
            this.txtEarthquakeName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "地震名称";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "保障天数";
            // 
            // nmDaysInShort
            // 
            this.nmDaysInShort.Location = new System.Drawing.Point(132, 93);
            this.nmDaysInShort.Name = "nmDaysInShort";
            this.nmDaysInShort.Size = new System.Drawing.Size(262, 21);
            this.nmDaysInShort.TabIndex = 2;
            this.nmDaysInShort.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "物资贮备分布点";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "灾区位置分布点";
            // 
            // txtFacilityLoc
            // 
            this.txtFacilityLoc.Location = new System.Drawing.Point(132, 123);
            this.txtFacilityLoc.Name = "txtFacilityLoc";
            this.txtFacilityLoc.ReadOnly = true;
            this.txtFacilityLoc.Size = new System.Drawing.Size(262, 21);
            this.txtFacilityLoc.TabIndex = 0;
            this.txtFacilityLoc.Click += new System.EventHandler(this.txtFacilityLoc_Click);
            // 
            // txtIncidentLoc
            // 
            this.txtIncidentLoc.Location = new System.Drawing.Point(132, 154);
            this.txtIncidentLoc.Name = "txtIncidentLoc";
            this.txtIncidentLoc.ReadOnly = true;
            this.txtIncidentLoc.Size = new System.Drawing.Size(262, 21);
            this.txtIncidentLoc.TabIndex = 0;
            this.txtIncidentLoc.Click += new System.EventHandler(this.txtIncidentLoc_Click);
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(210, 233);
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
            this.button1.Location = new System.Drawing.Point(319, 233);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "取消(&C)";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(61, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "地震时间";
            // 
            // dtEarthquakeDate
            // 
            this.dtEarthquakeDate.Location = new System.Drawing.Point(132, 66);
            this.dtEarthquakeDate.Name = "dtEarthquakeDate";
            this.dtEarthquakeDate.Size = new System.Drawing.Size(262, 21);
            this.dtEarthquakeDate.TabIndex = 4;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FormDispatchInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 314);
            this.Controls.Add(this.dtEarthquakeDate);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.nmDaysInShort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtIncidentLoc);
            this.Controls.Add(this.txtFacilityLoc);
            this.Controls.Add(this.txtEarthquakeName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormDispatchInput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "饮用水供给方案";
            ((System.ComponentModel.ISupportInitialize)(this.nmDaysInShort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEarthquakeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nmDaysInShort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFacilityLoc;
        private System.Windows.Forms.TextBox txtIncidentLoc;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtEarthquakeDate;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}