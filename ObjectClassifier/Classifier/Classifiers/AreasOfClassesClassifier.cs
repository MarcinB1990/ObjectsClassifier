using Classifier.Classifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Classifier.Classifiers.Common;

namespace Classifier
{
    /// <summary>
    /// Implementacja interfejsu IClassifyStrategy dla klasyfikacji metodą obszarów klas
    /// </summary>
    public class AreasOfClassesClassifier:ClassifyStrategyAbstract
    {
        /// <summary>
        /// Metoda dokonująca klasyfikacji wzorca z wykorzystaniem klasyfikatora wykorzystującego obszary klas
        /// </summary>
        /// <param name="trainingSampleSet">Tablica elementów uczących</param>
        /// <param name="resultSampleSet">Tablica elementów do zaklasyfikowania</param>
        /// <param name="resultSetBuilder">Builder zbioru wynikowego</param>
        /// <param name="resultSetsController">Kontroler obsługujący bazę zbiorów wynikowych</param>
        /// <param name="userId">Identyfikator użytkownika dokonującego klasyfikacji</param>
        /// <param name="resultSetId">Identyfikator zbioru wynikowego</param>
        /// <returns>Zbiór wynikowy</returns>
        public override string Classify(Classifiers.Common.TrainingSample[] trainingSampleSet, Classifiers.Common.ResultSample[] resultSampleSet, Classifiers.Common.IResultSetBuilder resultSetBuilder, WebRole.Controllers.ResultSetsController resultSetsController, string userId, string resultSetId, int k)
        {
            resultSetsController.UpdateProgress(userId, resultSetId, "0%");
            IDictionary<int,double> areasOfClasses = new Dictionary<int,double>();
            IList<int> classes=trainingSampleSet.GroupBy(o => o.ClassOfSample).Select(o => o.Key).ToList();
            for (int i = 0; i < classes.Count; i++)
            {
                areasOfClasses.Add(classes.ElementAt(i), 0);
            }
            for (int i = 0; i < trainingSampleSet.Length; i++)
            {
                double min = trainingSampleSet.Where(o => o.ClassOfSample==trainingSampleSet[i].ClassOfSample && o!=trainingSampleSet[i]).Min(o => EuclideanMetric(o.Attributes, trainingSampleSet[i].Attributes));
                if (min > areasOfClasses[trainingSampleSet[i].ClassOfSample])
                {
                    areasOfClasses[trainingSampleSet[i].ClassOfSample] = min;
                }
            }
            for (int i = 0; i < resultSampleSet.Length; i++)
            {
                IList<int> resultsOfAreas = trainingSampleSet.Where(o => EuclideanMetric(o.Attributes, resultSampleSet[i].Attributes) < areasOfClasses[o.ClassOfSample]).GroupBy(o => o.ClassOfSample).Select(o=>o.Key).ToList();
                if (resultsOfAreas.Count == 1)
                {
                    resultSampleSet[i].ClassOfSample = resultsOfAreas.ElementAt(0);
                }
                else if (resultsOfAreas.Count == 0)
                {
                    resultSampleSet[i].ClassOfSample = -1;
                }
                else
                {
                    resultSampleSet[i].ClassOfSample = trainingSampleSet.TakeKMin(o => EuclideanMetric(resultSampleSet[i].Attributes, o.Attributes),k).Select(o => o.ClassOfSample).GroupBy(o => o).OrderByDescending(o => o.Count()).ThenByDescending(o => o.Key).First().Key;
                }
                resultSetBuilder.BuildResultSample(resultSampleSet[i]);
                resultSetsController.UpdateProgress(userId, resultSetId, (i*100 / resultSampleSet.Length).ToString() + "%");
            }
            return resultSetBuilder.GetResultSet();
        }
    }
}
