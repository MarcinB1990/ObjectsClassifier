using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{

    /// <summary>
    /// Encja zbioru uczącego. UserId stanowi PartitionKey, a TrainingSetId - RowKey
    /// </summary>
    public class TrainingSetEntity:TableEntity
    {
        /// <summary>
        /// Nazwa użytkownika wprowadzającego zbiór uczący
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Nazwa zbioru uczącego
        /// </summary>
        public string Name { get; set; }
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
        /// Referencja do Bloba zawierającego zawartość zbioru uczącego
        /// </summary>
        public string ReferenceToBlob { get; set; }
        /// <summary>
        /// Adres pliku ze zbiorem uczącym
        /// </summary>
        public string TrainingSetFileSource { get; set; }
        /// <summary>
        /// Liczba użyć
        /// </summary>
        public int NumberOfUses { get; set; }
        /// <summary>
        /// Prawa dostępu
        /// </summary>
        public string AccessRights { get; set; }

        public TrainingSetEntity()
            : base(string.Empty,string.Empty)
        {
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="userId">Id użytkownika wprowadzającego zbiór uczący</param>
        /// <param name="trainingSetId">Id zbioru uczącego</param>
        /// <param name="userName">Nazwa użytkownika wprowadzającego zbiór uczący</param>
        /// <param name="name">Nazwa zbioru uczącego</param>
        /// <param name="numberOfClasses">Liczba klas</param>
        /// <param name="numberOfAttributes">Liczba cech</param>
        /// <param name="dateOfEntry">Data wprowadzenia</param>
        /// <param name="comment">Komentarz</param>
        /// <param name="referenceToBlob">Referencja do Bloba zawierającego zawartość zbioru uczącego</param>
        /// <param name="trainingSetFileSource">Adres pliku ze zbiorem uczącym</param>
        /// <param name="numberOfUses">Liczba użyć</param>
        /// <param name="accessRights">Prawa dostępu</param>
        public TrainingSetEntity(string userId, string trainingSetId, string userName, string name, int numberOfClasses, int numberOfAttributes, DateTime dateOfEntry, string comment, string referenceToBlob, string trainingSetFileSource, int numberOfUses,string accessRights)
            : base(userId,trainingSetId)
        {
            UserName = userName;
            Name = name;
            NumberOfClasses = numberOfClasses;
            NumberOfAttributes = numberOfAttributes;
            DateOfEntry = dateOfEntry;
            Comment = comment;
            ReferenceToBlob = referenceToBlob;
            TrainingSetFileSource = trainingSetFileSource;
            NumberOfUses = numberOfUses;
            AccessRights = accessRights;
        }
    }
}