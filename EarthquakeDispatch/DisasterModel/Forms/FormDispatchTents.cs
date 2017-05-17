using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisasterModel;
using DisasterModel.Forms;
using DisasterModel.SitesCol;

namespace DisasterModel
{
   public class FormDispatchTents:FormDispatch
    {

        protected override UCParas GetUC()
        {
            return new UCTents();
        }
        protected override RepositoryCol GetRepoCol()
        {
            RepositoryCol repoCol = new RepositoryCol();
            repoCol.Setup(_dispatcher.RepoFeatureClass, RepositoryCol.TentField);
            return repoCol;
        }

        protected override RefugeeSiteCol GetSiteCol()
        {
            RefugeeSiteTentCol siteCol = new RefugeeSiteTentCol();
            UCTents ucw = _ucParas as UCTents;
           siteCol.Setup(_dispatcher, ucw.ShareTent);
            return siteCol;
        }
    }
}
