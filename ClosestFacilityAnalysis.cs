
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.NetworkAnalyst;
using System;

namespace Geoway.GNC.Statistics.MathCore
{
    public class ClosestFacilityAnalysis
    {
        private string _facilityNameField = "NAME";
        private string _impedanceAttributeName = "Shape_Length";
        public string _incidentNameField = "NAME";
        private int _targetFacilityCount = 1;
        private string facilitiesWhere = null;
        private IFeatureClass incidentFC = null;
        private string incidentWhere = null;
        private bool isInit = false;
        private IFeatureClass m_facilitiesFC = null;
        private double m_maxSnapDis = 1000.0;
        private INAContext m_NAContext;
        private string m_NDName = null;
        private INetworkDataset m_networkDataset = null;
        private string m_networkFeatureDataset = null;
        private IWorkspace m_workspace = null;
        private readonly string OUTPUTCLASSNAME = "CFRoutes";


        public IFeatureClass FacilitiesFC
        {
            get
            {
                return this.m_facilitiesFC;
            }
        }

        public string FacilityNameField
        {
            get
            {
                return this._facilityNameField;
            }
            set
            {
                this._facilityNameField = value;
            }
        }

        public string ImpedanceAttributeName
        {
            get
            {
                return this._impedanceAttributeName;
            }
            set
            {
                this._impedanceAttributeName = value;
            }
        }

        public string IncidentNameField
        {
            get
            {
                return this._incidentNameField;
            }
            set
            {
                this._incidentNameField = value;
            }
        }

        public IFeatureClass IncidentsFC
        {
            get
            {
                return this.incidentFC;
            }
        }

        public double MaxSnapDistance
        {
            get
            {
                return this.m_maxSnapDis;
            }
            set
            {
                this.m_maxSnapDis = value;
            }
        }

        public INAContext NAContext
        {
            get
            {
                return this.m_NAContext;
            }
        }

        public string NetworkDatasetName
        {
            get
            {
                return this.m_NDName;
            }
            set
            {
                this.m_NDName = value;
            }
        }

        public string NetworkFeatureDataset
        {
            get
            {
                return this.m_networkFeatureDataset;
            }
            set
            {
                this.m_networkFeatureDataset = value;
            }
        }

        public int TargetFacilityCount
        {
            get
            {
                return this._targetFacilityCount;
            }
            set
            {
                this._targetFacilityCount = value;
            }
        }

        public IWorkspace Workspace
        {
            get
            {
                return this.m_workspace;
            }
            set
            {
                this.m_workspace = value;
            }
        }

        public void CreateSolverContext(INetworkDataset networkDataset)
        {
            if (networkDataset != null)
            {
                IDENetworkDataset deNDS = this.GetDENetworkDataset(networkDataset);
                INASolver naSolver = new NAClosestFacilitySolverClass();
                this.m_NAContext = naSolver.CreateContext(deNDS, naSolver.Name);
                ((INAContextEdit)this.m_NAContext).Bind(networkDataset, new GPMessagesClass());
            }
        }

        public IDENetworkDataset GetDENetworkDataset(INetworkDataset networkDataset)
        {
            IDatasetComponent dsComponent = networkDataset as IDatasetComponent;
            return (dsComponent.DataElement as IDENetworkDataset);
        }

        public void Initialize()
        {
            if ((this.m_workspace == null) || (this.m_NDName == null))
            {
                throw new ArgumentException("请设置工作空间和网络数据集属性!");
            }
            IFeatureWorkspace featureWorkspace = this.m_workspace as IFeatureWorkspace;
            this.m_networkDataset = this.OpenNetworkDataset(this.m_workspace, this.m_networkFeatureDataset, this.m_NDName);
            this.CreateSolverContext(this.m_networkDataset);
        }

