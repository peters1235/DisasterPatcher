using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.NetworkAnalyst;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Carto;

namespace DisasterModel
{
    class RoadNetwork
    {
        private INAContext m_NAContext;
        private readonly string OUTPUTCLASSNAME = "CFRoutes";
        private IFeatureClass _facilityClass;
        private IFeatureClass _siteClass;
        INetworkDataset _networkDataset = null;

        /// <summary>
        /// Initialize the solver by calling the ArcGIS Network Analyst extension functions.
        /// </summary>
        public void Initialize(string workspacePath, string dsName)
        {
            this.Attribute = "minutes";

            IFeatureWorkspace featureWorkspace = null;

            try
            {
                // Open Geodatabase and network dataset
                IWorkspace workspace = null;// OpenGDBWorkspace(Application.StartupPath + @"\..\..\..\..\..\Data\SanFrancisco\SanFrancisco.gdb");
                //networkDataset = OpenNetworkDataset(workspace, "Transportation", "Streets_ND");

                workspace = WorkspaceUtil.OpenShapeWorkspace(workspacePath) as IWorkspace;

                _networkDataset = ShapefileToNetwork(workspace, dsName);

                featureWorkspace = workspace as IFeatureWorkspace;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Unable to open dataset. Error Message: " + ex.Message);
                return;
            }

            // Create NAContext and NASolver
            CreateSolverContext(_networkDataset);

            // Get available cost attributes from the network dataset
            INetworkAttribute networkAttribute;
            for (int i = 0; i < _networkDataset.AttributeCount; i++)
            {
                networkAttribute = _networkDataset.get_Attribute(i);
                if (networkAttribute.UsageType == esriNetworkAttributeUsageType.esriNAUTCost)
                {
                }
            }


            /*

            // Load incidents from a feature class
            featureWorkspace = OpenShapeWorkspace(@"F:\17\private\Disaster\震后基本应急物资与设备供应计算程序") as IFeatureWorkspace;
            //IFeatureClass inputFClass = featureWorkspace.OpenFeatureClass("Stores");
            IFeatureClass inputFClass = featureWorkspace.OpenFeatureClass("灾区位置分布点");

            LoadNANetworkLocations("Incidents", inputFClass, 500);

            // Load facilities from a feature class
            //inputFClass = featureWorkspace.OpenFeatureClass("FireStations");
            inputFClass = featureWorkspace.OpenFeatureClass("物资贮备分布点");
            LoadNANetworkLocations("Facilities", inputFClass, 500);

          
            //Create Layer for Network Dataset and add to ArcMap
            INetworkLayer networkLayer = new NetworkLayerClass();
            networkLayer.NetworkDataset = _networkDataset;
            var layer = networkLayer as ILayer;
            layer.Name = "Network Dataset";
            // axMapControl.AddLayer(layer, 0);

            //Create a Network Analysis Layer and add to ArcMap
            INALayer naLayer = m_NAContext.Solver.CreateLayer(m_NAContext);
            layer = naLayer as ILayer;
            layer.Name = m_NAContext.Solver.DisplayName;
            //axMapControl.AddLayer(layer, 0);
            */
        }

        public ILayer GetNetworkLayer()
        {
            INetworkLayer networkLayer = new NetworkLayerClass();
            networkLayer.NetworkDataset = _networkDataset;
            var layer = networkLayer as ILayer;
            layer.Name = "路网";
            return layer;
        }

        public ILayer GetNetworkAnalysisLayer()
        {
            INALayer naLayer = m_NAContext.Solver.CreateLayer(m_NAContext);
            ILayer layer = naLayer as ILayer;
            layer.Name = m_NAContext.Solver.DisplayName;
            return layer;
        }

