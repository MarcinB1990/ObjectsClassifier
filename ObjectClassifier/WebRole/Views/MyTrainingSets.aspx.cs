using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRole.Controllers;
using WebRole.Models;
using Microsoft.AspNet.Identity;

namespace WebRole.Views
{
    public partial class MyTrainingSets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TrainingSetController trainingSetController = new TrainingSetController();
            IEnumerable<TrainingSetReturn> myTrainingSets = trainingSetController.GetMyTrainingSets(Context.User.Identity.GetUserId());
            myTrainingSetsView.DataSource = myTrainingSets;
            myTrainingSetsView.DataBind();
        }
    }
}