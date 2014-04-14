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
    public partial class AddTraining : System.Web.UI.Page
    {
        TrainingSetsController trainingSetController;
        protected void Page_Load(object sender, EventArgs e)
        {
            trainingSetController = new TrainingSetsController();
            if (User.Identity.IsAuthenticated)
            {
                loggedOut.Visible = false;
                loggedIn.Visible = true;

            }
        }

        protected void UploadTrainingSet(object sender, EventArgs e)
        {
            if(trainingSetController.SaveNew(new TrainingSet(User.Identity.GetUserId(),User.Identity.GetUserName(),name.Text,Int32.Parse(numberOfClasses.Text),Int32.Parse(numberOfAttributes.Text),comment.Text,fileUploader.FileContent,fileUploader.FileName))){
                loggedIn.Visible=false;
                uploaded.Visible=true;
                error.Visible = false;
            }
            else{
                error.Visible=true;
            }
        }
    }
}