        public void LoadAnalysisObjectsByGeometry(ESRI.ArcGIS.Geodatabase.IFeatureClass
    inputFeatureClass, string naClassName, ESRI.ArcGIS.NetworkAnalyst.INAContext
    naContext)
        {
            // Both Initialize and Load take a cursor from the input feature class
            ESRI.ArcGIS.Geodatabase.ICursor cursor = inputFeatureClass.Search(null, false) as
                ESRI.ArcGIS.Geodatabase.ICursor;

            // Initialize the default field mappings.
            // If you want to specify field mappings beyond the default ones, use naClassLoader.FieldMap to retrieve
            //  and edit the mappings between the input class and the naclass.
            ESRI.ArcGIS.NetworkAnalyst.INAClassLoader2 naClassLoader = new
                ESRI.ArcGIS.NetworkAnalyst.NAClassLoaderClass();
            naClassLoader.Initialize(naContext, naClassName, cursor);

            // Use ExcludeRestrictedElements and CacheRestrictedElements to prevent locations from being placed on restricted elements.
            // Some ways to restrict elements include restriction barriers and restriction attributes.
            // If you are loading barriers into barrier classes, or not loading locations (for example, seedpoints)
            //  then you should not exclude the restricted elements.  Also, if you do have barriers in your analysis problem,
            //  then you should load those first, to make sure the restricted elements are established before loading 
            //  non-barrier classes.
            ESRI.ArcGIS.NetworkAnalyst.INALocator3 naLocator3 = naClassLoader.Locator as
                ESRI.ArcGIS.NetworkAnalyst.INALocator3;
            naLocator3.ExcludeRestrictedElements = true;
            naLocator3.CacheRestrictedElements(naContext);

            // After Loading is complete, the rowsIn and rowsLocated variable can be used to verify
            //  that every row from the input feature class has been loaded into the network analysis class
            int rowsIn = 0;
            int rowsLocated = 0;
            naClassLoader.Load(cursor, null, ref rowsIn, ref rowsLocated);
        }

        public void LoadAnalysisObjectsByField(ESRI.ArcGIS.Geodatabase.ITable inputClass,
    string naClassName, ESRI.ArcGIS.NetworkAnalyst.INAContext naContext)
        {
            // Both Initialize and Load take a cursor from the input class
            ESRI.ArcGIS.Geodatabase.ICursor cursor = inputClass.Search(null, false) as
                ESRI.ArcGIS.Geodatabase.ICursor;
            ESRI.ArcGIS.NetworkAnalyst.INAClassLoader2 naClassLoader = new
                ESRI.ArcGIS.NetworkAnalyst.NAClassLoaderClass();
            naClassLoader.Initialize(naContext, naClassName, cursor);

            // Store the current set of locator agents, so they can be added back later
            int agentCount = naContext.Locator.LocatorAgentCount;
            var listOfAgents = new System.Collections.Generic.List<
                ESRI.ArcGIS.NetworkAnalyst.INALocatorAgent>();
            for (int locIndex = 0; locIndex < agentCount; locIndex++)
                listOfAgents.Add(naContext.Locator.get_LocatorAgent(locIndex));

            // Remove the existing locator agents from the locator
            // This for loop is done in reverse order, because agents are being removed as the loop executes
            for (int locIndex = agentCount - 1; locIndex >= 0; locIndex--)
                naContext.Locator.RemoveLocatorAgent(locIndex);

            // Create and add a fields agent
            var fieldsAgent = new
                ESRI.ArcGIS.NetworkAnalyst.NALocatorLocationFieldsAgentClass() as
                ESRI.ArcGIS.NetworkAnalyst.INALocatorLocationFieldsAgent2;

            // Set the field names appropriately based on input data and NAClass
            var naClass = naContext.NAClasses.get_ItemByName(naClassName) as
                ESRI.ArcGIS.NetworkAnalyst.INAClass;
            var naFeatureClass = naClass as ESRI.ArcGIS.Geodatabase.IFeatureClass;

            // Check to see if the NAClass is of type NALocation or NALocationRanges
            ESRI.ArcGIS.esriSystem.UID naLocationFeatureUID = new
                ESRI.ArcGIS.esriSystem.UIDClass();
            naLocationFeatureUID.Value = "esriNetworkAnalyst.NALocationFeature";
            ESRI.ArcGIS.esriSystem.UID naLocationFeatureRangesUID = new
                ESRI.ArcGIS.esriSystem.UIDClass();
            naLocationFeatureRangesUID.Value = "esriNetworkAnalyst.NALocationRangesFeature";
            if (naFeatureClass.CLSID.Compare(naLocationFeatureUID))
            {
                // The field names listed below are the names used in ArcGIS Network Analyst extension classes to represent NALocations.
                //  These are also the names of fields added by the CalculateLocations geoprocessing tool
                fieldsAgent.OIDFieldName = "SourceOID";
                fieldsAgent.SourceIDFieldName = "SourceID";
                fieldsAgent.PositionFieldName = "PosAlong";
                fieldsAgent.SideFieldName = "SideOfEdge";
            }
            else if (naFeatureClass.CLSID.Compare(naLocationFeatureRangesUID))
            {
                // The location ranges input field must be of type BLOB
                fieldsAgent.LocationRangesFieldName = "Locations";
                var blobField = inputClass.Fields.get_Field(inputClass.FindField
                    (fieldsAgent.LocationRangesFieldName));
                if (blobField.Type !=
                    ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeBlob)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "Loading location ranges by field requires a blob field");
                    return;
                }
            }
            naContext.Locator.AddLocatorAgent(fieldsAgent as
                ESRI.ArcGIS.NetworkAnalyst.INALocatorAgent);