        public INAClassLoader LoadNANetworkLocations(string strNAClassName, IFeatureClass inputFC, double maxSnapTolerance, string filedName, string mapFiledName, string where)
        {
            INAClass naClass = this.m_NAContext.NAClasses.get_ItemByName(strNAClassName) as INAClass;
            naClass.DeleteAllRows();
            INAClassLoader classLoader = new NAClassLoaderClass
            {
                Locator = this.m_NAContext.Locator
            };
            INALocator2 nloc3 = (INALocator2)classLoader.Locator;
            if (maxSnapTolerance > 0.0)
            {
                nloc3.MaxSnapTolerance = maxSnapTolerance;
            }
            nloc3.SnapToleranceUnits = esriUnits.esriMeters;
            classLoader.NAClass = naClass;
            INAClassFieldMap fieldMap = new NAClassFieldMapClass();
            fieldMap.CreateMapping(naClass.ClassDefinition, inputFC.Fields);
            fieldMap.set_MappedField(filedName, mapFiledName);
            classLoader.FieldMap = fieldMap;
            INALocator2 locator = this.m_NAContext.Locator as INALocator2;
            int rowsIn = 0;
            int rowsLocated = 0;
            IQueryFilter filter = null;
            if (where != null)
            {
                filter = new QueryFilterClass
                {
                    WhereClause = where
                };
            }
            IFeatureCursor featureCursor = inputFC.Search(filter, true);
            classLoader.Load((ICursor)featureCursor, null, ref rowsIn, ref rowsLocated);
            ((INAContextEdit)this.m_NAContext).ContextChanged();
            return classLoader;
        }

        public INetworkDataset OpenNetworkDataset(IWorkspace workspace, string featureDatasetName, string strNDSName)
        {
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            IFeatureDatasetExtensionContainer featureDatasetExtensionContainer = featureWorkspace.OpenFeatureDataset(featureDatasetName) as IFeatureDatasetExtensionContainer;
            IDatasetContainer3 datasetContainer3 = featureDatasetExtensionContainer.FindExtension(esriDatasetType.esriDTNetworkDataset) as IDatasetContainer3;
            return (datasetContainer3.get_DatasetByName(esriDatasetType.esriDTNetworkDataset, strNDSName) as INetworkDataset);
        }

        public void SetFacilitiesFC(IFeatureClass facFc, string where)
        {
            this.m_facilitiesFC = facFc;
            this.facilitiesWhere = where;
        }

        public void SetIncidentsFC(IFeatureClass inFc, string where)
        {
            this.incidentFC = inFc;
            this.incidentWhere = where;
        }

        public void SetSolverSettings()
        {
            INASolver naSolver = this.m_NAContext.Solver;
            INAClosestFacilitySolver cfSolver = naSolver as INAClosestFacilitySolver;
            cfSolver.DefaultCutoff = null;
            cfSolver.DefaultTargetFacilityCount = this._targetFacilityCount;
            cfSolver.OutputLines = esriNAOutputLineType.esriNAOutputLineTrueShapeWithMeasure;
            cfSolver.TravelDirection = esriNATravelDirection.esriNATravelDirectionToFacility;
            INASolverSettings naSolverSettings = naSolver as INASolverSettings;
            naSolverSettings.ImpedanceAttributeName = this._impedanceAttributeName;
            IStringArray restrictions = naSolverSettings.RestrictionAttributeNames;
            naSolverSettings.RestrictionAttributeNames = restrictions;
            naSolverSettings.RestrictUTurns = esriNetworkForwardStarBacktrack.esriNFSBNoBacktrack;
            naSolverSettings.IgnoreInvalidLocations = true;
            naSolver.UpdateContext(this.m_NAContext, this.GetDENetworkDataset(this.m_NAContext.NetworkDataset), new GPMessagesClass());
        }

        public ITable Solve(ref IGPMessages gpMessages)
        {
            if (!this.isInit)
            {
                this.Initialize();
                this.isInit = true;
            }
            this.LoadNANetworkLocations("Incidents", this.incidentFC, this.m_maxSnapDis, "NAME", this._incidentNameField, this.incidentWhere);
            this.LoadNANetworkLocations("Facilities", this.m_facilitiesFC, this.m_maxSnapDis, "NAME", this._facilityNameField, this.facilitiesWhere);
            try
            {
                this.SetSolverSettings();
                this.m_NAContext.Solver.Solve(this.m_NAContext, gpMessages, null);
                return (this.m_NAContext.NAClasses.get_ItemByName(this.OUTPUTCLASSNAME) as ITable);
            }
            catch (Exception ee)
            {
                Geoway.ADF.MIS.Utility.Log.LogHelper.Error.Append(ee);
            }
            return null;
        }
    }
}

