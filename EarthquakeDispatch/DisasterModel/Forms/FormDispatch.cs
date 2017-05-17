using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DisasterModel.SitesCol;

namespace DisasterModel
{
    public partial class FormDispatch : Form
    {
        public FormDispatch()
        {
            InitializeComponent();

            txtEarthquakeName.Text = "he";
            txtFacilityLoc.Text = @"F:\17\private\Disaster\Data\EarthquakeData\物资贮备分布点.shp";
            txtIncidentLoc.Text = @"F:\17\private\Disaster\Data\EarthquakeData\灾区位置分布点.shp";
            txtOutputFolder.Text = GetNonExist();
            _ucParas = GetUC();
            _ucParas.Dock = DockStyle.Fill;
            paraPanel.Controls.Add(_ucParas);
        }

        protected virtual UCParas GetUC()
        {
            throw new NotImplementedException();
        }

        protected UCParas _ucParas = null;
        protected Dispatcher _dispatcher = null;
        public Dispatcher Dispatcher { get { return _dispatcher; } }

        protected string GetFile(OpenFileDialog openFileDialog1)
        {
            openFileDialog1.Filter = "Shp文件|*.shp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                return path;
            }
            return null;
        }

        protected string GetFolder(FolderBrowserDialog folderBrowserDialog1)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return folderBrowserDialog1.SelectedPath;
            }
            return "";
        }

        private string GetNonExist()
        {
            string dir = @"F:\17\private\Disaster\Data\output\o";
            while (System.IO.Directory.Exists(dir))
            {
                dir += "1";
            }
            return dir;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            Dispatch();
        }

        private void Dispatch()
        {
            try
            {
                Earthquake quake = new Earthquake()
                {
                    DateTime = dtEarthquakeDate.Value,
                    Name = txtEarthquakeName.Text
                };

                string incidentData = txtIncidentLoc.Text;
                string facilityData = txtFacilityLoc.Text;
                string outputFolder = txtOutputFolder.Text;

                _dispatcher = new Dispatcher();
                _dispatcher.OutputFolder = outputFolder;
                if (_dispatcher.Setup(quake, facilityData, incidentData))
                {
                    RefugeeSiteCol siteCol = GetSiteCol();
                    _dispatcher.SetReportName(siteCol);
                    RepositoryCol repoCol = GetRepoCol();

                    SupplyNetwork supplyNetwork = new SupplyNetwork();
                    supplyNetwork.SetLocations(siteCol, repoCol, _dispatcher.RoadNetwork);
                    supplyNetwork.Init(_dispatcher.GetRoutesClass());

                    foreach (var site in siteCol.Sites)
                    {
                        supplyNetwork.SupplyResource(site);
                    }
                    _dispatcher.StoreResult(supplyNetwork.Routes);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        protected virtual RepositoryCol GetRepoCol()
        {
            throw new Exception(); 
        }

        protected virtual RefugeeSiteCol GetSiteCol()
        {
            throw new Exception(); 
        }

        private void txtFacilityLoc_Click(object sender, EventArgs e)
        {
            txtFacilityLoc.Text = GetFile(openFileDialog1);
        }

        private void txtIncidentLoc_Click(object sender, EventArgs e)
        {
            txtIncidentLoc.Text = GetFile(openFileDialog1);
        }

        private void txtOutputFolder_Click(object sender, EventArgs e)
        {
            txtOutputFolder.Text = GetFolder(folderBrowserDialog1);
        }

      
    }

    
}
