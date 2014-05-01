using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Classifiers.Common
{
    public interface IResultSetBuilder
    {
        void BuildResultSample(ResultSample resultSample);
        string GetResultSet();
    }
}
