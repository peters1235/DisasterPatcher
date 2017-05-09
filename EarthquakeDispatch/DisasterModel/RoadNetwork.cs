using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;

namespace DisasterModel
{
    public class RoadNetwork
    {
        INetworkDataset network = null;
        private List<Repository> _repositories;

        internal void SetupRepositories(List<Repository> repositories)
        {
            this._repositories = repositories;
        }

        internal void SupplyRefugeeSite(RefugeeSite site)
        {
            SetupMarks();
            while (site.WaterInNeed > 0)
            {
                Repository repo = FindClosestRepository();
                if (repo.Water >= site.WaterInNeed)
                {
                    repo.Water -= site.WaterInNeed;
                    site.WaterInNeed = 0;
                }
                else
                {
                    site.WaterInNeed -= repo.Water;
                    repo.Water = 0;
                }
            }
        }

        private Repository FindClosestRepository()
        {
            throw new NotImplementedException();
        }

        private void SetupMarks()
        {
            throw new NotImplementedException();
        }
    }
}
