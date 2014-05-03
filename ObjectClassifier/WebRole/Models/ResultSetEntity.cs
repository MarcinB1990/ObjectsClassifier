using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    //PartitionKey is UserId, RowKey is ResultSetId
    public class ResultSetEntity:TableEntity
    {
        public int NumberOfClasses { get; set; }
        public int NumberOfAttributes { get; set; }
        public DateTime DateOfEntry { get; set; }
        public string Comment { get; set; }
        public string TrainingSetFileSource { get; set; }
        public string InputFileSource { get; set; }
        public string ResultSetFileSource { get; set; }
        public string MethodOfClassification { get; set; }
        public string ReferenceToBlob { get; set; }
        public string Progress { get; set; }

        public ResultSetEntity()
            : base(string.Empty,string.Empty)
        {
        }

        public ResultSetEntity(string userId,string resultSetId,int numberOfClasses,int numberOfAttributes,DateTime timeOfEntry,string comment,string trainingSetFileSource,string inputFileSource,string resultSetFileSource,string referenceToBlob,string methodOfClassification,string progress)
            : base(userId,resultSetId)
        {
            this.NumberOfClasses = numberOfClasses;
            this.NumberOfAttributes = numberOfAttributes;
            this.DateOfEntry = timeOfEntry;
            this.Comment = comment;
            this.TrainingSetFileSource = trainingSetFileSource;
            this.InputFileSource = inputFileSource;
            this.ResultSetFileSource = resultSetFileSource;
            this.MethodOfClassification = methodOfClassification;
            this.ReferenceToBlob=referenceToBlob;
            this.Progress = progress;
        }
    }
}