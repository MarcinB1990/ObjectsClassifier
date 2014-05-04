using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRole.Controllers
{
    /// <summary>
    /// Interfejs dla wzorca projektowego builder budującego wiadomość dla kolejki inputqueue 
    /// </summary>
    public interface IMessageBuilder
    {
        /// <summary>
        /// Definicja metody dodającej guid wiadomości
        /// </summary>
        /// <param name="guid">Guid wiadomości</param>
        void BuildGuid(Guid guid);

        /// <summary>
        /// Definicja metody dodającej id zbioru wynikowego
        /// </summary>
        /// <param name="resultSetId">Id zbioru wynikowego</param>
        void BuildResultSetId(string resultSetId);

        /// <summary>
        /// Definicja metody dodającej id użytkownika dokonującego klasyfikacji
        /// </summary>
        /// <param name="usedUserIdToResult">Id użytkownika dokonującego klasyfikacji</param>
        void BuildUsedUserIdToResult(string usedUserIdToResult);

        /// <summary>
        /// Definicja metody określającej konieczność usunięcia zbioru wynikowego z pamięci po zakończeniu klasyfikacji
        /// </summary>
        /// <param name="removeResultAfterClassification">True dla konieczności usunięcia zbioru wynikowego z pamięci po zakończeniu klasyfikacji</param>
        void BuildRemoveResultAfterClassification(bool removeResultAfterClassification);

        /// <summary>
        /// Definicja metody dodającej id zbioru uczącego
        /// </summary>
        /// <param name="trainingSetId">Id zbioru uczącego</param>
        void BuildTrainingSetId(string trainingSetId);

        /// <summary>
        /// Definicja metody dodającej Id użytkownika, który wprowadził zbiór uczący
        /// </summary>
        /// <param name="usedUserIdToTraining">Id użytkownika, który wprowadził zbiór uczący</param>
        void BuildUsedUserIdToTraining(string usedUserIdToTraining);

        /// <summary>
        /// Definicja metody określającej konieczność usunięcia zbioru uczącego z pamięci po zakońćzeniu klasyfikacji
        /// </summary>
        /// <param name="removeTrainingAfterClassification">True dla konieczności usunięcia zbioru uczącego z pamięci po zakończeniu klasyfikacji</param>
        void BuildRemoveTrainingAfterClassification(bool removeTrainingAfterClassification);

        /// <summary>
        /// Definicja metody dodającej wybór sposobu klasyfikacji
        /// </summary>
        /// <param name="methodOfClassification">Sposób klasyfikacji (0-5NN, 1-5NN Chaudhuriego, 2-5NNKellera)</param>
        void BuildMethodOfClassification(int methodOfClassification);

        /// <summary>
        /// Definicja metody pobierającej gotową treść wiadomości
        /// </summary>
        /// <returns>Treść wiadomości</returns>
        string GetMessage();

        /// <summary>
        /// Definicja metody odkodowującej otrzymaną wiadomość
        /// </summary>
        /// <param name="receivedMessage">Wiadomość</param>
        /// <returns>Słownik zawierający poszczególne elementy otrzymanej wiadomości</returns>
        IDictionary DecodeInputMessage(CloudQueueMessage receivedMessage);
    }
}
