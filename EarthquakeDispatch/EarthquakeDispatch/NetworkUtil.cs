using System;
using System.Collections.Generic;
using System.Text;

namespace EarthquakeDispatch
{
    class NetworkUtil
    {
      
        ///<summary>Create a new closest facility layer.</summary>
        ///   
        ///<param name="networkDataset">An INetworkDataset interface that is the network dataset on which to perform the closest facility analysis.</param>
        ///   
        ///<returns>An INALayer3 interface that is the newly created network analysis layer.</returns>
        public static ESRI.ArcGIS.NetworkAnalyst.INALayer3 CreateClosestFacilityLayer(ESRI.ArcGIS.Geodatabase.INetworkDataset networkDataset)
        {
            ESRI.ArcGIS.NetworkAnalyst.INAClosestFacilitySolver naClosesestFacilitySolver = new ESRI.ArcGIS.NetworkAnalyst.NAClosestFacilitySolverClass();
            ESRI.ArcGIS.NetworkAnalyst.INASolver naSolver = naClosesestFacilitySolver as ESRI.ArcGIS.NetworkAnalyst.INASolver;

            ESRI.ArcGIS.Geodatabase.IDatasetComponent datasetComponent = networkDataset as ESRI.ArcGIS.Geodatabase.IDatasetComponent; // Dynamic Cast
            ESRI.ArcGIS.Geodatabase.IDENetworkDataset deNetworkDataset = datasetComponent.DataElement as ESRI.ArcGIS.Geodatabase.IDENetworkDataset; // Dynamic Cast
            ESRI.ArcGIS.NetworkAnalyst.INAContext naContext = naSolver.CreateContext(deNetworkDataset, naSolver.Name);
            ESRI.ArcGIS.NetworkAnalyst.INAContextEdit naContextEdit = naContext as ESRI.ArcGIS.NetworkAnalyst.INAContextEdit; // Dynamic Cast

            ESRI.ArcGIS.Geodatabase.IGPMessages gpMessages = new ESRI.ArcGIS.Geodatabase.GPMessagesClass();
            naContextEdit.Bind(networkDataset, gpMessages);

            ESRI.ArcGIS.NetworkAnalyst.INALayer naLayer = naSolver.CreateLayer(naContext);
            ESRI.ArcGIS.NetworkAnalyst.INALayer3 naLayer3 = naLayer as ESRI.ArcGIS.NetworkAnalyst.INALayer3; // Dynamic Cast

            return naLayer3;
        }
    }
}
