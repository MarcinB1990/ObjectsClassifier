using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRole.Controllers;
using WebRole.Models;

namespace WebRole.Views
{
    public partial class AddTraining : System.Web.UI.Page
    {
        TrainingSetController trainingSetController;
        protected void Page_Load(object sender, EventArgs e)
        {
            trainingSetController = new TrainingSetController();
            if (User.Identity.IsAuthenticated)
            {
                loggedOut.Visible = false;
                loggedIn.Visible = true;

            }
        }

        protected void UploadTrainingSet(object sender, EventArgs e)
        {
            if(trainingSetController.Save(new TrainingSet(name.Text,Int32.Parse(numberOfClasses.Text),Int32.Parse(numberOfAttributes.Text),comment.Text,fileUploader.FileContent,fileUploader.FileName))){
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