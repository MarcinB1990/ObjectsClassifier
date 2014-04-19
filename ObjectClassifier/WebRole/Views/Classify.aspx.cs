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
    public partial class Classify : System.Web.UI.Page
    {
        TrainingSetsController trainingSetController;
        List<TrainingSetReturn> myTrainingSets;
        protected void Page_Load(object sender, EventArgs e)
        {
            trainingSetController = new TrainingSetsController();
            if (User.Identity.IsAuthenticated)
            {
                radioNewOrOldTrainingSet.Visible = true;
                myTrainingSets = trainingSetController.GetMyTrainingSets(Context.User.Identity.GetUserId()).ToList();
                if (myTrainingSets.Count > 0)
                {
                    myTrainingSetsView.DataSource = myTrainingSets;
                    myTrainingSetsView.DataBind();
                   
                }
            }
        }
    }
}