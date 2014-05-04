using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Classifiers.Common
{
    /// <summary>
    /// Implementacja wzorca projektowego builder budującego zbiór wynikowy
    /// </summary>
    public class ResultSetBuilderImpl:IResultSetBuilder

    {
        private string _resultSet;

        public ResultSetBuilderImpl()
        {
            _resultSet = string.Empty;
        }

        /// <summary>
        /// Metoda dodająca nowy wiersz do zbioru wynikowego
        /// </summary>
        /// <param name="resultSample">Elelement wynikowy do umieszczenia w zbiorze wynikowym</param>
        public void BuildResultSample(ResultSample resultSample)
        {
            _resultSet += resultSample.ClassOfSample.ToString();
            _resultSet += '\t';
            for (int i = 0; i < resultSample.Attributes.Length; i++)
            {
                _resultSet += resultSample.Attributes[i].ToString();
                _resultSet += '\t';
            }
            _resultSet += '\n';
        }
        /// <summary>
        /// Metoda pobierająca rezultat budowy zbioru wynikowego
        /// </summary>
        /// <returns>Zbiór wynikowy</returns>
        public string GetResultSet()
        {
            if (_resultSet.Length > 0)
            {
                _resultSet = _resultSet.Substring(0, _resultSet.Length - 1);
            }
            return _resultSet;
        }
    }
}
