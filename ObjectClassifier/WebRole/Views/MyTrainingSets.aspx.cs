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
        TrainingSetsController trainingSetController;
        List<TrainingSetReturn> myTrainingSets;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                loggedOut.Visible = false;
                loggedIn.Visible = true;
                trainingSetController = new TrainingSetsController();
                myTrainingSets = trainingSetController.GetMyTrainingSets(Context.User.Identity.GetUserId()).ToList();
                if (myTrainingSets.Count > 0)
                {
                    myTrainingSetsView.DataSource = myTrainingSets;
                    myTrainingSetsView.DataBind();
                    listNotEmpty.Visible = true;
                    listEmpty.Visible = false;
                }
            }
        }

        protected void myTrainingSetsView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (trainingSetController.DeleteTrainingSet(User.Identity.GetUserId(), myTrainingSets.ElementAt(e.RowIndex).TrainingSetId))
            {
                myTrainingSets.RemoveAt(e.RowIndex);
                if (myTrainingSets.Count == 0)
                {
                    myTrainingSetsView.DataSource = myTrainingSets;
                    myTrainingSetsView.DataBind();
                    listNotEmpty.Visible = false;
                    listEmpty.Visible = true;
                }
            }
        }
    }
}