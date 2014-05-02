using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class TrainingSetReturn
    {
        public string TrainingSetId { get; set; }
        public string Name { get; set; }
        public int NumberOfClasses { get; set; }
        public int NumberOfAttributes { get; set; }
        public DateTime DateOfEntry { get; set; }
        public string Comment { get; set; }
        public int NumberOfUses { get; set; }
        public string TrainingSetFileSource { get; set; }

        public TrainingSetReturn(string trainingSetId,string name, int numberOfClasses, int numberOfAttributes,DateTime dateOfEntry, string comment, int numberOfUses,string trainingSetFileSource)
        {
            TrainingSetId = trainingSetId;
            Name = name;
            NumberOfClasses = numberOfClasses;
            NumberOfAttributes = numberOfAttributes;
            Comment = comment;
            DateOfEntry = dateOfEntry;
            NumberOfUses = numberOfUses;
            TrainingSetFileSource = trainingSetFileSource;
        }
    }
}