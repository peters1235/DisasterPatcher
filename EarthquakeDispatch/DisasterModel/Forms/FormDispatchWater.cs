using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisasterModel.SitesCol;


namespace DisasterModel
{
    public class FormDispatchWater: FormDispatch
    {
        protected override UCParas GetUC()
        {
            return new UCWater();
        }
        protected override RepositoryCol GetRepoCol()
        {
            RepositoryCol repoCol = new RepositoryCol();
            repoCol.Setup(_dispatcher.RepoFeatureClass, RepositoryCol.WaterField);
            return repoCol;
        }

        protected override RefugeeSiteCol GetSiteCol()
        {
            RefugeeSiteWaterCol siteCol = new RefugeeSiteWaterCol();
            UCWater ucw = _ucParas as UCWater;
            siteCol.Setup(_dispatcher,  ucw.DaysInShort, ucw.Quota);
            return siteCol;
        }
    }
}
