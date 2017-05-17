using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace DisasterModel.SitesCol
{
    public class RefugeeSiteWaterCol : RefugeeSiteCol
    {
        public static string PopulationField = "灾区人口";
        public static int WaterQuota = 2;
        public static int DaysInShort = 10;
 
        private Earthquake _earthquake;
        private SeasonCoefficient _seasonCoffe;
        private RegionCoefficient _regionCoffe;

        int _idxPop = -1;
      
        internal void Setup(Dispatcher dispatcher, int daysInShort, int quota)
        {
            this._fc = dispatcher.SiteFeatureClass;
            this._earthquake = dispatcher.Earthquake;
            RefugeeSiteWaterCol.DaysInShort = daysInShort;
            RefugeeSiteWaterCol.WaterQuota = quota;

            this._seasonCoffe = dispatcher.Coe;
            this._regionCoffe = dispatcher.Region;
            _idxPop = _fc.Fields.FindField(PopulationField);
            _refugeeSites = GetRefugeeSites();
            
        }
        protected override RefugeeSite CreateSite(IFeature feature)
        {
            int popu = (int)(feature.get_Value(_idxPop));

            RefugeeSiteWater site = new RefugeeSiteWater()
            {
                OID = feature.OID,
                Location = feature.ShapeCopy as IPoint,
                Population = popu
            };

            site.Priority = this._regionCoffe.GetRegionCoefficient(site.Location);

            site.ResourceInNeed = (int) (site.Population * RefugeeSiteWaterCol.DaysInShort * WaterQuota *
                 _seasonCoffe.GetSiteCoeffecient(site.Location, _earthquake.GetOccurMonth()));
            return site;
        }

       
    }
}
