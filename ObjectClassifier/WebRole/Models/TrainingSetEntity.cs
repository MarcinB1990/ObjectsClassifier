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
        public string UserName { get; set; }
        public string Name { get; set; }
        public DateTime DateOfEntry { get; set; }
        public int NumberOfClasses { get; set; }
        public int NumberOfAttributes { get; set; }
        public string Comment { get; set; }
        public string TrainingSetReference { get; set; }
        public string TrainingSetFileSource { get; set; }
        public int NumberOfUses { get; set; }

        public TrainingSetEntity()
            : base(string.Empty,string.Empty)
        {
        }

        public TrainingSetEntity(string userId,string trainingSetId,string userName,DateTime dateOfEntry,string name, int numberOfClasses,int numberOfAttributes,string comment,string trainingSetReference,string trainingSetFileSource,int numberOfUses)
            : base(userId,trainingSetId)
        {
            this.UserName = userName;
            this.Name = name;
            this.DateOfEntry = dateOfEntry;
            this.NumberOfClasses = numberOfClasses;
            this.NumberOfAttributes = numberOfAttributes;
            this.Comment = comment;
            this.TrainingSetReference = trainingSetReference;
            this.TrainingSetFileSource = trainingSetFileSource;
            this.NumberOfUses = numberOfUses;
        }
    }
}