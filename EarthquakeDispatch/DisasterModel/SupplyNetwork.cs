using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;

namespace DisasterModel
{
    public class SupplyNetwork
    {
        INetworkDataset network = null;
        private List<Repository> _repositories;
        private RefugeeSiteCol _refugeSiteCol;
        private RepositoryCol _repositoryCol;
        private RoadNetwork _roadNetwork;
        private IFeatureClass _outputFC;

        List<SupplyRoute> _siteRoutes = new List<SupplyRoute>();

        public List<SupplyRoute> Routes
        {
            get { return _siteRoutes; }
        }

        public void SupplyResource(RefugeeSite site)
        {
            do
            {
                IFeatureClass facilityClass = _repositoryCol.FeatureClass;
                IQueryFilter facilityFilter = _repositoryCol.ValidFilter();

                if (facilityClass.FeatureCount(facilityFilter) == 0)
                {
                    LogHelper.Error("没有足够的" + site.ResourceName());
                    break;
                }
                _roadNetwork.SetFacilities(facilityClass, facilityFilter);

                IQueryFilter siteFilter = _refugeSiteCol.SiteFilter(site);
                IFeatureClass siteClass = _refugeSiteCol.FeatureClass;
                _roadNetwork.SetIncidents(siteClass, siteFilter);

                SupplyRoute route = _roadNetwork.FindRoute();

                Repository repo = _repositoryCol.FindRepoByID(route.RepoID);
                int amount = 0;
                if (repo.Remain >= site.ResourceInNeed)
                {
                    amount = site.ResourceInNeed;
                }
                else
                {
                    amount = repo.Remain;
                }

                _repositoryCol.SupplyResource(repo, amount);
                _refugeSiteCol.ReplenishResource(site, amount);

                route.SetMessagePara(site.ResourceName(), amount, site.ResourceUnit());
                route.IncidentID = site.OID ;

                AddRouteFeature(route);
                _siteRoutes.Add(route);
            }
            while (site.ResourceInNeed > 0);
        }

        private void AddRouteFeature(SupplyRoute route)
        {
            int idxResource = this._outputFC.FindField(SupplyRoute.ResourceField);
            int idxAmount = this._outputFC.FindField(SupplyRoute.AmountField);
            int idxUnit = this._outputFC.FindField(SupplyRoute.UnitField);
            int idxRepoID = this._outputFC.FindField(SupplyRoute.RepoIDField);
            int idxIncidentID = this._outputFC.FindField(SupplyRoute.IncidentIDField);

            IFeature f = _outputFC.CreateFeature();
            f.set_Value(idxAmount,route.Amount);
            f.set_Value(idxResource, route.Resource);
            f.set_Value(idxUnit, route.Unit);
            f.set_Value(idxRepoID, route.RepoID);
            f.set_Value(idxIncidentID, route.IncidentID);

            f.Shape = route.Route;

            f.Store();
        }

       

        internal void SetLocations(RefugeeSiteCol _refugeSiteCol, RepositoryCol _repositoryCol, RoadNetwork _roadNetwork)
        {
            this._refugeSiteCol = _refugeSiteCol;
            this._repositoryCol = _repositoryCol;
            this._roadNetwork = _roadNetwork;
        }

        internal void Init(IFeatureClass outputFC)
        {
            this._outputFC = outputFC;
        }
    }
}