            // After Loading is complete, the rowsIn and rowsLocated variable can be used to verify
            //  that every row from the input feature class has been loaded into the network analysis class
            int rowsIn = 0;
            int rowsLocated = 0;
            naClassLoader.Load(cursor, null, ref rowsIn, ref rowsLocated);

            // Now remove the custom fields agent and add back the stored agents
            naContext.Locator.RemoveLocatorAgent(0);
            foreach (var agent in listOfAgents)
                naContext.Locator.AddLocatorAgent(agent);

        }


        #region Set up Context and Solver

        /// <summary>
        /// Geodatabase function: open work space
        /// </summary>
        /// <param name="strGDBName">Input file name</param>
        /// <returns>Workspace</returns>
        public IWorkspace OpenGDBWorkspace(string strGDBName)
        {
            // As Workspace Factories are Singleton objects, they must be instantiated with the Activator
            var workspaceFactory = System.Activator.CreateInstance(System.Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory")) as ESRI.ArcGIS.Geodatabase.IWorkspaceFactory;

            if (!System.IO.Directory.Exists(strGDBName))
            {
                MessageBox.Show("The workspace: " + strGDBName + " does not exist", "Workspace Error");
                return null;
            }

            IWorkspace workspace = null;
            try
            {
                workspace = workspaceFactory.OpenFromFile(strGDBName, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Opening workspace failed: " + ex.Message, "Workspace Error");
            }

            return workspace;
        }

        /// <summary>
        /// Geodatabase function: open network dataset
        /// </summary>
        /// <param name="workspace">Input workspace</param>
        /// <param name="strNDSName">Input network dataset name</param>
        /// <returns>NetworkDataset</returns>
        public INetworkDataset OpenNetworkDataset(IWorkspace workspace, string featureDatasetName, string strNDSName)
        {
            // Obtain the dataset container from the workspace
            var featureWorkspace = workspace as IFeatureWorkspace;
            ESRI.ArcGIS.Geodatabase.IFeatureDataset featureDataset = featureWorkspace.OpenFeatureDataset(featureDatasetName);
            var featureDatasetExtensionContainer = featureDataset as ESRI.ArcGIS.Geodatabase.IFeatureDatasetExtensionContainer;
            ESRI.ArcGIS.Geodatabase.IFeatureDatasetExtension featureDatasetExtension = featureDatasetExtensionContainer.FindExtension(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTNetworkDataset);
            var datasetContainer3 = featureDatasetExtension as ESRI.ArcGIS.Geodatabase.IDatasetContainer3;

            // Use the container to open the network dataset.
            ESRI.ArcGIS.Geodatabase.IDataset dataset = datasetContainer3.get_DatasetByName(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTNetworkDataset, strNDSName);
            return dataset as ESRI.ArcGIS.Geodatabase.INetworkDataset;
        }

        private INetworkDataset ShapefileToNetwork(IWorkspace shpWS, string name)
        {
            IWorkspaceExtensionManager wsExtMgr = shpWS as IWorkspaceExtensionManager;
            UID myUID = new UIDClass();
            myUID.Value = "esriGeoDatabase.NetworkDatasetWorkspaceExtension";
            IWorkspaceExtension wsExt = wsExtMgr.FindExtension(myUID);
            IDatasetContainer2 dsCont = wsExt as IDatasetContainer2;
            IDataset dataset = dsCont.get_DatasetByName(esriDatasetType.esriDTNetworkDataset, name);
            INetworkDataset networkDataset = dataset as INetworkDataset;
            return networkDataset;
        }

        /// <summary>
        /// Geodatabase function: get network dataset
        /// </summary>
        /// <param name="networkDataset">Input network dataset</param>
        /// <returns>DE network dataset</returns>		
        public IDENetworkDataset GetDENetworkDataset(INetworkDataset networkDataset)
        {
            // Cast from the network dataset to the DatasetComponent
            IDatasetComponent dsComponent = networkDataset as IDatasetComponent;

            // Get the data element
            return dsComponent.DataElement as IDENetworkDataset;
        }

        /// <summary>
        /// Create NASolver and NAContext
        /// </summary>
        /// <param name="networkDataset">Input network dataset</param>
        /// <returns>NAContext</returns>
        public void CreateSolverContext(INetworkDataset networkDataset)
        {
            if (networkDataset == null) return;

            //Get the Data Element
            IDENetworkDataset deNDS = GetDENetworkDataset(networkDataset);

            INASolver naSolver = new NAClosestFacilitySolver();
            m_NAContext = naSolver.CreateContext(deNDS, naSolver.Name);
            ((INAContextEdit)m_NAContext).Bind(networkDataset, new GPMessagesClass());
        }

        /// <summary>
        /// Set solver settings
        /// </summary>
        /// <param name="strNAClassName">NAClass name</param>
        /// <param name="inputFC">Input feature class</param>
        /// <param name="maxSnapTolerance">Max snap tolerance</param>
        public void LoadNANetworkLocations(string strNAClassName, IFeatureClass inputFC, IQueryFilter filter, double maxSnapTolerance)
        {
            INamedSet classes = m_NAContext.NAClasses;
            INAClass naClass = classes.get_ItemByName(strNAClassName) as INAClass;

            // delete existing Locations except if that a barriers
            naClass.DeleteAllRows();

            // Create a NAClassLoader and set the snap tolerance (meters unit)
            INAClassLoader classLoader = new NAClassLoader();
            classLoader.Locator = m_NAContext.Locator;
            if (maxSnapTolerance > 0) ((INALocator3)classLoader.Locator).MaxSnapTolerance = maxSnapTolerance;
            classLoader.NAClass = naClass;

            //Create field map to automatically map fields from input class to NAClass
            INAClassFieldMap fieldMap = new NAClassFieldMapClass();
            fieldMap.CreateMapping(naClass.ClassDefinition, inputFC.Fields);
            classLoader.FieldMap = fieldMap;

            // Avoid loading network locations onto non-traversable portions of elements
            INALocator3 locator = m_NAContext.Locator as INALocator3;
            locator.ExcludeRestrictedElements = true;
            locator.CacheRestrictedElements(m_NAContext);

            //Load Network Locations
            int rowsIn = 0;
            int rowsLocated = 0;
            IFeatureCursor featureCursor = inputFC.Search(filter, true);
            classLoader.Load((ICursor)featureCursor, null, ref rowsIn, ref rowsLocated);

            //Message all of the network analysis agents that the analysis context has changed
            ((INAContextEdit)m_NAContext).ContextChanged();
        }

        #endregion

        #region Post-Solve



        /// <summary>
        /// Gather the error/warning/informative messages from GPMessages
        /// <summary>
        /// <param name="gpMessages">GPMessages container</param>
        /// <returns>string of all GPMessages</returns>
        public string GetGPMessagesAsString(IGPMessages gpMessages)
        {
            // Gather Error/Warning/Informative Messages
            var messages = new StringBuilder();
            if (gpMessages != null)
            {
                for (int i = 0; i < gpMessages.Count; i++)
                {
                    IGPMessage gpMessage = gpMessages.GetMessage(i);
                    string message = gpMessage.Description;
                    switch (gpMessages.GetMessage(i).Type)
                    {
                        case esriGPMessageType.esriGPMessageTypeError:
                            messages.AppendLine("Error " + gpMessage.ErrorCode + ": " + message);
                            break;
                        case esriGPMessageType.esriGPMessageTypeWarning:
                            messages.AppendLine("Warning: " + message);
                            break;
                        default:
                            messages.AppendLine("Information: " + message);
                            break;
                    }
                }
            }
            return messages.ToString();
        }

        #endregion

        #region Solver Settings

        /// <summary>
        /// Set solver settings
        /// </summary>
        public void SetSolverSettings(string attributeName)
        {
            //Set Route specific Settings
            INASolver naSolver = m_NAContext.Solver;

            INAClosestFacilitySolver cfSolver = naSolver as INAClosestFacilitySolver;
            cfSolver.OutputLines = esriNAOutputLineType.esriNAOutputLineTrueShapeWithMeasure;
            cfSolver.TravelDirection = esriNATravelDirection.esriNATravelDirectionFromFacility;

            // Set generic solver settings
            // Set the impedance attribute
            INASolverSettings naSolverSettings;
            naSolverSettings = naSolver as INASolverSettings;
            naSolverSettings.ImpedanceAttributeName = attributeName;

            // Set the OneWay Restriction if necessary
            IStringArray restrictions;
            restrictions = naSolverSettings.RestrictionAttributeNames;
            restrictions.RemoveAll();
            naSolverSettings.RestrictionAttributeNames = restrictions;

            //Restrict UTurns
            naSolverSettings.RestrictUTurns = esriNetworkForwardStarBacktrack.esriNFSBNoBacktrack;
            naSolverSettings.IgnoreInvalidLocations = true;

            // Set the Hierarchy attribute
            if (naSolverSettings.UseHierarchy)
                naSolverSettings.HierarchyAttributeName = "HierarchyMultiNet";

            // Do not forget to update the context after you set your impedance
            naSolver.UpdateContext(m_NAContext, GetDENetworkDataset(m_NAContext.NetworkDataset), new GPMessagesClass());
        }

        /// <summary>
        /// Check whether a string represents a double value.
        /// </summary>
        /// <param name="str">String to test</param>
        /// <returns>bool</returns>
        private bool IsNumeric(string str)
        {
            try
            {
                double.Parse(str.Trim());
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

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

        internal void SetFacilities(IFeatureClass facilityClass, IQueryFilter facilityFilter)
        {
            this._facilityClass = facilityClass;
            LoadNANetworkLocations("Facilities", facilityClass, facilityFilter, 500);
        }

        internal void SetIncidents(IFeatureClass siteClass, IQueryFilter siteFilter)
        {
            this._siteClass = siteClass;
            LoadNANetworkLocations("Incidents", siteClass, siteFilter, 500);
        }

        internal SupplyRoute FindRoute()
        {
            IFeatureCursor cursor = null;
            try
            {
                IGPMessages gpMessages = new GPMessagesClass();
                SetSolverSettings(this.Attribute);

                if (!m_NAContext.Solver.Solve(m_NAContext, gpMessages, null))
                {

                }

                IFeatureClass routeClass = m_NAContext.NAClasses.get_ItemByName(OUTPUTCLASSNAME) as IFeatureClass;

                cursor = routeClass.Search(null, false);
                IFeature fcRoute = cursor.NextFeature();
                if (fcRoute == null)
                {
                    return null;
                }

                IPolyline geoRoute = fcRoute.ShapeCopy as IPolyline;
                IPoint facilityQueryPoint = geoRoute.FromPoint;

                int id = FindFacilityID(facilityQueryPoint);
                SupplyRoute resultRoute = new SupplyRoute() { RepoID = id, Route = geoRoute };
                return resultRoute;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ComReleaser.ReleaseCOMObject(cursor);
            }
        }

        private int FindFacilityID(IPoint facilityPoint)
        {
            IEnvelope envlope = facilityPoint.Envelope;
            
            double _tolerance = 10;
            envlope.Expand(_tolerance,_tolerance,false);

            ISpatialFilter filter = new SpatialFilter();
            filter.Geometry = envlope;

            //filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
            //filter.SpatialRelDescription = "T******** ";

            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelEnvelopeIntersects;
            IFeatureCursor cursor = null;
            try
            {
                cursor = _facilityClass.Search(filter, true);

                /*
                 * will throw an exception
                IFeature o1 = _facilityClass.GetFeature(0);
                IPoint p1 = o1.ShapeCopy as IPoint;
                IRelationalOperator relOper = p1 as IRelationalOperator;
               // bool b = relOper.Relation(facilityPoint, "T********");
                bool b1 = relOper.Equals(facilityPoint);

                */

                IFeature f = cursor.NextFeature();
                if (f != null)
                {
                    return f.OID;
                }
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("未能在查找到的道路上找到配送点");
                return -1;
            }
            finally
            {
                ComReleaser.ReleaseCOMObject(cursor);
            }
        }

        public string Attribute { get; set; }
    }
}
