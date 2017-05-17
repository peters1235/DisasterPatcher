using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.ADF;

namespace DisasterModel
{
    public abstract class RefugeeSiteCol
    {
        protected List<RefugeeSite> _refugeeSites = null;
        protected IFeatureClass _fc;
        protected int GetInsertIndex(List<RefugeeSite> col, RefugeeSite site)
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
            filter.WhereClause = _fc.OIDFieldName + " = " + site.OID.ToString();
            return filter;
        }
        protected List<RefugeeSite> GetRefugeeSites()
        {
            List<RefugeeSite> results = new List<RefugeeSite>();
            IFeatureCursor cursor = null;
            try
            {
                cursor = _fc.Search(null, true);
                IFeature feature = cursor.NextFeature();

                while (feature != null)
                {
                    RefugeeSite site = CreateSite(feature);
                    int insertIndex = GetInsertIndex(results, site);
                    results.Insert(insertIndex, site);

                    feature = cursor.NextFeature();
                }
                return results;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
            finally
            {
                ComReleaser.ReleaseCOMObject(cursor);
            }
        }

        protected abstract RefugeeSite CreateSite(IFeature feature);

        public IFeatureClass FeatureClass { get { return _fc; } }

        internal void ReplenishResource(RefugeeSite site, int amount)
        {
            site.ResourceInNeed -= amount;
        }
    }
}
