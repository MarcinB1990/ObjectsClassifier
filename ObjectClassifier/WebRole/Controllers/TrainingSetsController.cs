using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRole.Models;

namespace WebRole.Controllers
{
    /// <summary>
    /// Kontroler obsługujący pamięć zbiorów uczących
    /// </summary>
    public class TrainingSetsController
    {
        private CloudTable trainingSets;
        private CloudBlobContainer trainingSetsContainer;

        public TrainingSetsController()
        {
            CloudStorageAccount csa = CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient ctc = csa.CreateCloudTableClient();
            trainingSets = ctc.GetTableReference("trainingsetstable");
            trainingSets.CreateIfNotExists();
            CloudBlobClient cbc = csa.CreateCloudBlobClient();
            BlobContainerPermissions bcp = new BlobContainerPermissions();
            bcp.PublicAccess = BlobContainerPublicAccessType.Blob;
            trainingSetsContainer = cbc.GetContainerReference("trainingsetscontainer");
            trainingSetsContainer.CreateIfNotExists();
            trainingSetsContainer.SetPermissions(bcp);
        }

        /// <summary>
        /// Metoda zapisująca w pamięci nowy zbiór uczący
        /// </summary>
        /// <param name="trainingSet">Zbiór uczący</param>
        /// <returns>Id zbioru uczącego</returns>
        public string SaveNew(TrainingSet trainingSet)
        {
            try
            {
                string trainingSetId = Guid.NewGuid().ToString();
                string referenceToBlob = trainingSetId + "/" + trainingSet.NameOfFile;
                CloudBlockBlob blob = trainingSetsContainer.GetBlockBlobReference(referenceToBlob);
                blob.UploadFromStream(trainingSet.FileStream);
                TrainingSetEntity tse = new TrainingSetEntity(trainingSet.UserId, trainingSetId, trainingSet.UserName, trainingSet.Name, trainingSet.NumberOfClasses, trainingSet.NumberOfAttributes,DateTime.Now, trainingSet.Comment,referenceToBlob, blob.Uri.AbsoluteUri, trainingSet.NumberOfUses);
                TableOperation insertOperation = TableOperation.Insert(tse);
                trainingSets.Execute(insertOperation);
                return trainingSetId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Metoda zwracająca wszystkie zbiory uczące przypisane do użytkownika
        /// </summary>
        /// <param name="userId">Id uzytkownika</param>
        /// <returns>Lista zbiorów uczących przypisanych do użytkownika</returns>
        public IEnumerable<TrainingSetReturn> GetMyTrainingSets(string userId)
        {
            TableQuery<TrainingSetEntity> queryGetTrainingSetsByUserId = new TableQuery<TrainingSetEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId));
            return trainingSets.ExecuteQuery(queryGetTrainingSetsByUserId).Select(o => new TrainingSetReturn(o.RowKey,o.Name, o.NumberOfClasses, o.NumberOfAttributes,o.DateOfEntry, o.Comment, o.NumberOfUses, o.TrainingSetFileSource)).OrderByDescending(o=>o.NumberOfUses).ThenByDescending(o=>o.DateOfEntry);
        }

        /// <summary>
        /// Metoda zwracająca adres pliku zawierającego zbiór uczący
        /// </summary>
        /// <param name="userId">Id użytkownika, do którego przypisany jest zbiór uczący</param>
        /// <param name="trainingSetId">Id zbioru uczącego</param>
        /// <returns>Źródło pliku zawierającego zbiór uczący</returns>
        public string GetTrainingSetFileSourceSourceById(string userId,string trainingSetId)
        {
            TableOperation selectById = TableOperation.Retrieve<TrainingSetEntity>(userId, trainingSetId);
            TableResult tr = trainingSets.Execute(selectById);
            return ((TrainingSetEntity)tr.Result).TrainingSetFileSource;
        }

        /// <summary>
        /// Metoda zwracająca referencje do Bloba zawierającego zawartość zbioru uczącego
        /// </summary>
        /// <param name="userId">Id użytkownika, do którego przypisany jest zbiór uczący</param>
        /// <param name="resultSetId">Id zbiory uczącego</param>
        /// <returns>Referencja do Bloba zawierającego zawartość zbioru uczącego</returns>
        public string GetTrainingSetReferenceToBlobById(string userId, string trainingSetId)
        {
            TableOperation selectById = TableOperation.Retrieve<TrainingSetEntity>(userId, trainingSetId);
            TableResult tr = trainingSets.Execute(selectById);
            return ((TrainingSetEntity)tr.Result).ReferenceToBlob;
        }

        /// <summary>
        /// Metoda usuwająca zbiór uczący
        /// </summary>
        /// <param name="userId">Id użytkownika, do którego przypisany jest zbiór uczący</param>
        /// <param name="resultSetId">Id zbioru uczącego</param>
        /// <returns>Zwraca True w przypadku, gdy udało się usunąć zbiór uczący lub false, gdy operacja zakończyła się błędem</returns>
        public bool DeleteTrainingSet(string userId,string trainingSetId)
        {
            TableOperation rowToDelete=TableOperation.Retrieve<TrainingSetEntity>(userId, trainingSetId);
            TableResult tr=trainingSets.Execute(rowToDelete);
            TrainingSetEntity trResult = (TrainingSetEntity)tr.Result;
            if (trResult != null)
            {
                TableOperation delete = TableOperation.Delete(trResult);
                trainingSets.Execute(delete);
                if (trResult.NumberOfUses == 0)
                {
                    CloudBlockBlob cbb=trainingSetsContainer.GetBlockBlobReference(trResult.ReferenceToBlob);
                    cbb.DeleteAsync();
                }
                return true;
            }
            else{
                return false;
            }
        }

        /// <summary>
        /// Metoda zwiększająca o 1 wartość licznika odwołań do zbioru uczącego
        /// </summary>
        /// <param name="userId">Id użytkownika, do którego przypisany jest zbiór uczący</param>
        /// <param name="trainingSetId">Id zbioru uczącego</param>
        public void IncrementUses(string userId, string trainingSetId)
        {
            TableOperation selectById = TableOperation.Retrieve<TrainingSetEntity>(userId, trainingSetId);
            TableResult tr = trainingSets.Execute(selectById);
            TrainingSetEntity tse = ((TrainingSetEntity)tr.Result);
            if (tse != null)
            {
                tse.NumberOfUses++;
                TableOperation update = TableOperation.Replace(tse);
                trainingSets.ExecuteAsync(update);
            }
        }
    }
}