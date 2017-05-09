using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisasterModel
{
    public class Dispatcher
    {
        Earthquake _quake = null;
        RegionCoefficient _region = null;
        SeasonCoefficient _season = null;
        List<RefugeeSite> _refugeeSites = null;
        List<Repository> _repositories = null;
        RoadNetwork _roadNetwork = null;

        public void Setup()
        {
            _quake = GetEarthquake();
            _region = GetRegionCoefficient();
            _season = GetSeasonCoefficient();

            _refugeeSites = GetRefugeeSites();

            _roadNetwork = SetupRoadNetwork();

            _repositories = GetRepositories();

            SortRefugeeSites(_refugeeSites);
        }

        private RoadNetwork SetupRoadNetwork()
        {
            throw new NotImplementedException();
        }

        private List<Repository> GetRepositories()
        {
            throw new NotImplementedException();
        }

        public void Dispatch()
        {
            foreach (var site in _refugeeSites)
            {
                DispatchOneSite(site);
            }
        }

        private void DispatchOneSite(RefugeeSite site)
        {
            _roadNetwork.SetupRepositories(_repositories);
            _roadNetwork.SupplyRefugeeSite(site);
        }

        private void SortRefugeeSites(List<RefugeeSite> _refugeeSites)
        {
            throw new NotImplementedException();
        }

        private List<RefugeeSite> GetRefugeeSites()
        {
            List<RefugeeSite> result = new List<RefugeeSite>();

            foreach (var item in result)
            {
                item.SetupLocation(_quake, _season, _region);
            }

            return result;
        }

        private SeasonCoefficient GetSeasonCoefficient()
        {
            throw new NotImplementedException();
        }

        private RegionCoefficient GetRegionCoefficient()
        {
            throw new NotImplementedException();
        }

        private Earthquake GetEarthquake()
        {
            throw new NotImplementedException();
        }

    }
}
