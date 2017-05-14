using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;

namespace DisasterModel
{
    class RepositoryCol
    {
        List<Repository> _repositories = null;
        private IFeatureClass _fc;
        public static string TentField = "帐篷";
        public static string FoodField = "食品";
        public static string WaterField = "饮用水";

        public void Setup(IFeatureClass fc)
        {
            this._fc = fc;
            _repositories = GetRepositories();
        }

        private List<Repository> GetRepositories()
        {
            _repositories = new List<Repository>();
            IFeatureCursor cursor = _fc.Search(null, false);
            IFeature f = cursor.NextFeature();

            int idxFood = _fc.FindField(FoodField);
            int idxTent = _fc.FindField(TentField);
            int idxWater = _fc.FindField(WaterField);


            while (f != null)
            {
                Repository repo = new Repository()
                {
                    ID = f.OID,
                    Food = double.Parse(f.get_Value(idxFood).ToString()),
                    Tents = double.Parse(f.get_Value(idxTent).ToString()),
                    Water = double.Parse(f.get_Value(idxWater).ToString())
                };
                _repositories.Add(repo);

                f = cursor.NextFeature();

            }

            return _repositories;
        }


        internal ESRI.ArcGIS.Geodatabase.IQueryFilter ValidWaterFilter()
        {
            IQueryFilter filter = new QueryFilterClass();
            filter.WhereClause = string.Format("{0} > 0", WaterField);
            return filter;
        }

        public ESRI.ArcGIS.Geodatabase.IFeatureClass FeatureClass
        {
            get
            {
                return _fc;
            }
        }

        internal Repository FindRepoByID(int id)
        {
            foreach (var item in _repositories)
            {
                if (item.ID == id)
                {
                    return item;
                }

            }
            System.Windows.Forms.MessageBox.Show("未找到 " + id.ToString() + " 号物资储备点");
            return null;
        }

        internal void SupplyWater(Repository repo, double p)
        {
            repo.Water -= p;
            UpdateWater(repo.ID, p);
        }

        private void UpdateWater(int oid, double amount)
        {
            IFeature f = _fc.GetFeature(oid);
            int idxWater = _fc.FindField(WaterField);

            double oldValue = double.Parse(f.get_Value(idxWater).ToString());
            f.set_Value(idxWater, oldValue - amount);
            f.Store();
        }
    }
}
