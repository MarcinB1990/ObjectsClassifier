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
        ResultSetsController resultSetsController;
        protected void Page_Load(object sender, EventArgs e)
        {
            resultSetsController=new ResultSetsController();
            if (User.Identity.IsAuthenticated)
            {
                loggedOut.Visible = false;
                loggedIn.Visible = true;
            }
        }
    }
}