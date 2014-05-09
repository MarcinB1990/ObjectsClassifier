using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Classifiers.Common
{
    /// <summary>
    /// Definiuje interfejs dla wzorca projektowego Builder budującego zbiór wynikowy
    /// </summary>
    public interface IResultSetBuilder
    {
        /// <summary>
        /// Definicja metody dodającej nowy wiersz do zbioru wynikowego
        /// </summary>
        /// <param name="resultSample">Elelement wynikowy do umieszczenia w zbiorze wynikowym</param>
        void BuildResultSample(ResultSample resultSample);


        /// <summary>
        /// Definicja metody pobierającej rezultat budowy zbioru wynikowego
        /// </summary>
        /// <returns>Zbiór wynikowy</returns>
        string GetResultSet();
    }
}
