using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace DisasterModel
{
    public class Earthquake
    {
        public string Name { get; set; }

        public DateTime DateTime { get; set; }

        public int GetOccurMonth()
        {
            return DateTime.Month; 
        }

        public Earthquake()
        {
            Name = "";
        }
    }

 
}
