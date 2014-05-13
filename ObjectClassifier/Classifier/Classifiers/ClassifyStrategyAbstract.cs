using Classifier.Classifiers.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRole.Controllers;

namespace Classifier.Classifiers
{
    /// <summary>
    /// Klasa abstrakcyjna implementująca metodę EuclideanMetric z interfejsu IClassifyStrategy
    /// </summary>
    public abstract class ClassifyStrategyAbstract:IClassifyStrategy
    {
        /// <summary>
        /// Definicja metody abstrakcyjnej dokonującej klasyfikacji wzorca
        /// </summary>
        /// <param name="trainingSampleSet">Tablica elementów uczących</param>
        /// <param name="resultSampleSet">Tablica elementów do zaklasyfikowania</param>
        /// <param name="resultSetBuilder">Builder zbioru wynikowego</param>
        /// <param name="resultSetsController">Kontroler obsługujący bazę zbiorów wynikowych</param>
        /// <param name="userId">Identyfikator użytkownika dokonującego klasyfikacji</param>
        /// <param name="resultSetId">Identyfikator zbioru wynikowego</param>
        /// <returns>Zbiór wynikowy</returns>
        public abstract string Classify(TrainingSample[] trainingSampleSet, ResultSample[] resultSampleSet, IResultSetBuilder resultSetBuilder, ResultSetsController resultSetsController, string userId, string resultSetId);
        
        /// <summary>
        /// Metoda pomocnicza wyznaczająca metrykę euklidesową
        /// </summary>
        /// <param name="attributes1">Cechy pierwszego elementu</param>
        /// <param name="attributes2">Cechy drugiego elementu</param>
        /// <returns>Zwraca metrykę euklidesową</returns>
        public double EuclideanMetric(double[] attributes1, double[] attributes2)
        {
            if (attributes1.Length != attributes2.Length)
                throw new ArithmeticException("Input sets sizes into euclidan metric aren't eqal");
            double sum = 0;
            for (int i = 0; i < attributes1.Length; i++)
            {
                sum += (attributes1[i] - attributes2[i]) * (attributes1[i] - attributes2[i]);
            }
            return sum;
        }
    }
}
