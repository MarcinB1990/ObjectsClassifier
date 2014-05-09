using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    /// <summary>
    /// Model zbioru wynikowego
    /// </summary>
    public class ResultSetReturn
    {
        /// <summary>
        /// Liczba klas
        /// </summary>
        public int NumberOfClasses { get; set; }
        /// <summary>
        /// Liczba cech
        /// </summary>
        public int NumberOfAttributes { get; set; }
        /// <summary>
        /// Data wprowadzenia
        /// </summary>
        public DateTime DateOfEntry { get; set; }
        /// <summary>
        /// Komentarz
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Adres pliku ze zbiorem uczącym
        /// </summary>
        public string TrainingSetFileSource { get; set; }
        /// <summary>
        /// Adres pliku ze zbiorem wejściowym do klasyfikacji
        /// </summary>
        public string InputFileSource { get; set; }
        /// <summary>
        /// Adres pliku ze zbiorem wynikowym
        /// </summary>
        public string ResultSetFileSource { get; set; }
        /// <summary>
        /// Sposób klasyfikacji (0-5NN,1-5NN Chaudhuriego, 2-5NN Kellera)
        /// </summary>
        public string MethodOfClassification { get; set; }
        /// <summary>
        /// Postęp klasyfikacji
        /// </summary>
        public string Progress { get; set; }
        /// <summary>
        /// Format pliku
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="numberOfClasses">Liczba klas</param>
        /// <param name="numberOfAttributes">Liczba cech</param>
        /// <param name="dateOfEntry">Data wprowadzenia</param>
        /// <param name="comment">Komentarz</param>
        /// <param name="trainingSetFileSource">Adres pliku ze zbiorem uczącym</param>
        /// <param name="inputFileSource">Adres pliku ze zbiorem wejściowym do klasyfikacji</param>
        /// <param name="resultSetFileSource">Adres pliku ze zbiorem wynikowym</param>
        /// <param name="methodOfClassification">Sposób klasyfikacji (0-5NN,1-5NN Chaudhuriego, 2-5NN Kellera)</param>
        /// <param name="progress">Postęp klasyfikacji</param>
        /// <param name="fileExtension">Format pliku</param>
        public ResultSetReturn(int numberOfClasses, int numberOfAttributes, DateTime dateOfEntry, string comment, string trainingSetFileSource, string inputFileSource, string resultSetFileSource, string methodOfClassification, string progress,string fileExtension)
        {
            NumberOfClasses = numberOfClasses;
            NumberOfAttributes = numberOfAttributes;
            DateOfEntry = dateOfEntry;
            Comment = comment;
            TrainingSetFileSource = trainingSetFileSource;
            InputFileSource = inputFileSource;
            ResultSetFileSource = resultSetFileSource;
            MethodOfClassification = methodOfClassification;
            Progress = progress;
            FileExtension = fileExtension;
        }
    }
}