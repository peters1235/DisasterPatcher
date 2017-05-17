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
    public abstract class RefugeeSite
    {
        public int OID { get; set; }
        public IPoint Location { get; set; }
        public double Priority
        {
            get;
            set;
        }

        internal abstract string ResourceName();

        internal  int  ResourceInNeed{get;set;}

        internal abstract string ResourceUnit();
    }

    public class RefugeeSiteWater : RefugeeSite
    {
        public int Population { get; set; }

        internal override string ResourceName()
        {
            return "饮用水";
        }
        internal override string ResourceUnit()
        {
            return "L";
        }
      
    }

    public class RefugeeSiteFood : RefugeeSite
    {
        public int Population { get; set; }

        internal override string ResourceName()
        {
            return "方便食物";
        }
        internal override string ResourceUnit()
        {
            return "包";
        }

    }

    public class RefugeeSiteTent : RefugeeSite
    {
        public int PeopleNeedTent { get; set; }

        internal override string ResourceName()
        {
            return "帐篷";
        }
        internal override string ResourceUnit()
        {
            return "顶";
        }
    }
}
