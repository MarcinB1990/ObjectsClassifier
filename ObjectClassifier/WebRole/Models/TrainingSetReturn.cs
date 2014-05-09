using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    /// <summary>
    /// Model zbioru uczącego
    /// </summary>
    public class TrainingSetReturn
    {
        /// <summary>
        /// Id użytkownika wprowadzającego zbiór uczący
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Nazwa użytkownika wprowadzającego zbiór uczący
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Id zbioru uczącego
        /// </summary>
        public string TrainingSetId { get; set; }
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
        /// Liczba użyć
        /// </summary>
        public int NumberOfUses { get; set; }
        /// <summary>
        /// Adres pliku ze zbiorem uczącym
        /// </summary>
        public string TrainingSetFileSource { get; set; }
        /// <summary>
        /// Prawa dostępu
        /// </summary>
        public string AccessRights { get; set; }
             
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="trainingSetId">Id pliku uczącego</param>
        /// <param name="name">Nazwa pliku uczącego</param>
        /// <param name="numberOfClasses">Liczba klas</param>
        /// <param name="numberOfAttributes">Liczba cech</param>
        /// <param name="dateOfEntry">Data wprowadzenia</param>
        /// <param name="comment">Komentarz</param>
        /// <param name="numberOfUses">Liczba użyć</param>
        /// <param name="trainingSetFileSource">Adres pliku ze zbiorem uczącym</param>
        /// <param name="accessRights">Prawa dostępu</param>
        /// <param name="userId">Id użytkownika wprowadzającego zbiór uczący</param>
        /// <param name="userName">Nazwa użytkownika wprowadzającego zbiór uczący</param>
        public TrainingSetReturn(string trainingSetId,string name, int numberOfClasses, int numberOfAttributes,DateTime dateOfEntry, string comment, int numberOfUses,string trainingSetFileSource,string accessRights,string userName,string userId)
        {
            UserId = userId;
            UserName = userName;
            TrainingSetId = trainingSetId;
            Name = name;
            NumberOfClasses = numberOfClasses;
            NumberOfAttributes = numberOfAttributes;
            Comment = comment;
            DateOfEntry = dateOfEntry;
            NumberOfUses = numberOfUses;
            TrainingSetFileSource = trainingSetFileSource;
            AccessRights = accessRights;
        }
    }
}