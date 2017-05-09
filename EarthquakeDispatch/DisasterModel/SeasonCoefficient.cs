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
        public void SetClass(IFeatureClass coefficients)
        {
            _fcCoefficient = coefficients;
        }

        public double GetSiteCoeffecient(IPoint site, double month)
        {
            return 0;
        }
    }
}
