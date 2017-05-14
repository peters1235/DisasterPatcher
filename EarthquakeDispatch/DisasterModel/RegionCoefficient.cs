using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace DisasterModel
{

    /// <summary>
    /// 地区系数
    /// </summary>
    public class RegionCoefficient
    {
        public static string CoeField = "地区系数";
        private IFeatureClass _fcRegion = null;
        private IFeatureClass _class;

        public RegionCoefficient(IFeatureClass iFeatureClass)
        {
            this._fcRegion = iFeatureClass;
        }

        public double GetRegionCoefficient(IPoint pt)
        {
            ISpatialFilter filter = new SpatialFilterClass();
            filter.Geometry = pt;
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;

            IFeatureCursor cursor = _fcRegion.Search(filter, true);
            IFeature f = cursor.NextFeature();
            int idxCoe = _fcRegion.FindField(CoeField);
            double coeff = double.Parse(f.get_Value(idxCoe).ToString());
            return coeff;
        }
    }
}
