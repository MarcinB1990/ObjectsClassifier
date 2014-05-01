using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class ResultSetReturn
    {
        public int NumberOfClasses { get; set; }
        public int NumberOfAttributes { get; set; }
        public DateTime DateOfEntry { get; set; }
        public string Comment { get; set; }
        public string TrainingSetFileSource { get; set; }
        public string InputFileSource { get; set; }
        public string ResultSetFileSource { get; set; }
        public string Progress { get; set; }


        public ResultSetReturn(int numberOfClasses, int numberOfAttributes, DateTime dateOfEntry, string comment, string trainingSetFileSource, string inputFileSource, string resultSetFileSource, string progress)
        {
            this.NumberOfClasses = numberOfClasses;
            this.NumberOfAttributes = numberOfAttributes;
            this.DateOfEntry = dateOfEntry;
            this.Comment = comment;
            this.TrainingSetFileSource = trainingSetFileSource;
            this.InputFileSource = inputFileSource;
            this.ResultSetFileSource = resultSetFileSource;
            this.Progress = progress;
        }
    }
}