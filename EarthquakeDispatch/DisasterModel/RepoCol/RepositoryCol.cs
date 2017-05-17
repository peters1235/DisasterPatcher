using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.ADF;

namespace DisasterModel
{
    public class RepositoryCol
    {
        List<Repository> _repositories = null;

        List<int> _reposWithNoResource = new List<int>();


        private IFeatureClass _fc;
        public static string TentField = "帐篷";
        public static string FoodField = "食品";
        public static string WaterField = "饮用水";
       
        private string _resourceField;

        internal void Setup(IFeatureClass fc, string resourceField)
        {
            this._fc = fc;
            this._resourceField = resourceField;

            _repositories = GetRepositories();
        }

        public void Setup(IFeatureClass fc)
        {
            this._fc = fc;
            _repositories = GetRepositories();
        }

        private List<Repository> GetRepositories()
        {
            _repositories = new List<Repository>();
            IFeatureCursor cursor = null;
            try
            {


                cursor = _fc.Search(null, false);
                IFeature f = cursor.NextFeature();

                int _idxResource = _fc.FindField(_resourceField);


                while (f != null)
                {
                    Repository repo = new Repository()
                    {
                        ID = f.OID,
                        Remain = int.Parse(f.get_Value(_idxResource).ToString())
                    };
                    _repositories.Add(repo);

                    f = cursor.NextFeature();

                }

                return _repositories;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
            finally
            {
                ComReleaser.ReleaseCOMObject(cursor);
            }
        }


        internal ESRI.ArcGIS.Geodatabase.IQueryFilter ValidFilter()
        {
            IQueryFilter filter = new QueryFilterClass();
            //filter.WhereClause = string.Format("{0} > 0", WaterField);
            filter.WhereClause = ExcludeIDs(_reposWithNoResource);
            return filter;
        }

        private string ExcludeIDs(List<int> ids)
        {
            if (ids.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder(_fc.OIDFieldName + " not in (");
            foreach (var id in ids)
            {
                sb.Append(id.ToString() + ",");
            }
            return sb.ToString().TrimEnd(',') + ")";
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

        internal void SupplyResource(Repository repo, int p)
        {
            repo.Remain -= p;

            if (repo.Remain <= 0)
            {
                _reposWithNoResource.Add(repo.ID);
            }
            //UpdateWater(repo.ID, p);
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
