using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace DisasterModel.Forms
{
    public class RefugeeSiteFoodCol:RefugeeSiteCol
    {
        public static string PopulationField = "灾区人口";
        public static int FoodQuota = 3;
        public static int DaysInShort = 10;

        private Earthquake _earthquake;
        private SeasonCoefficient _seasonCoffe;
        private RegionCoefficient _regionCoffe;

        int _idxPop = -1;

        internal void Setup(Dispatcher dispatcher, int daysInShort, int quota)
        {
            this._fc = dispatcher.SiteFeatureClass;
            this._earthquake = dispatcher.Earthquake;
            RefugeeSiteFoodCol.DaysInShort = daysInShort;
            RefugeeSiteFoodCol.FoodQuota = quota;

            this._regionCoffe = dispatcher.Region;
            _idxPop = _fc.Fields.FindField(PopulationField);
            _refugeeSites = GetRefugeeSites();

        }
        protected override RefugeeSite CreateSite(IFeature feature)
        {
            int popu = (int)(feature.get_Value(_idxPop));

            RefugeeSiteFood site = new RefugeeSiteFood()
            {
                OID = feature.OID,
                Location = feature.ShapeCopy as IPoint,
                Population = popu
            };

            site.Priority = this._regionCoffe.GetRegionCoefficient(site.Location);

            site.ResourceInNeed = (int)(site.Population * RefugeeSiteFoodCol.DaysInShort * FoodQuota); 
            return site;
        }
    }
}
