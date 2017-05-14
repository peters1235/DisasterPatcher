using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace DisasterModel
{
    public class SeasonCoefficient
    {
        private IFeatureClass _fcCoefficient = null;

        public SeasonCoefficient(IFeatureClass iFeatureClass)
        {
            this._fcCoefficient = iFeatureClass;
        }

        public void SetClass(IFeatureClass coefficients)
        {
            _fcCoefficient = coefficients;
        }

        public double GetSiteCoeffecient(IPoint site, double month)
        {
            ISpatialFilter filter = new SpatialFilterClass();
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;

            IFeatureCursor cursor = _fcCoefficient.Search(filter, true);
            IFeature f = cursor.NextFeature();
            int idx = _fcCoefficient.FindField(month.ToString());
            double seasonCoe = double.Parse(f.get_Value(idx).ToString());
            if (seasonCoe <= 0)
            {
                seasonCoe = 1;
            }
            return seasonCoe;
        }
    }
}
