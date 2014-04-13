using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class TrainingSet
    {
        string Name { get; set; }
        int NumberOfClasses { get; set; }
        int NumberOfAttributes { get; set; }
        string Comment { get; set; }
        Stream FileStream { get; set; }
        string NameOfFile { get; set; }

        public TrainingSet(string name, int numberOfClasses, int numberOfAttributes, string comment, Stream fileStream, string nameOfFile)
        {
            Name = name;
            NumberOfClasses = numberOfClasses;
            NumberOfAttributes = numberOfAttributes;
            Comment = comment;
            FileStream = fileStream;
            NameOfFile = nameOfFile;
        }
    }
}