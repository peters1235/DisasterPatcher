using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisasterModel.Forms
{
   public class UCFood : UCParas
    {
        private System.Windows.Forms.NumericUpDown nmDaysInShort;
        private System.Windows.Forms.NumericUpDown nmFoodQuota;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;

       private void InitializeComponent()
       {
           this.nmDaysInShort = new System.Windows.Forms.NumericUpDown();
           this.nmFoodQuota = new System.Windows.Forms.NumericUpDown();
           this.label6 = new System.Windows.Forms.Label();
           this.label2 = new System.Windows.Forms.Label();
           ((System.ComponentModel.ISupportInitialize)(this.nmDaysInShort)).BeginInit();
           ((System.ComponentModel.ISupportInitialize)(this.nmFoodQuota)).BeginInit();
           this.SuspendLayout();
           // 
           // nmDaysInShort
           // 
           this.nmDaysInShort.Location = new System.Drawing.Point(122, 43);
           this.nmDaysInShort.Name = "nmDaysInShort";
           this.nmDaysInShort.Size = new System.Drawing.Size(231, 21);
           this.nmDaysInShort.TabIndex = 9;
           this.nmDaysInShort.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
           // 
           // nmWaterQuota
           // 
           this.nmFoodQuota.Location = new System.Drawing.Point(122, 15);
           this.nmFoodQuota.Name = "nmWaterQuota";
           this.nmFoodQuota.Size = new System.Drawing.Size(231, 21);
           this.nmFoodQuota.TabIndex = 10;
           this.nmFoodQuota.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
           // 
           // label6
           // 
           this.label6.AutoSize = true;
           this.label6.Location = new System.Drawing.Point(9, 17);
           this.label6.Name = "label6";
           this.label6.Size = new System.Drawing.Size(77, 12);
           this.label6.TabIndex = 7;
           this.label6.Text = "每人每天包数";
           // 
           // label2
           // 
           this.label2.AutoSize = true;
           this.label2.Location = new System.Drawing.Point(51, 45);
           this.label2.Name = "label2";
           this.label2.Size = new System.Drawing.Size(53, 12);
           this.label2.TabIndex = 8;
           this.label2.Text = "保障天数";
           // 
           // UCFood
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
           this.Controls.Add(this.nmDaysInShort);
           this.Controls.Add(this.nmFoodQuota);
           this.Controls.Add(this.label6);
           this.Controls.Add(this.label2);
           this.Name = "UCFood";
           this.Size = new System.Drawing.Size(383, 110);
           ((System.ComponentModel.ISupportInitialize)(this.nmDaysInShort)).EndInit();
           ((System.ComponentModel.ISupportInitialize)(this.nmFoodQuota)).EndInit();
           this.ResumeLayout(false);
           this.PerformLayout();

       }

       public UCFood()
       {
           InitializeComponent();
       }

       public int DaysInShort { get { return (int)nmDaysInShort.Value; } }

       public int Quota { get { return (int)nmFoodQuota.Value; } }
   }
}
