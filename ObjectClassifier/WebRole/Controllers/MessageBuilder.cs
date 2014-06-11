using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Controllers
{
    /// <summary>
    /// Implementacja wzorca projektowego builder budującego wiadomość dla kolejki inputqueue
    /// </summary>
    public class MessageBuilder:IMessageBuilder
    {
        private string _message=string.Empty;
        private string[] _decodedMessageDictionaryKeys = new string[] {"operationGuid","resultSetId","usedUserIdToResult","removeResultAfterClassification","trainingSetId","usedUserIdToTraining","removeTrainingAfterClassification","methodOfClassification","extensionOfOutputFile" };

        private void AddSeparator()
        {
            _message += "|";
        }

        /// <summary>
        /// Metoda dodająca do wiadomości guid wiadomości
        /// </summary>
        /// <param name="guid">Guid wiadomości</param>
        public void BuildGuid(Guid guid)
        {
            if (_message != String.Empty)
            {
                throw new Exception("BuildGuid must be called at first");
            }
            _message += guid.ToString();
        }

        /// <summary>
        /// Metoda dodająca do wiadomości id zbioru wynikowego
        /// </summary>
        /// <param name="resultSetId">Id zbioru wynikowego</param>
        public void BuildResultSetId(string resultSetId)
        {
            AddSeparator();
            _message += resultSetId;
        }

        /// <summary>
        /// Metoda dodająca do wiadomości id użytkownika dokonującego klasyfikacji
        /// </summary>
        /// <param name="usedUserIdToResult">Id użytkownika dokonującego klasyfikacji</param>
        public void BuildUsedUserIdToResult(string usedUserIdToResult)
        {
            AddSeparator();
            _message += usedUserIdToResult;
        }

        /// <summary>
        /// Metoda określająca konieczność usunięcia zbioru wynikowego z pamięci po zakończeniu klasyfikacji
        /// </summary>
        /// <param name="removeResultAfterClassification">True dla konieczności usunięcia zbioru wynikowego z pamięci po zakończeniu klasyfikacji</param>
        public void BuildRemoveResultAfterClassification(bool removeResultAfterClassification)
        {
            AddSeparator();
            if (removeResultAfterClassification)
            {
                _message += "1";
            }
            else
            {
                _message += "0";
            }
        }

        /// <summary>
        /// Metoda dodająca do wiadomości id zbioru uczącego
        /// </summary>
        /// <param name="trainingSetId">Id zbioru uczącego</param>
        public void BuildTrainingSetId(string trainingSetId)
        {
            AddSeparator();
            _message += trainingSetId;
        }

        /// <summary>
        /// Metoda dodająca do wiadomości Id użytkownika, który wprowadził zbiór uczący
        /// </summary>
        /// <param name="usedUserIdToTraining">Id użytkownika, który wprowadził zbiór uczący</param>
        public void BuildUsedUserIdToTraining(string usedUserIdToTraining)
        {
            AddSeparator();
            _message += usedUserIdToTraining;
        }

        /// <summary>
        /// Metoda określająca konieczność usunięcia zbioru uczącego z pamięci po zakońćzeniu klasyfikacji
        /// </summary>
        /// <param name="removeTrainingAfterClassification">True dla konieczności usunięcia zbioru uczącego z pamięci po zakończeniu klasyfikacji</param>
        public void BuildRemoveTrainingAfterClassification(bool removeTrainingAfterClassification)
        {
            AddSeparator();
            if (removeTrainingAfterClassification)
            {
                _message += "1";
            }
            else
            {
                _message += "0";
            }
        }

        /// <summary>
        /// Metody dodająca do wiadomości wybrany sposobu klasyfikacji
        /// </summary>
        /// <param name="methodOfClassification">Sposób klasyfikacji (0-k-NN, 1-k-NN Chaudhuriego, 2-k-NN Kellera)</param>
        public void BuildMethodOfClassification(int methodOfClassification)
        {
            AddSeparator();
            _message += methodOfClassification.ToString();
        }

        /// <summary>
        /// Metoda dodająca do wiadomości wybrany format pliku wyjściowego
        /// </summary>
        /// <param name="extensionOfOutputFile">Wybrany format pliku wyjściowego (0-txt, 1-csv)</param>
        public void BuildExtensionOfOutputFile(int extensionOfOutputFile)
        {
            AddSeparator();
            _message += extensionOfOutputFile.ToString();
        }

        /// <summary>
        /// Metoda pobierająca gotową treść wiadomości
        /// </summary>
        /// <returns>Treść wiadomości</returns>
        public string GetMessage()
        {
            return _message;
        }

        /// <summary>
        /// Metody odkodowująca otrzymaną wiadomość
        /// </summary>
        /// <param name="receivedMessage">Wiadomość</param>
        /// <returns>Słownik zawierający poszczególne elementy otrzymanej wiadomości</returns>
        public IDictionary DecodeInputMessage(CloudQueueMessage receivedMessage)
        {
            string[] decodedMessage = receivedMessage.AsString.Split('|');
            IDictionary decodedMessageDictionary = new Dictionary<string, string>();
            for (int i = 0; i < _decodedMessageDictionaryKeys.Length; i++)
            {
                decodedMessageDictionary.Add(_decodedMessageDictionaryKeys[i], decodedMessage[i]);

            }
            return decodedMessageDictionary;
        }
    }
}