using Classifier.Classifiers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Classifiers
{
    /// <summary>
    /// Implementacja interfejsu IClassifyStrategy dla klasyfikacji metodą k-NN Kellera
    /// </summary>
    public class KNNKellerClassifier:ClassifyStrategyAbstract
    {
        /// <summary>
        /// Metoda pomocnicza zamieniająca metrykę euklidesową na minimalną możliwą wartość dodatnią w przypadku, gdy równa się ona 0
        /// </summary>
        /// <param name="attributes1">Cechy pierwszego elemenetu</param>
        /// <param name="attributes2">Cechy drugiego elementu</param>
        /// <returns>Zwraca metrykę euklidesową lub Double.Epsilon, gdy jest ona równa 0.</returns>
        private double NonZeroEuclideanMetric(double[] attributes1, double[] attributes2)
        {
            double euclideanMetric = EuclideanMetric(attributes1, attributes2);
            if (euclideanMetric == 0)
            {
                return Double.Epsilon;
            }
            else
            {
                return euclideanMetric;
            }
        }

        /// <summary>
        /// Metoda dokonująca klasyfikacji wzorca z wykorzystaniem klasyfikatora k-NN Kellera
        /// </summary>
        /// <param name="trainingSampleSet">Tablica elementów uczących</param>
        /// <param name="resultSampleSet">Tablica elementów do zaklasyfikowania</param>
        /// <param name="resultSetBuilder">Builder zbioru wynikowego</param>
        /// <param name="resultSetsController">Kontroler obsługujący bazę zbiorów wynikowych</param>
        /// <param name="userId">Identyfikator użytkownika dokonującego klasyfikacji</param>
        /// <param name="resultSetId">Identyfikator zbioru wynikowego</param>
        /// <returns>Zbiór wynikowy</returns>
        public override string Classify(Common.TrainingSample[] trainingSampleSet, Common.ResultSample[] resultSampleSet, Common.IResultSetBuilder resultSetBuilder, WebRole.Controllers.ResultSetsController resultSetsController, string userId, string resultSetId, int k)
        {
            resultSetsController.UpdateProgress(userId, resultSetId, "0%");
            IList<int> classes = trainingSampleSet.GroupBy(o => o.ClassOfSample).Select(o => o.Key).ToList();
            IDictionary<TrainingSample, IDictionary<int, double>> belongingVectors = new Dictionary<TrainingSample, IDictionary<int, double>>();
            for (int i = 0; i < trainingSampleSet.Length; i++)
            {
                IList<TrainingSample> nearestPoints = trainingSampleSet.Where(o => o != trainingSampleSet[i]).ToList().TakeKMin(o => EuclideanMetric(o.Attributes, trainingSampleSet[i].Attributes), k);
                IDictionary<int, double> belongingVector = new Dictionary<int, double>();
                for (int j = 0; j < classes.Count; j++)
                {
                    belongingVector.Add(classes.ElementAt(j), 0.49*nearestPoints.Where(o => o.ClassOfSample == classes.ElementAt(j)).Count() / (double)k);
                }
                belongingVector[trainingSampleSet[i].ClassOfSample] = belongingVector[trainingSampleSet[i].ClassOfSample] + 0.51;
                belongingVectors.Add(trainingSampleSet[i], belongingVector);
                resultSetsController.UpdateProgress(userId, resultSetId, (i*50 / trainingSampleSet.Length).ToString() + "%");
            }

            for (int i = 0; i < resultSampleSet.Length; i++)
            {
                IList<System.Collections.Generic.KeyValuePair<TrainingSample, IDictionary<int, double>>> nearestPoints = belongingVectors.ToList().TakeKMin(o => EuclideanMetric(o.Key.Attributes, resultSampleSet[i].Attributes), k);
                IDictionary<int, double> belongingVector = new Dictionary<int, double>();
                for (int j = 0; j < classes.Count; j++)
                {
                    belongingVector.Add(classes.ElementAt(j), nearestPoints.Sum(o => (o.Value[classes.ElementAt(j)]) / (Math.Pow(NonZeroEuclideanMetric(resultSampleSet[i].Attributes, o.Key.Attributes), (2.0 / (classes.Count - 1))))));
                }
                resultSampleSet[i].ClassOfSample = belongingVector.OrderByDescending(o => o.Value).First().Key;
                resultSetBuilder.BuildResultSample(resultSampleSet[i]);
                resultSetsController.UpdateProgress(userId, resultSetId, ((i * 50 / resultSampleSet.Length)+50).ToString() + "%");
            }

            return resultSetBuilder.GetResultSet();
        }
    }
}
