using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using log4net;
using System.Windows.Forms;

namespace DisasterModel
{
    public class Dispatcher
    {
        Earthquake _quake = null;

        string _facilityClassName = "物资贮备分布点";
        string _incidentClassName = "灾区位置分布点";
        RegionCoefficient _region = null;
        SeasonCoefficient _season = null;

        RefugeeSiteCol _refugeSiteCol = null;
        RepositoryCol _repositoryCol = null;
        SupplyNetwork _supplyNetwork = null;
        RoadNetwork _roadNetwork = null;

        string templatePath = System.IO.Path.Combine(Application.StartupPath, "Template");
        string outputPath = @"F:\17\private\Disaster\Data\output";
        private IFeatureClass _outputFC;

        internal bool Setup(Earthquake quake, string facilityData, string incidentData)
        {
            string networkWorkspace = System.IO.Path.Combine(Application.StartupPath,
        @"AnalystData\road");// @"F:\17\private\Disaster\Data\road";
            string networkClassName = "road_ND";

            string analystDataPath = System.IO.Path.Combine(Application.StartupPath,
                "AnalystData");// @"F:\17\private\Disaster\Data\EarthquakeData";
            string regionClassName = "地区系数";
            string seasonClassName = "季节系数";

            _quake = quake;
            if (!SetupOutputDirectory())
            {
                System.Windows.Forms.MessageBox.Show("初始化输出目录失败");
                return false;
            }
            IFeatureWorkspace ws = WorkspaceUtil.OpenShapeWorkspace(analystDataPath);

            _region = GetRegionCoefficient(ws, regionClassName);
            _season = GetSeasonCoefficient(ws, seasonClassName);

            _incidentWorkspace = System.IO.Path.GetDirectoryName(incidentData);
            _incidentClassName = System.IO.Path.GetFileNameWithoutExtension(incidentData);
            ws = WorkspaceUtil.OpenShapeWorkspace(_incidentWorkspace);
            _refugeSiteCol = GetRefugeeSiteCol(ws, _incidentClassName);

            _repoWorkspace = System.IO.Path.GetDirectoryName(facilityData);
            _facilityClassName = System.IO.Path.GetFileNameWithoutExtension(facilityData);
            ws = WorkspaceUtil.OpenShapeWorkspace(_repoWorkspace);
            _repositoryCol = GetRepositoryCol(ws, _facilityClassName);

            _roadNetwork = GetRoadNetwork(networkWorkspace, networkClassName);
            _supplyNetwork = GetSupplyNetwork();

            return true;
        }

