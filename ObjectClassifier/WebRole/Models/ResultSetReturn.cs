using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class ResultSetReturn
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public string DateOfEntry { get; set; }
        public string TrainingSetFileSource { get; set; }
        public string InputFileSource { get; set; }
        public string ResultSetFileSource { get; set; }
        public string Progress { get; set; }


        public ResultSetReturn(string name, string comment, DateTime dateOfEntry, string trainingSetFileSource, string inputFileSource, string resultSetFileSource, string progress)
        {
            this.Name = name;
            this.Comment = comment;
            this.DateOfEntry = dateOfEntry.GetDateTimeFormats('g').ElementAt(0);
            this.TrainingSetFileSource = trainingSetFileSource;
            this.InputFileSource = inputFileSource;
            this.ResultSetFileSource = resultSetFileSource;
            this.Progress = progress;
        }
    }
}