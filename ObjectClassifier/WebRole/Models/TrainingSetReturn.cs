using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class TrainingSetReturn
    {
        public string Name { get; set; }
        public int NumberOfClasses { get; set; }
        public int NumberOfAttributes { get; set; }
        public string Comment { get; set; }
        public string DateOfEntry { get; set; }
        public int NumberOfUses { get; set; }
        public string TrainingSetFileSource { get; set; }

        public TrainingSetReturn(string name, int numberOfClasses, int numberOfAttributes, string comment, DateTime dateOfEntry, int numberOfUses,string trainingSetFileSource)
        {
            Name = name;
            NumberOfClasses = numberOfClasses;
            NumberOfAttributes = numberOfAttributes;
            Comment = comment;
            DateOfEntry = dateOfEntry.GetDateTimeFormats('g').ElementAt(0);
            NumberOfUses = numberOfUses;
            TrainingSetFileSource = trainingSetFileSource;
        }
    }
}