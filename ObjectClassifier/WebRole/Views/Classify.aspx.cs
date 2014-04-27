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
        MessageController messageController = new MessageController();
        List<TrainingSetReturn> myTrainingSets;
        protected void Page_Load(object sender, EventArgs e)
        {
            radioNewOrOldTrainingSet.Visible=User.Identity.IsAuthenticated;
            checkboxToSaveTrainingSet.Visible = User.Identity.IsAuthenticated;
            validFor.Visible = User.Identity.IsAuthenticated;
            commentRowTraining.Visible = User.Identity.IsAuthenticated;
            commentRowResult.Visible = User.Identity.IsAuthenticated;
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
                error.Visible = false;
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
            string trainingSetId = null;
            string resultSetId = null;
            int numberOfClassesTemp = -1;
            int numberOfAttributesTemp = -1;
            string usedUserIdToTraining = string.Empty;
            string usedUserIdToResult = string.Empty;
            bool removeTrainingAfterClassification = true;
            bool removeResultAfterClassification = true;

            if (radioNewOrOldTrainingSet.SelectedIndex == 0)//uzyskiwanie zbioru uczącego
            {
                numberOfClassesTemp = Int32.Parse(numberOfClasses.Text);
                numberOfAttributesTemp = Int32.Parse(numberOfAttributes.Text);
                if (checkboxToSaveTrainingSet.Checked)
                {
                    removeTrainingAfterClassification = false;
                    usedUserIdToTraining=User.Identity.GetUserId();
                    trainingSetId = trainingSetController.SaveNew(new TrainingSet(usedUserIdToTraining, User.Identity.GetUserName(), name.Text, numberOfClassesTemp, numberOfAttributesTemp, comment.Text, fileUploader.FileContent, fileUploader.FileName,1));
                }
                else
                {
                    usedUserIdToTraining = Guid.NewGuid().ToString();
                    trainingSetId = trainingSetController.SaveNew(new TrainingSet(usedUserIdToTraining, "temporaryUser", name.Text, numberOfClassesTemp, numberOfAttributesTemp, comment.Text, fileUploader.FileContent, fileUploader.FileName, 1));
                }
            }
            else
            {
                removeTrainingAfterClassification = false;
                if (myTrainingSetsView.SelectedIndex != -1)
                {
                    usedUserIdToTraining = User.Identity.GetUserId();
                    trainingSetId = myTrainingSets.ElementAt(myTrainingSetsView.SelectedIndex).TrainingSetId;
                    trainingSetController.IncrementUses(usedUserIdToTraining, trainingSetId);
                    numberOfClassesTemp=myTrainingSets.ElementAt(myTrainingSetsView.SelectedIndex).NumberOfClasses;
                    numberOfAttributesTemp = myTrainingSets.ElementAt(myTrainingSetsView.SelectedIndex).NumberOfAttributes;
                }
            }

            if (trainingSetId == null)//wyswietlenie odpowiedniego komunikatu o bledzie jesli nie udalo sie uzyskac zbioru uczacego
            {
                if (radioNewOrOldTrainingSet.SelectedIndex == 0)
                {
                    error.Visible = true;
                }
                else
                {
                    noSelectedTraining.Visible = true;
                }
            }
            else//proces klasyfikacji
            {
                if (User.Identity.IsAuthenticated)
                {
                    usedUserIdToResult = User.Identity.GetUserId();
                    removeTrainingAfterClassification = false;
                    removeResultAfterClassification = false;
                }
                else
                {
                    usedUserIdToResult = usedUserIdToTraining;
                }
                resultSetId = resultSetController.SaveNew(new ResultSet(usedUserIdToResult, User.Identity.GetUserName(), inputFileUpload.FileName, numberOfClassesTemp, numberOfAttributesTemp, commentToClassification.Text, inputFileUpload.FileContent, trainingSetId, usedUserIdToTraining), trainingSetController);
                Guid operationGuid = Guid.NewGuid();
                messageController.SendInputMessage(new MessageBuilder(), operationGuid, resultSetId, usedUserIdToResult, removeResultAfterClassification, trainingSetId, usedUserIdToTraining, removeTrainingAfterClassification, methodOfClassification.SelectedIndex);
                bool finished = false;
                //int progressOfClassification = 0;
                //while (!finished)
                //{
                    string mess = messageController.ReceiveMessage(operationGuid);
                    string[] receivedessageParts = mess.Split('|');
                    firstStep.Visible = false;
                    if (("1").Equals(receivedessageParts[1]))
                    {
                        //    int newProgress;
                        //    bool result=Int32.TryParse(receivedessageParts[3], out newProgress);
                        //    if (result)
                        //    {
                        //        if (newProgress > progressOfClassification)
                        //        {
                        //            progress.Text = progressOfClassification.ToString();
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        classificationResult.Visible=true;
                        result.NavigateUrl = receivedessageParts[2];
                    //    finished = true;
                    }
                    else {
                        classificationFault.Visible=true;
                    }
                //}
            }
        }

        protected void myTrainingSetsView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            noSelectedTraining.Visible = false;
        }
    }
}