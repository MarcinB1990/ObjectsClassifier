using Classifier.Classifiers;
using Classifier.Classifiers.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebRole.Controllers;

namespace Classifier
{
    /// <summary>
    /// Implementacja interfejsu IClassifyStrategy dla klasyfikacji metodą k-NN
    /// </summary>
    public class KNNClassifier:ClassifyStrategyAbstract
    {
        /// <summary>
        /// Metoda dokonująca klasyfikacji wzorca z wykorzystaniem klasyfikatora k-NN
        /// </summary>
        /// <param name="trainingSampleSet">Tablica elementów uczących</param>
        /// <param name="resultSampleSet">Tablica elementów do zaklasyfikowania</param>
        /// <param name="resultSetBuilder">Builder zbioru wynikowego</param>
        /// <param name="resultSetsController">Kontroler obsługujący bazę zbiorów wynikowych</param>
        /// <param name="userId">Identyfikator użytkownika dokonującego klasyfikacji</param>
        /// <param name="resultSetId">Identyfikator zbioru wynikowego</param>
        /// <returns>Zbiór wynikowy</returns>
        public override string Classify(TrainingSample[] trainingSampleSet, ResultSample[] resultSampleSet, IResultSetBuilder resultSetBuilder, ResultSetsController resultSetsController, string userId, string resultSetId, int k)
        {
            resultSetsController.UpdateProgress(userId, resultSetId, "0%");
            for (int i = 0; i < resultSampleSet.Length; i++)
            {
                resultSampleSet[i].ClassOfSample = trainingSampleSet.TakeKMin(o => EuclideanMetric(resultSampleSet[i].Attributes, o.Attributes),k).Select(o => o.ClassOfSample).GroupBy(o => o).OrderByDescending(o => o.Count()).ThenByDescending(o => o.Key).First().Key;
                resultSetBuilder.BuildResultSample(resultSampleSet[i]);
                resultSetsController.UpdateProgress(userId, resultSetId, (i*100 / resultSampleSet.Length).ToString() + "%");
            }
            return resultSetBuilder.GetResultSet();
        }
    }
}