        private bool SetupOutputDirectory()
        {
            try
            {
                if (System.IO.Directory.Exists(outputPath))
                {
                    System.IO.Directory.Delete(outputPath, true);
                }
                while (!System.IO.Directory.Exists(outputPath))
                {
                    System.IO.Directory.CreateDirectory(outputPath);
                }
                string outputWorkspace = System.IO.Path.Combine(outputPath, DisasterModel.Properties.Resources.MDBName);
                string templateMdb = System.IO.Path.Combine(templatePath, DisasterModel.Properties.Resources.MDBName);
                System.IO.File.Copy(templateMdb, outputWorkspace);

                string templateDoc = System.IO.Path.Combine(templatePath, DisasterModel.Properties.Resources.MxdName);
                string outDoc = GetMxdLoc();
                System.IO.File.Copy(templateDoc, outDoc);

                string templateReport = System.IO.Path.Combine(templatePath, DisasterModel.Properties.Resources.DocName);
                string outReport = GetDocLoc();
                System.IO.File.Copy(templateReport, outReport);

                this._outputFC = WorkspaceUtil.OpenMDBWorkspace(outputWorkspace).OpenFeatureClass(
                     DisasterModel.Properties.Resources.RoutesClassName);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        private string GetDocLoc()
        {
            string outReport = System.IO.Path.Combine(outputPath, DisasterModel.Properties.Resources.DocName);
            return outReport;
        }

        private string GetMxdLoc()
        {
            string outDoc = System.IO.Path.Combine(outputPath, DisasterModel.Properties.Resources.MxdName);
            return outDoc;
        }

        private RepositoryCol GetRepositoryCol(IFeatureWorkspace ws, string repoClassName)
        {
            RepositoryCol col = new RepositoryCol();
            col.Setup(ws.OpenFeatureClass(repoClassName));
            return col;
        }

        private RefugeeSiteCol GetRefugeeSiteCol(IFeatureWorkspace ws, string siteClassName)
        {
            RefugeeSiteCol col = new RefugeeSiteCol();
            col.Setup(ws.OpenFeatureClass(siteClassName), _quake, _region, _season);
            return col;
        }

        private SeasonCoefficient GetSeasonCoefficient(IFeatureWorkspace ws, string seasonClassName)
        {
            return new SeasonCoefficient(ws.OpenFeatureClass(seasonClassName));
        }

        private RegionCoefficient GetRegionCoefficient(IFeatureWorkspace ws, string regionClassName)
        {
            RegionCoefficient coe = new RegionCoefficient(ws.OpenFeatureClass(regionClassName));
            return coe;
        }

        private Earthquake GetEarthquake(string name, DateTime dateTime, int daysInShort)
        {
            Earthquake quake = new Earthquake() { Name = name, DateTime = dateTime, DaysInShort = daysInShort };
            return quake;
        }

        private RoadNetwork GetRoadNetwork(string wsPath, string dsName)
        {
            RoadNetwork network = new RoadNetwork();
            network.Initialize(wsPath, dsName);
            return network;
        }

        private SupplyNetwork GetSupplyNetwork()
        {
            SupplyNetwork result = new SupplyNetwork();
            result.SetLocations(_refugeSiteCol, _repositoryCol, _roadNetwork);
            result.Init(_outputFC);
            return result;
        }

        public void Dispatch(EnumResource enumResource)
        {
            switch (enumResource)
            {
                case EnumResource.Water:
                    foreach (var site in _refugeSiteCol.Sites)
                    {
                        _supplyNetwork.SupplyWater(site);
                    }
                    break;
                case EnumResource.Food:
                    foreach (var site in _refugeSiteCol.Sites)
                    {
                        _supplyNetwork.SupplyFood(site);
                    }
                    break;
                case EnumResource.Tent:
                    foreach (var site in _refugeSiteCol.Sites)
                    {
                        _supplyNetwork.SupplyTents(site);
                    }
                    break;
                case EnumResource.Electricity:
                    break;
                case EnumResource.FireFighter:
                    break;
                case EnumResource.Rescue:
                    break;
                default:
                    break;
            }

        }

        IMap _map = null;
        private string _incidentWorkspace;
        private string _repoWorkspace;

        public ESRI.ArcGIS.Carto.IMap GetMap()
        {
            if (_map == null)
            {
                IMapDocument mapDocu = new MapDocumentClass();
                mapDocu.Open(GetMxdLoc());
                _map = mapDocu.Map[0];

                IFeatureLayer incidentsLayer = FindLayer(_map, "灾区位置分布点");
                incidentsLayer.FeatureClass = _refugeSiteCol.FeatureClass;

                IFeatureLayer facilityLayer = FindLayer(_map, "物资贮备分布点");
                facilityLayer.FeatureClass = this._repositoryCol.FeatureClass;

                //test
               // IGeoFeatureLayer geoLayer = FindLayer(_map, "Routes") as IGeoFeatureLayer;
                //geoLayer.DisplayAnnotation = false;
                //test

                incidentsLayer.Name = _incidentClassName;
                facilityLayer.Name = _facilityClassName;

                ILayer networkLayer = _roadNetwork.GetNetworkLayer();
                _map.AddLayer(networkLayer);
                _map.MoveLayer(networkLayer, 3);

            }
            return _map;
        }

        private IFeatureLayer FindLayer(IMap map, string layername)
        {
            for (int i = 0; i < map.LayerCount; i++)
            {
                ILayer lyr = map.get_Layer(i);
                if (lyr.Name == layername)
                {
                    return lyr as IFeatureLayer;
                }
            }
            return null;
        }

        public string OutputFolder { get { return outputPath; } }

        public void CreateReport(IActiveView view)
        {
            ExportActiveView export = new ExportActiveView();
            export.ExportActiveViewParameterized(view, 300, 1, "JPEG", GetImageLoc(), false);

            ExportToWord _errorExport = new ExportToWord();
            string loc = GetDocLoc();
            _errorExport.InitWord(loc);

            _errorExport.WriteWord("EarthquakeName", _quake.Name, false);
            _errorExport.WriteWord("DispatchSchema", GetDispatchSchema(), false);
            _errorExport.WriteWord("DispatchRoutes", GetImageLoc(), true);
            _errorExport.Finish();
        }

        private string GetDispatchSchema()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var route in _supplyNetwork.WaterRoutes)
            {
                sb.Append(string.Format("从物资贮备分布点{0}运送{1}{2}{3}至灾区位置分布点{4}\r\n", route.RepoID, route.Resource, route.Amount, route.Unit, route.IncidentID));
            }
            return sb.ToString();
        }

        private string GetImageLoc()
        {
            return System.IO.Path.Combine(outputPath, DisasterModel.Properties.Resources.ImgName);
        }

        public string EarthquakeName { get; set; }


    }
}
