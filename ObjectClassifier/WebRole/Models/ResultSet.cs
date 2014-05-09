using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    /// <summary>
    /// Model zbioru wynikowego
    /// </summary>
    public class ResultSet
    {
        /// <summary>
        /// Id użytkownika dokonującego klasyfikacji
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Nazwa użytkownika dokonującego klasyfikacji
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Liczba klas
        /// </summary>
        public int NumberOfClasses { get; set; }
        /// <summary>
        /// Liczba cech
        /// </summary>
        public int NumberOfAttributes { get; set; }
        /// <summary>
        /// Komentarz
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Zawartość pliku ze zbiorem do klasyfikacji
        /// </summary>
        public Stream InputFileStream { get; set; }
        /// <summary>
        /// Nazwa pliku ze zbiorem do klasyfikacji
        /// </summary>
        public string NameOfInputFile { get; set; }
        /// <summary>
        /// Id zbioru uczącego
        /// </summary>
        public string TrainingSetId { get; set; }
        /// <summary>
        /// Sposób klasyfikacji (0-5NN,1-5NN Chaudhuriego, 2-5NN Kellera)
        /// </summary>
        public int MethodOfClassification { get; set; }
        /// <summary>
        /// Id użytkownika, który wprowadził zbiór uczący
        /// </summary>
        public string UsedUserId { get; set; }
        /// <summary>
        /// Format pliku(0-txt,1-csv)
        /// </summary>
        public int FileExtension { get; set; }
        
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="userId">Id użytkownika dokonującego klasyfikacji</param>
        /// <param name="userName">Nazwa użytkownika dokonującego klasyfikacji</param>
        /// <param name="nameOfInputFile">Nazwa pliku ze zbiorem do klasyfikacji</param>
        /// <param name="numberOfClasses">Liczba klas</param>
        /// <param name="numberOfAttributes">Liczba cech</param>
        /// <param name="comment">Komentarz</param>
        /// <param name="inputFileStream">Zawartość pliku ze zbiorem do klasyfikacji</param>
        /// <param name="trainingSetId">Id zbioru uczącego</param>
        /// <param name="methodOfClassification">Sposób klasyfikacji (0-5NN,1-5NN Chaudhuriego, 2-5NN Kellera)</param>
        /// <param name="usedUserId">Id użytkownika, który wprowadził zbiór uczący</param>
        /// <param name="fileExtension">Format pliku(0-txt,1-csv)</param>
        public ResultSet(string userId,string userName,string nameOfInputFile, int numberOfClasses, int numberOfAttributes, string comment, Stream inputFileStream, string trainingSetId,int methodOfClassification, string usedUserId,int fileExtension)
        {
            UserId = userId;
            UserName = userName;
            NameOfInputFile = nameOfInputFile;
            NumberOfClasses = numberOfClasses;
            NumberOfAttributes = numberOfAttributes;
            Comment = comment;
            InputFileStream = inputFileStream;
            TrainingSetId = trainingSetId;
            MethodOfClassification = methodOfClassification;
            UsedUserId = usedUserId;
            FileExtension = fileExtension;
        }
    }
}