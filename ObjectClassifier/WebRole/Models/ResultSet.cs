using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class ResultSet
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int NumberOfClasses { get; set; }
        public int NumberOfAttributes { get; set; }
        public string Comment { get; set; }
        public Stream InputFileStream { get; set; }
        public string NameOfInputFile { get; set; }
        public string TrainingSetId { get; set; }
        
        public ResultSet(string userId,string userName,string nameOfInputFile, int numberOfClasses, int numberOfAttributes, string comment, Stream inputFileStream, string nameOfFile,string trainingSetId)
        {
            UserId = userId;
            UserName = userName;
            NameOfInputFile = nameOfInputFile;
            NumberOfClasses = numberOfClasses;
            NumberOfAttributes = numberOfAttributes;
            Comment = comment;
            InputFileStream = inputFileStream;
            TrainingSetId = trainingSetId;
        }
    }
}