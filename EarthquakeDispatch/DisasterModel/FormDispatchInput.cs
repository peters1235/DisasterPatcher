using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DisasterModel
{
    public partial class FormDispatchInput : Form
    {
        Dispatcher _dispatcher = null;
        public FormDispatchInput()
        {
            InitializeComponent();
        }

        public Dispatcher Dispatcher { get { return _dispatcher; } }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                Earthquake quake = new Earthquake()
                {
                    DateTime = dtEarthquakeDate.Value,
                    DaysInShort = (int)nmDaysInShort.Value,
                    Name = txtEarthquakeName.Text
                };

                _dispatcher = new Dispatcher();
                string incidentData = txtIncidentLoc.Text;
                string facilityData = txtFacilityLoc.Text;
                if (_dispatcher.Setup(quake, facilityData, incidentData))
                {
                    _dispatcher.Dispatch(EnumResource.Water);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }

        }

        private void txtFacilityLoc_Click(object sender, EventArgs e)
        {
            txtFacilityLoc.Text = GetFile();
        }

        private string GetFile()
        {
            openFileDialog1.Filter = "Shp文件|*.shp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                return path;
            }
            return null;
        }

        private void txtIncidentLoc_Click(object sender, EventArgs e)
        {
            txtIncidentLoc.Text = GetFile();
        }
    }
}
