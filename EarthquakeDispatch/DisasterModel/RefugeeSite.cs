using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisasterModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RefugeeSite
    {
        public static int WaterQuota = 2;

        private Earthquake _earthquake;
        private SeasonCoefficient _seasonCoffe;
        private RegionCoefficient _regionCoffe;
        public int Population { get; set; }

        public int PeopleNeedTent { get; set; }

        public void SetupLocation(Earthquake earthquake, SeasonCoefficient coe, RegionCoefficient region)
        {
            this._earthquake = earthquake;
            this._seasonCoffe = coe;
            this._regionCoffe = region;
            _waterInNeed = Population * _earthquake.DaysInShort * WaterQuota *
                _seasonCoffe.GetSiteCoeffecient(Location, _earthquake.GetOccurMonth());
        }

        public IPoint Location { get; set; }

        public void SetMonthCoeff(SeasonCoefficient coe)
        {
            this._seasonCoffe = coe;
        }

        double _waterInNeed = 0;
        public double WaterInNeed
        {
            get
            {
                return _waterInNeed;
            }
            set
            {
                _waterInNeed = value;
            }
        }

        public double Priority
        {
            get
            {
                return this._regionCoffe.GetRegionCoefficient(Location);
            }
        }
    }
}
