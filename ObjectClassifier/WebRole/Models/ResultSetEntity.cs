using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    /// <summary>
    /// Encja zbioru wynikowego. UserId stanowi PartitionKey, a ResultSetId - RowKey
    /// </summary>
    public class ResultSetEntity:TableEntity
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
        /// Sposób klasyfikacji (0-k-NN,1-k-NN Chaudhuriego, 2-k-NN Kellera)
        /// </summary>
        public string MethodOfClassification { get; set; }
        /// <summary>
        /// Referencja do Bloba zawierającego zawartość zbioru wynikowego
        /// </summary>
        public string ReferenceToBlob { get; set; }
        /// <summary>
        /// Postęp klasyfikacji
        /// </summary>
        public string Progress { get; set; }
        /// <summary>
        /// Format pliku
        /// </summary>
        public string FileExtension { get; set; }

        public ResultSetEntity()
            : base(string.Empty,string.Empty)
        {
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="userId">Id użytkownika dokonującego klasyfikacji</param>
        /// <param name="resultSetId">Id zbioru wynikowego</param>
        /// <param name="numberOfClasses">Liczba klas</param>
        /// <param name="numberOfAttributes">Liczba cech</param>
        /// <param name="timeOfEntry">Data wprowadzenia</param>
        /// <param name="comment">Komentarz</param>
        /// <param name="trainingSetFileSource">Adres pliku ze zbiorem uczącym</param>
        /// <param name="inputFileSource">Adres pliku ze zbiorem wejściowym do klasyfikacji</param>
        /// <param name="resultSetFileSource">Adres pliku ze zbiorem wynikowym</param>
        /// <param name="referenceToBlob">Referencja do Bloba zawierającego zawartość zbioru wynikowego</param>
        /// <param name="methodOfClassification">Sposób klasyfikacji (0-k-NN,1-k-NN Chaudhuriego, 2-k-NN Kellera)</param>
        /// <param name="progress">Postęp klasyfikacji</param>
        /// <param name="fileExtension">Format pliku</param>
        public ResultSetEntity(string userId,string resultSetId,int numberOfClasses,int numberOfAttributes,DateTime timeOfEntry,string comment,string trainingSetFileSource,string inputFileSource,string resultSetFileSource,string referenceToBlob,string methodOfClassification,string progress,string fileExtension)
            : base(userId,resultSetId)
        {
            NumberOfClasses = numberOfClasses;
            NumberOfAttributes = numberOfAttributes;
            DateOfEntry = timeOfEntry;
            Comment = comment;
            TrainingSetFileSource = trainingSetFileSource;
            InputFileSource = inputFileSource;
            ResultSetFileSource = resultSetFileSource;
            MethodOfClassification = methodOfClassification;
            ReferenceToBlob=referenceToBlob;
            Progress = progress;
            FileExtension = fileExtension;
        }
    }
}