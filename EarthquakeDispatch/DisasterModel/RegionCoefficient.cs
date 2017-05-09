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
        private IFeatureClass _fcRegion = null;
        public void SetClass(IFeatureClass fc)
        {
            _fcRegion = fc;
        }

        public double GetRegionCoefficient(IPoint pt)
        {
            return 0;
        }
    }
}
