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
        TrainingSetsController trainingSetController=new TrainingSetsController();
        ResultSetsController resultSetController = new ResultSetsController();
        List<TrainingSetReturn> myTrainingSets;
        protected void Page_Load(object sender, EventArgs e)
        {
            radioNewOrOldTrainingSet.Visible=User.Identity.IsAuthenticated;
            checkboxToSaveTrainingSet.Visible = User.Identity.IsAuthenticated;
            if (User.Identity.IsAuthenticated)
            {
                myTrainingSets = trainingSetController.GetMyTrainingSets(Context.User.Identity.GetUserId()).ToList();
                if (myTrainingSets.Count > 0)
                {
                    myTrainingSetsView.DataSource = myTrainingSets;
                    myTrainingSetsView.DataBind();
                    noTrainingSets.Visible = false;
                    myTrainingSetsView.Visible = true;
                }
                else
                {
                    myTrainingSetsView.Visible = false;
                    noTrainingSets.Visible = true;
                }
            }
        }

        protected void radioNewOrOldTrainingSet_SelectedIndexChanged(object sender, EventArgs e)    
        {
            if (radioNewOrOldTrainingSet.SelectedIndex == 0)
            {
                noSelectedTraining.Visible = false;
                uploadNewTrainingSet.Visible = true;
                useExistingTrainingSet.Visible = false;
                requiredFieldValidatorFileUploaded.Enabled = true;
                requiredFieldValidatorName.Enabled = true;
                requiredFieldValidatorNumberOfAttributes.Enabled = true;
                requiredFieldValidatorNumberOfClasses.Enabled = true;
                regExpValidatorFileUpload.Enabled = true;
                regExpValidatorName.Enabled = true;
                regExpValidatorNumberOfAttributes.Enabled = true;
                regExpValidatorNumberOfClasses.Enabled = true;
            }
            else
            {
                uploadNewTrainingSet.Visible = false;
                useExistingTrainingSet.Visible = true;
                requiredFieldValidatorFileUploaded.Enabled = false;
                requiredFieldValidatorName.Enabled = false;
                requiredFieldValidatorNumberOfAttributes.Enabled = false;
                requiredFieldValidatorNumberOfClasses.Enabled = false;
                regExpValidatorFileUpload.Enabled = false;
                regExpValidatorName.Enabled = false;
                regExpValidatorNumberOfAttributes.Enabled = false;
                regExpValidatorNumberOfClasses.Enabled = false;
            }
        }

        protected void classifyButton_Click(object sender, EventArgs e)
        {
            if (radioNewOrOldTrainingSet.SelectedIndex == 0)
            {
                if (checkboxToSaveTrainingSet.Checked)
                {
                    string trainingSetId=trainingSetController.SaveNew(new TrainingSet(User.Identity.GetUserId(), User.Identity.GetUserName(), name.Text, Int32.Parse(numberOfClasses.Text), Int32.Parse(numberOfAttributes.Text), comment.Text, fileUploader.FileContent, fileUploader.FileName));
                    if (trainingSetId!=null)
                    {
                        error.Visible = false;
                        firstStep.Visible = false;
                        classification.Visible = true;
                        progress.Text = "0%";
                    }
                    else
                    {
                        error.Visible = true;
                    }
                }
            }
            else
            {
                if (myTrainingSetsView.SelectedIndex == -1)
                {
                    noSelectedTraining.Visible = true;
                }
                else
                {
                    firstStep.Visible = false;
                    classification.Visible = true;
                    progress.Text = "0%";
                    resultSetController.SaveNew(new ResultSet(User.Identity.GetUserId(), User.Identity.GetUserName(), inputFileUpload.FileName, myTrainingSets.ElementAt(myTrainingSetsView.SelectedIndex).NumberOfClasses, myTrainingSets.ElementAt(myTrainingSetsView.SelectedIndex).NumberOfAttributes, commentToClassification.Text, inputFileUpload.FileContent, myTrainingSets.ElementAt(myTrainingSetsView.SelectedIndex).TrainingSetId),trainingSetController);
                }
            }
        }

        protected void myTrainingSetsView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            noSelectedTraining.Visible = false;
        }
    }
}