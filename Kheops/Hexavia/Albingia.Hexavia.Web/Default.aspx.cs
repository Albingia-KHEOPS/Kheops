using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Albingia.Hexavia.Services;
using Albingia.Hexavia.DataAccess;
using Albingia.Hexavia.CoreDomain;
using System.Runtime.Caching;

namespace Albingia.Hexavia.Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Response.Redirect("~/GRP/TestGrp.html");
            }
        }
    }
}
