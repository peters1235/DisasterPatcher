using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DisasterModel
{
    public class UCWater:UCParas
    {
        private System.Windows.Forms.NumericUpDown nmDaysInShort;
        private System.Windows.Forms.NumericUpDown nmWaterQuota;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;

        public UCWater()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.nmDaysInShort = new System.Windows.Forms.NumericUpDown();
            this.nmWaterQuota = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nmDaysInShort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmWaterQuota)).BeginInit();
            this.SuspendLayout();
            // 
            // nmDaysInShort
            // 
            this.nmDaysInShort.Location = new System.Drawing.Point(124, 45);
            this.nmDaysInShort.Name = "nmDaysInShort";
            this.nmDaysInShort.Size = new System.Drawing.Size(231, 21);
            this.nmDaysInShort.TabIndex = 5;
            this.nmDaysInShort.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // nmWaterQuota
            // 
            this.nmWaterQuota.Location = new System.Drawing.Point(124, 17);
            this.nmWaterQuota.Name = "nmWaterQuota";
            this.nmWaterQuota.Size = new System.Drawing.Size(231, 21);
            this.nmWaterQuota.TabIndex = 6;
            this.nmWaterQuota.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "每人每天用水(L)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "保障天数";
            // 
            // UCWater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Controls.Add(this.nmDaysInShort);
            this.Controls.Add(this.nmWaterQuota);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Name = "UCWater";
            this.Size = new System.Drawing.Size(379, 84);
            ((System.ComponentModel.ISupportInitialize)(this.nmDaysInShort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmWaterQuota)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public int DaysInShort { get { return (int)nmDaysInShort.Value; } }

        public int Quota { get { return (int)nmWaterQuota.Value; } }
    }
}
