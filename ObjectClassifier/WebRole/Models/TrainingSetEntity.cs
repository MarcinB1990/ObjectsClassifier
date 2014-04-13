using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    //PartitionKey is UserId, RowKey is TrainingSetId
    public class TrainingSetEntity:TableEntity
    {
        string UserName { get; set; }
        string Name { get; set; }
        DateTime DateOfEntry { get; set; }
        int NumberOfClasses{get;set;}
        int NumberOfAttributes{get; set;}
        string Comment { get; set; }
        string TrainingSetFileSource { get; set; }
        int NumberOfUses{get;set;}

        public TrainingSetEntity()
            : base(string.Empty,string.Empty)
        {
        }

        public TrainingSetEntity(string userId,string trainingSetId,string userName,DateTime dateOfEntry,string name, int numberOfClasses,int numberOfAttributes,string comment,string trainingSetFileSource,int numberOfUses)
            : base(userId,trainingSetId)
        {
            this.UserName = userName;
            this.Name = name;
            this.DateOfEntry = dateOfEntry;
            this.NumberOfClasses = numberOfClasses;
            this.NumberOfAttributes = numberOfAttributes;
            this.Comment = comment;
            this.TrainingSetFileSource = trainingSetFileSource;
            this.NumberOfUses = numberOfUses;
        }
    }
}