using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    //PartitionKey is UserId, RowKey is TrainingSetId
    public class TrainingSetEntity:TableServiceEntity
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
            : base(string.Empty,Guid.NewGuid().ToString())
        {
        }

        public TrainingSetEntity(string userId,string name, string userName, int numberOfClasses,int numberOfAttributes,string comment,string trainingSetFileSource)
            : base(userId,Guid.NewGuid().ToString())
        {
            this.UserName = userName;
            this.Name = name;
            this.DateOfEntry = DateTime.Now;
            this.NumberOfClasses = numberOfClasses;
            this.NumberOfAttributes = numberOfAttributes;
            this.Comment = comment;
            this.TrainingSetFileSource = trainingSetFileSource;
            this.NumberOfUses = 0;
        }
    }
}