using Classifier.Classifiers;
using Classifier.Classifiers.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Classifier
{
    /// <summary>
    /// Implementacja interfejsu IClassifyStrategy dla klasyfikacji metodą k-NN Chaudhuriego
    /// </summary>
    public class KNNChaudhuriClassifier:ClassifyStrategyAbstract
    {
        /// <summary>
        /// Metoda pomocnicza wyznaczająca środek ciężkości dla punktów z listy elementów uczących oraz pojedynczego punktu
        /// </summary>
        /// <param name="points">Lista elementów uczących</param>
        /// <param name="testedPoint">Tablica cech testowanego punktu</param>
        /// <returns>Tablica cech będących środkiem ciężkości dla parametrów wejściowych</returns>
        private double[] GetCenterOfGravity(IList<TrainingSample> points,double[] testedPoint){
            double[] centerOfGravity=new double[testedPoint.Length];
            for (int i = 0; i < testedPoint.Length;i++ )
            {
                double sum = 0;
                for (int j = 0; j < points.Count; j++)
                {
                    sum += points.ElementAt(j).Attributes[i];
                }
                sum += testedPoint[i];
                centerOfGravity[i] = sum / (points.Count + 1.0);
            }
            return centerOfGravity;
        }

        /// <summary>
        /// Metoda dokonująca klasyfikacji wzorca z wykorzystaniem klasyfikatora k-NN Chaudhuriego
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
            IList<TrainingSample> nearestPointsUsingCenterOfGravity = new List<TrainingSample>();
            resultSetsController.UpdateProgress(userId, resultSetId, "0%");
            for (int i = 0; i < resultSampleSet.Length; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    nearestPointsUsingCenterOfGravity.Add(trainingSampleSet.TakeKMin(o=>EuclideanMetric(resultSampleSet[i].Attributes,GetCenterOfGravity(nearestPointsUsingCenterOfGravity,o.Attributes)),1).First());
                }
                resultSampleSet[i].ClassOfSample=nearestPointsUsingCenterOfGravity.GroupBy(o=>o.ClassOfSample).OrderByDescending(o=>o.Count()).ThenByDescending(o=>o.Key).First().Key;
                nearestPointsUsingCenterOfGravity.Clear();
                resultSetBuilder.BuildResultSample(resultSampleSet[i]);
                resultSetsController.UpdateProgress(userId, resultSetId, (i*100 / resultSampleSet.Length).ToString() + "%");
            }
            return resultSetBuilder.GetResultSet();
        }

    }
}
