using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using log4net;

namespace DisasterModel
{
    public class Dispatcher
    {
        Earthquake _quake = null;
        RegionCoefficient _region = null;
        SeasonCoefficient _season = null;

        RefugeeSiteCol _refugeSiteCol = null;
        RepositoryCol _repositoryCol = null;
        SupplyNetwork _supplyNetwork = null;
        RoadNetwork _roadNetwork = null;

        string templatePath = @"Template";
        string outputPath = @"F:\17\private\Disaster\Data\output";
        private IFeatureClass _outputFC;

        public Dispatcher()
        {
            LogHelper.Error("error1 helloWorld");
            LogHelper.Info("info1 helloworld");

            Exception ex = new Exception("ex hellowolrd");
            LogHelper.Error(ex);
            LogHelper.Info(ex);
        }

        public void Setup()
        {

            string networkWorkspace = @"F:\17\private\Disaster\Data\road";
            string networkClassName = "road_ND";

            string quakePath = @"F:\17\private\Disaster\Data\EarthquakeData";
            string regionClassName = "地区系数";
            string seasonClassName = "季节系数";
            string repoClassName = "物资贮备分布点";
            string siteClassName = "灾区位置分布点";

            string quakeName = "test";
            DateTime quakeTime = DateTime.Now;
            int daysInShort = 10;
            _quake = GetEarthquake(quakeName, quakeTime, daysInShort);

            SetupOutputDirectory();
            IFeatureWorkspace ws = WorkspaceUtil.OpenShapeWorkspace(quakePath);

            _region = GetRegionCoefficient(ws, regionClassName);
            _season = GetSeasonCoefficient(ws, seasonClassName);

            _refugeSiteCol = GetRefugeeSiteCol(ws, siteClassName);
            _repositoryCol = GetRepositoryCol(ws, repoClassName);

            _roadNetwork = GetRoadNetwork(networkWorkspace, networkClassName);
            _supplyNetwork = GetSupplyNetwork();
        }

        private bool SetupOutputDirectory()
        {
            try
            {
                if (System.IO.Directory.Exists(outputPath))
                {
                    System.IO.Directory.Delete(outputPath, true);
                }

                System.IO.Directory.CreateDirectory(outputPath);

                string outputWorkspace = System.IO.Path.Combine(outputPath, DisasterModel.Properties.Resources.ResultDBName);
                string templateMdb = System.IO.Path.Combine(templatePath, DisasterModel.Properties.Resources.ResultDBName);
                System.IO.File.Copy(templateMdb, outputWorkspace);

                string templateDoc = System.IO.Path.Combine(templatePath, DisasterModel.Properties.Resources.MapDocName);
                string outDoc = GetOutputMapLoc();
                System.IO.File.Copy(templateDoc, outDoc);

                this._outputFC = WorkspaceUtil.OpenMDBWorkspace(outputWorkspace).OpenFeatureClass(
                     DisasterModel.Properties.Resources.ResultFeatureClass);
                return true;
            }
            catch (Exception ex)
            {
//TODO: ex
                throw ex;
            }
        }

        private string GetOutputMapLoc()
        {
            string outDoc = System.IO.Path.Combine(outputPath, DisasterModel.Properties.Resources.MapDocName);
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

        public void Dispatch()
        {
            foreach (var site in _refugeSiteCol.Sites)
            {
                _supplyNetwork.SupplyRefugeeSite(site);
            }
        }


        public ESRI.ArcGIS.Carto.IMap GetMap()
        {
            IMapDocument mapDocu = new MapDocumentClass();
            mapDocu.Open(GetOutputMapLoc());
            IMap map = mapDocu.Map[0];

            IFeatureLayer incidentsLayer = FindLayer(map, "灾区位置分布点");
            incidentsLayer.FeatureClass = _refugeSiteCol.FeatureClass;

            IFeatureLayer facilityLayer = FindLayer(map, "物资贮备分布点");
            facilityLayer.FeatureClass = this._repositoryCol.FeatureClass;
            
            return map;
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
    }
}
