using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRole.Controllers;
using WebRole.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace WebRole.Views
{
    public partial class History : System.Web.UI.Page
    {
        ResultSetsController resultSetsController=new ResultSetsController();
        List<ResultSetReturn> myResultSets;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                loggedOut.Visible = false;
                loggedIn.Visible = true;
                myResultSets = resultSetsController.GetMyResultSets(Context.User.Identity.GetUserId()).ToList();
                if (myResultSets.Count > 0)
                {
                    myResultSetsView.DataSource = myResultSets;
                    myResultSetsView.DataBind();
                    listNotEmpty.Visible = true;
                    listEmpty.Visible = false;
                }
            }
        }
    }
}