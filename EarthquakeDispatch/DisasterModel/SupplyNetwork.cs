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

        private void SupplyTents(RefugeeSite site)
        {
            
        }

        private void SupplyFood(RefugeeSite site)
        {
           
        }

        private void SupplyWater(RefugeeSite site)
        {
            List<SupplyRoute> siteWaterRoutes = new List<SupplyRoute>();
            do
            {
                IFeatureClass facilityClass = _repositoryCol.FeatureClass;
                IQueryFilter facilityFilter = _repositoryCol.ValidWaterFilter();

                if (facilityClass.FeatureCount(facilityFilter) == 0)
                {
                    LogHelper.Error("没有足够的水资源");
                    break;
                }
                _roadNetwork.SetFacilities(facilityClass, facilityFilter);

                IQueryFilter siteFilter = _refugeSiteCol.SiteFilter(site);
                IFeatureClass siteClass = _refugeSiteCol.FeatureClass;
                _roadNetwork.SetIncidents(siteClass, siteFilter);

                SupplyRoute route = _roadNetwork.FindRoute();

                Repository repo = _repositoryCol.FindRepoByID(route.RepoID);
                double waterAmount = 0;
                if (repo.Water >= site.WaterInNeed)
                {
                    waterAmount = site.WaterInNeed;
                }
                else
                {
                    waterAmount = repo.Water;
                }

                _repositoryCol.SupplyWater(repo, waterAmount);
                _refugeSiteCol.ReplenishWater(site, waterAmount);

                route.SetMessagePara("饮用水", waterAmount, "L");
                InsertFeature(route);
                siteWaterRoutes.Add(route);
            }
            while (site.WaterInNeed > 0);
        }

        private void InsertFeature(SupplyRoute route)
        {
            int idxResource = this._outputFC.FindField(SupplyRoute.ResourceField);
            int idxAmount = this._outputFC.FindField(SupplyRoute.AmountField);
            int idxUnit = this._outputFC.FindField(SupplyRoute.UnitField);
            int idxRepoID = this._outputFC.FindField(SupplyRoute.RepoIDField);

            IFeature f = _outputFC.CreateFeature();
            f.set_Value(idxAmount,route.Amount);
            f.set_Value(idxResource, route.Resource);
            f.set_Value(idxUnit, route.Unit);
            f.set_Value(idxRepoID, route.RepoID);
            f.Shape = route.Route;

            f.Store();
        }

        internal void SupplyRefugeeSite(RefugeeSite site)
        {
            SupplyWater(site);
            SupplyFood(site);
            SupplyTents(site);
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
