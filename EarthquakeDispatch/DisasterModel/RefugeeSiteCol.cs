using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace DisasterModel
{
    public class RefugeeSiteCol
    {
        public static string PopulationField = "灾区人口";
        public static string UrgentPopField = "转移人口";
        public static int WaterQuota = 2;

        List<RefugeeSite> _refugeeSites = null;
        private IFeatureClass _fc;

        private Earthquake _earthquake;
        private SeasonCoefficient _seasonCoffe;
        private RegionCoefficient _regionCoffe;

        public void Setup(IFeatureClass fc, Earthquake earthquake, RegionCoefficient region, SeasonCoefficient coe)
        {
            this._fc = fc;
            this._earthquake = earthquake;
            this._seasonCoffe = coe;
            this._regionCoffe = region;
            _refugeeSites = GetRefugeeSites();
        }

        private List<RefugeeSite> GetRefugeeSites()
        {
            List<RefugeeSite> results = new List<RefugeeSite>();
            IFeatureCursor cursor = _fc.Search(null, true);
            IFeature row = cursor.NextFeature();

            int idxPop = _fc.Fields.FindField(PopulationField);
            int idxUrgentPop = _fc.Fields.FindField(UrgentPopField);

            while (row != null)
            {
                int popu = (int)(row.get_Value(idxPop));
                int urgentPop = (int)(row.get_Value(idxUrgentPop));

                RefugeeSite site = new RefugeeSite()
                {
                    ID = row.OID,
                    Location = row.ShapeCopy as IPoint,
                    PeopleNeedTent = urgentPop,
                    Population = popu
                };

                site.Priority = this._regionCoffe.GetRegionCoefficient(site.Location);

                site.WaterInNeed = site.Population * _earthquake.DaysInShort * WaterQuota *
                     _seasonCoffe.GetSiteCoeffecient(site.Location, _earthquake.GetOccurMonth());

                int insertIndex = GetInsertIndex(results, site);
                results.Insert(insertIndex, site);

                row = cursor.NextFeature();
            }
            return results;
        }

        private int GetInsertIndex(List<RefugeeSite> col, RefugeeSite site)
        {
            int index = 0;
            for (int i = 0; i < col.Count; i++)
            {
                RefugeeSite s = col[i];
                if (site.Priority < s.Priority)
                {
                    index++;
                }
            }
            return index;
        }

        public List<RefugeeSite> Sites { get { return _refugeeSites; } }

        internal IQueryFilter SiteFilter(RefugeeSite site)
        {
            IQueryFilter filter = new QueryFilterClass();
            filter.WhereClause = _fc.OIDFieldName + " = " + site.ID.ToString();
            return filter;
        }

        public IFeatureClass FeatureClass { get { return _fc; } }

        internal void ReplenishWater(RefugeeSite site, double amount)
        {
            site.WaterInNeed -= amount;
        }
    }
}
