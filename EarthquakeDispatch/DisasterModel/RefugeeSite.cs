using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace DisasterModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RefugeeSite
    {
      
        public int Population { get; set; }

        public int PeopleNeedTent { get; set; }

        public int OID { get; set; }

        public IPoint Location { get; set; }

        double _waterInNeed = 0;
        public double WaterInNeed
        {
            get
            {
                return _waterInNeed;
            }
            set
            {
                _waterInNeed = value;
            }
        }

        public double Priority
        {
            get;set;
          
        }
    }
}
