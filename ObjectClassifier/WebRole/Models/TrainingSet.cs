using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    /// <summary>
    /// Model zbioru uczącego
    /// </summary>
    public class TrainingSet
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
        /// Komentarz
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Zawartość pliku ze zbiorem uczącym
        /// </summary>
        public Stream FileStream { get; set; }
        /// <summary>
        /// Nazwa pliku ze zbiorem uczącym
        /// </summary>
        public string NameOfFile { get; set; }
        /// <summary>
        /// Liczba użyć
        /// </summary>
        public int NumberOfUses { get; set; }
        /// <summary>
        /// Prawa dostępu
        /// </summary>
        public int AccessRights { get; set; }
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="userId">Id użytkownika wprowadzającego zbiór uczący</param>
        /// <param name="userName">Nazwa użytkownika wprowadzającego zbiór uczący</param>
        /// <param name="name">Nazwa zbioru uczącego</param>
        /// <param name="numberOfClasses">Liczba klas</param>
        /// <param name="numberOfAttributes">Liczba cech</param>
        /// <param name="comment">Komentarz</param>
        /// <param name="fileStream">Zawartość pliku ze zbiorem uczącym</param>
        /// <param name="nameOfFile">Nazwa pliku ze zbiorem uczącym</param>
        /// <param name="numberOfUses">Liczba użyć</param>
        /// <param name="accessRights">Prawa dostępu</param>
        public TrainingSet(string userId,string userName,string name, int numberOfClasses, int numberOfAttributes, string comment, Stream fileStream, string nameOfFile,int numberOfUses,int accessRights)
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
            AccessRights = accessRights;
        }
    }
}