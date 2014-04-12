using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    //PartitionKey is UserId, RowKey is ResultSetId
    public class ResultSetEntity:TableServiceEntity
    {
        string Name { get; set; }
        DateTime DateOfEntry { get; set; }
        string Comment { get; set; }
        string TrainingSetFileSource { get; set; }
        string InputFileSource { get; set; }
        string ResultSetFileSource { get; set; }
        string Progress { get; set; }

        public ResultSetEntity()
            : base(string.Empty,Guid.NewGuid().ToString())
        {
        }

        public ResultSetEntity(string userId,string name,string comment,string trainingSetFileSource,string inputFileSource,string resultSetFileSource)
            : base(userId,Guid.NewGuid().ToString())
        {
            this.Name = name;
            this.DateOfEntry = DateTime.Now;
            this.Comment = comment;
            this.TrainingSetFileSource = trainingSetFileSource;
            this.InputFileSource = inputFileSource;
            this.ResultSetFileSource = resultSetFileSource;
            this.Progress = "To do";
        }
    }
}