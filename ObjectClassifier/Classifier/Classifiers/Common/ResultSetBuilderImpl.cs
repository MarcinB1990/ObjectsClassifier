using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Classifiers.Common
{
    public class ResultSetBuilderImpl:IResultSetBuilder

    {
        private string _resultSet;
        public ResultSetBuilderImpl()
        {
            _resultSet = string.Empty;
        }

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
