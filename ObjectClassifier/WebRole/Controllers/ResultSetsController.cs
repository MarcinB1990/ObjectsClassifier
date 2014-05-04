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
    /// Kontroler obsługujący pamięć zbiorów wynikowych
    /// </summary>
    public class ResultSetsController
    {
        private CloudTable resultSets;
        private CloudBlobContainer inputFilesContainer;

        public ResultSetsController()
        {
            CloudStorageAccount csa = CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient ctc = csa.CreateCloudTableClient();
            resultSets = ctc.GetTableReference("resultsetstable");
            resultSets.CreateIfNotExists();
            CloudBlobClient cbc = csa.CreateCloudBlobClient();
            BlobContainerPermissions bcp = new BlobContainerPermissions();
            bcp.PublicAccess = BlobContainerPublicAccessType.Blob;
            inputFilesContainer = cbc.GetContainerReference("inputfilescontainer");
            inputFilesContainer.CreateIfNotExists();
            inputFilesContainer.SetPermissions(bcp);
        }

        /// <summary>
        /// Metoda zwracająca wszystkie zbiory wynikowe przypisane do danego użytkownika
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <returns>Lista zbiorów wynikowyc przypisanych do użytkownika</returns>
        public IEnumerable<ResultSetReturn> GetMyResultSets(string userId)
        {
            TableQuery<ResultSetEntity> queryGetResultSetsByUserId = new TableQuery<ResultSetEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId));
            return resultSets.ExecuteQuery(queryGetResultSetsByUserId).Select(o => new ResultSetReturn(o.NumberOfClasses,o.NumberOfAttributes,o.DateOfEntry,o.Comment,o.TrainingSetFileSource,o.InputFileSource,o.ResultSetFileSource,o.MethodOfClassification,o.Progress)).OrderByDescending(o => o.DateOfEntry);
        }

        /// <summary>
        /// Metoda zapisująca w pamięci nowy zbiór wynikowy
        /// </summary>
        /// <param name="resultSet">Zbiór wynikowy</param>
        /// <param name="tsc">Kontroler obsługujący pamięć zbiorów uczących</param>
        /// <returns>Id dodanego zbioru wynikowego</returns>
        public string SaveNew(ResultSet resultSet,TrainingSetsController tsc)
        {
            try
            {
                string resultSetId = Guid.NewGuid().ToString();
                string referenceToInputBlob = resultSetId + "/" + resultSet.NameOfInputFile;
                CloudBlockBlob inputBlob = inputFilesContainer.GetBlockBlobReference(referenceToInputBlob);
                inputBlob.UploadFromStream(resultSet.InputFileStream);
                string methodOfClassification = string.Empty;
                switch (resultSet.MethodOfClassification)
                {
                    case 0:
                        methodOfClassification = "5NN";
                        break;
                    case 1:
                        methodOfClassification = "5NN Chaudhuri's";
                        break;
                    case 2:
                        methodOfClassification="5NN Keller's";
                        break;
                }
                ResultSetEntity rse = new ResultSetEntity(resultSet.UserId, resultSetId, resultSet.NumberOfClasses, resultSet.NumberOfAttributes, DateTime.Now, resultSet.Comment, tsc.GetTrainingSetFileSourceSourceById(resultSet.UsedUserId, resultSet.TrainingSetId), inputBlob.Uri.AbsoluteUri, string.Empty, referenceToInputBlob,methodOfClassification, "in queue");
                TableOperation insertOperation = TableOperation.Insert(rse);
                resultSets.Execute(insertOperation);
                return resultSetId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Metoda zwracająca referencje do Bloba zawierającego zawartość zbioru wynikowego
        /// </summary>
        /// <param name="userId">Id użytkownika, do którego przypisany jest zbiór wynikowy</param>
        /// <param name="resultSetId">Id zbiory wynikowego</param>
        /// <returns>Referencja do Bloba zawierającego zawartość zbioru wynikowego</returns>
        public string GetResultSetReferenceToBlobById(string userId, string resultSetId)
        {
            TableOperation selectById = TableOperation.Retrieve<ResultSetEntity>(userId, resultSetId);
            TableResult tr = resultSets.Execute(selectById);
            return ((ResultSetEntity)tr.Result).ReferenceToBlob;
        }

        /// <summary>
        /// Metoda aktualizująca adres URI zbioru wynikowego
        /// </summary>
        /// <param name="userId">Id użytkownika, do którego przypisany jest zbiór wynikowy</param>
        /// <param name="resultSetId">Id zbioru wynikowego</param>
        /// <param name="resultSetURI">Nowy adres URI</param>
        public void UpadateUri(string userId, string resultSetId, string resultSetURI)
        {
            TableOperation selectById = TableOperation.Retrieve<ResultSetEntity>(userId, resultSetId);
            TableResult tr = resultSets.Execute(selectById);
            ResultSetEntity tse = ((ResultSetEntity)tr.Result);
            if (tse != null)
            {
                tse.ResultSetFileSource = resultSetURI;
                TableOperation update = TableOperation.Replace(tse);
                resultSets.Execute(update);
            }
        }

        /// <summary>
        /// Metoda zwracająca nazwę pliku zawierającego zbiór wynikowy
        /// </summary>
        /// <param name="userId">Id użytkownika, do którego przypisany jest zbiór wynikowy</param>
        /// <param name="resultSetId">Id zbioru wynikowego</param>
        /// <returns>Zwraca nazwę pliku zawierjącego zbiór wynikowy</returns>
        public string GetResultSetFileNameById(string userId, string resultSetId)
        {
            TableOperation selectById = TableOperation.Retrieve<ResultSetEntity>(userId, resultSetId);
            TableResult tr = resultSets.Execute(selectById);
            string[] ifs= ((ResultSetEntity)tr.Result).InputFileSource.Split('/');
            int indexOfStartOfExtension = ifs.Last().LastIndexOf(".");
            return ifs.Last().Substring(0,indexOfStartOfExtension);
        }

        /// <summary>
        /// Metoda aktualizująca postęp klasyfikacji
        /// </summary>
        /// <param name="userId">Id użytkownika, do którego przypisany jest zbiór wynikowy</param>
        /// <param name="resultSetId">Id zbioru wynikowego</param>
        /// <param name="progress">Postęp klasyfikacji</param>
        public void UpdateProgress(string userId, string resultSetId, string progress)
        {
            TableOperation selectById = TableOperation.Retrieve<ResultSetEntity>(userId, resultSetId);
            TableResult tr = resultSets.Execute(selectById);
            ResultSetEntity tse = ((ResultSetEntity)tr.Result);
            if (tse != null)
            {
                tse.Progress = progress;
                TableOperation update = TableOperation.Replace(tse);
                resultSets.Execute(update);
            }
        }

        /// <summary>
        /// Metoda usuwająca zbiór wynikowy
        /// </summary>
        /// <param name="userId">Id użytkownika, do którego przypisany jest zbiór wynikowy</param>
        /// <param name="resultSetId">Id zbioru wynikowego</param>
        public void DeleteResultSet(string userId, string resultSetId)
        {
            TableOperation rowToDelete=TableOperation.Retrieve<ResultSetEntity>(userId, resultSetId);
            TableResult tr=resultSets.Execute(rowToDelete);
            ResultSetEntity trResult = (ResultSetEntity)tr.Result;
            if (trResult != null)
            {
                TableOperation delete = TableOperation.Delete(trResult);
                resultSets.ExecuteAsync(delete);
            }
        }
    }
}