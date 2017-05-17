using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisasterModel.Forms
{
    class UCTents : UCParas
    {
        private System.Windows.Forms.NumericUpDown nmShareTent;
        private System.Windows.Forms.Label label6;

        public int ShareTent { get { return (int)nmShareTent.Value; } }
        public UCTents()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.nmShareTent = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nmShareTent)).BeginInit();
            this.SuspendLayout();
            // 
            // nmShareTent
            // 
            this.nmShareTent.Location = new System.Drawing.Point(131, 37);
            this.nmShareTent.Name = "nmShareTent";
            this.nmShareTent.Size = new System.Drawing.Size(231, 21);
            this.nmShareTent.TabIndex = 8;
            this.nmShareTent.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "几人一顶";
            // 
            // UCTents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Controls.Add(this.nmShareTent);
            this.Controls.Add(this.label6);
            this.Name = "UCTents";
            this.Size = new System.Drawing.Size(365, 94);
            ((System.ComponentModel.ISupportInitialize)(this.nmShareTent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
