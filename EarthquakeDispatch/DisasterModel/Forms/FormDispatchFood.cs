using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisasterModel;
using DisasterModel.Forms;

namespace DisasterModel
{
   public class FormDispatchFood:FormDispatch
    {

        protected override UCParas GetUC()
        {
            return new UCFood();
        }
        protected override RepositoryCol GetRepoCol()
        {
            RepositoryCol repoCol = new RepositoryCol();
            repoCol.Setup(_dispatcher.RepoFeatureClass, RepositoryCol.FoodField);
            return repoCol;
        }

        protected override RefugeeSiteCol GetSiteCol()
        {
            RefugeeSiteFoodCol siteCol = new RefugeeSiteFoodCol();
            UCFood ucw = _ucParas as UCFood;
           siteCol.Setup(_dispatcher, ucw.DaysInShort, ucw.Quota);
            return siteCol;
        }
    }
}
