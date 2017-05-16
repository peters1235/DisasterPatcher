using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace DisasterModel
{
    public class SupplyRoute
    {
        public object Amount { get; set; }
        public object Resource { get; set; }
        public object Unit { get; set; }   
        public int RepoID { get; set; }
        public IPolyline Route { get; set; }

        public static string ResourceField = "ResourceType";
        public static string AmountField = "Amount";
        public static string RepoIDField = "RepoID";
        public static string UnitField = "Unit";
        public static string IncidentIDField = "IncidentID";


        internal void SetMessagePara(string name, double amount, string unit)
        {
            this.Resource = name;
            this.Amount = amount;
            this.Unit = unit;
        }

        public int IncidentID { get; set; }
    }
}
