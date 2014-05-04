using Classifier.Classifiers.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRole.Controllers;

namespace Classifier
{
    ///<summary>
    ///Definiuje interfejs wzorca projektowego strategia dla różnych metod klasyfikacji
    /// </summary>
    public interface IClassifyStrategy
    {
        /// <summary>
        /// Definicja metody dokonującej klasyfikacji wzorca
        /// </summary>
        /// <param name="trainingSampleSet">Tablica elementów uczących</param>
        /// <param name="resultSampleSet">Tablica elementów do zaklasyfikowania</param>
        /// <param name="resultSetBuilder">Builder zbioru wynikowego</param>
        /// <param name="resultSetsController">Kontroler obsługujący bazę zbiorów wynikowych</param>
        /// <param name="userId">Identyfikator użytkownika dokonującego klasyfikacji</param>
        /// <param name="resultSetId">Identyfikator zbioru wynikowego</param>
        /// <returns>Zbiór wynikowy</returns>
        string Classify(TrainingSample[] trainingSampleSet, ResultSample[] resultSampleSet, IResultSetBuilder resultSetBuilder,ResultSetsController resultSetsController, string userId, string resultSetId);

        /// <summary>
        /// Definicja metody wyznaczającej metrykę euklidesową
        /// </summary>
        /// <param name="attributes1">Cechy pierwszego elementu</param>
        /// <param name="attributes2">Cechy drugiego elementu</param>
        /// <returns>Zwraca metrykę euklidesową</returns>
        double EuclideanMetric(double[] attributes1, double[] attributes2);
    }
}
