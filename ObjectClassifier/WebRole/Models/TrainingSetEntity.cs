﻿using Microsoft.WindowsAzure.Storage.Table;
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
        public int NumberOfClasses { get; set; }
        public int NumberOfAttributes { get; set; }
        public DateTime DateOfEntry { get; set; }
        public string Comment { get; set; }
        public string ReferenceToBlob { get; set; }
        public string TrainingSetFileSource { get; set; }
        public int NumberOfUses { get; set; }

        public TrainingSetEntity()
            : base(string.Empty,string.Empty)
        {
        }

        public TrainingSetEntity(string userId, string trainingSetId, string userName, string name, int numberOfClasses, int numberOfAttributes, DateTime dateOfEntry, string comment, string referenceToBlob, string trainingSetFileSource, int numberOfUses)
            : base(userId,trainingSetId)
        {
            this.UserName = userName;
            this.Name = name;
            this.NumberOfClasses = numberOfClasses;
            this.NumberOfAttributes = numberOfAttributes;
            this.DateOfEntry = dateOfEntry;
            this.Comment = comment;
            this.ReferenceToBlob = referenceToBlob;
            this.TrainingSetFileSource = trainingSetFileSource;
            this.NumberOfUses = numberOfUses;
        }
    }
}