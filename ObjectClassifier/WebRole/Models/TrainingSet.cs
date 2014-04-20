using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class TrainingSet
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public int NumberOfClasses { get; set; }
        public int NumberOfAttributes { get; set; }
        public string Comment { get; set; }
        public Stream FileStream { get; set; }
        public string NameOfFile { get; set; }
        public int NumberOfUses { get; set; }

        public TrainingSet(string userId,string userName,string name, int numberOfClasses, int numberOfAttributes, string comment, Stream fileStream, string nameOfFile,int numberOfUses)
        {
            UserId = userId;
            UserName = userName;
            Name = name;
            NumberOfClasses = numberOfClasses;
            NumberOfAttributes = numberOfAttributes;
            Comment = comment;
            FileStream = fileStream;
            NameOfFile = nameOfFile;
            NumberOfUses = numberOfUses;
        }
    }
}