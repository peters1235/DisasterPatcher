using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace DisasterModel.SitesCol
{
    public class RefugeeSiteTentCol : RefugeeSiteCol
    {
        public static string UrgentPopField = "转移人口";

        int _idxUrgentPop = -1;
        private Earthquake _earthquake;
        private RegionCoefficient _regionCoffe;
        private static int PersonPerTent = 6;
        internal void Setup(Dispatcher _dispatcher, int shareTent)
        {
            this._fc = _dispatcher.SiteFeatureClass;
            this._earthquake = _dispatcher.Earthquake;
            this._regionCoffe = _dispatcher.Region;
            _idxUrgentPop = _fc.Fields.FindField(UrgentPopField); 
            _refugeeSites = GetRefugeeSites();
          
            RefugeeSiteTentCol.PersonPerTent = shareTent;
        }

        protected override RefugeeSite CreateSite(IFeature feature)
        {
            int urgentPop = (int)(feature.get_Value(_idxUrgentPop));

            RefugeeSiteTent site = new RefugeeSiteTent()
            {
                OID = feature.OID,
                Location = feature.ShapeCopy as IPoint,
                PeopleNeedTent = urgentPop,
            };

            site.Priority = this._regionCoffe.GetRegionCoefficient(site.Location);

            site.ResourceInNeed = (int)Math.Ceiling(1.0 * site.PeopleNeedTent / RefugeeSiteTentCol.PersonPerTent);

            return site;
        }


      
    }
}
