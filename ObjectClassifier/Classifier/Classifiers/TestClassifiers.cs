using Classifier.Classifiers.Common;
using Classifier.Classifiers.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Classifiers
{
    class TestClassifiers:ClassifyStrategyAbstract
    {
        public override string Classify(TrainingSample[] trainingSampleSet, ResultSample[] resultSampleSet, IResultSetBuilder resultSetBuilder, WebRole.Controllers.ResultSetsController resultSetsController, string userId, string resultSetId)
        {
            string results = string.Empty;
            _5NNClassifierTest nt = new _5NNClassifierTest();
            results += nt.Classify(trainingSampleSet, resultSampleSet, resultSetBuilder, resultSetsController, userId, resultSetId) + "\n";
            _5NNChaudhuriClassifierTest ct = new _5NNChaudhuriClassifierTest();
            results += ct.Classify(trainingSampleSet, resultSampleSet, resultSetBuilder, resultSetsController, userId, resultSetId) + "\n";
            _5NNKelleraTest kt = new _5NNKelleraTest();
            results += kt.Classify(trainingSampleSet, resultSampleSet, resultSetBuilder, resultSetsController, userId, resultSetId) + "\n";
            AreasOfClassesClassifierTest at = new AreasOfClassesClassifierTest();
            results += at.Classify(trainingSampleSet, resultSampleSet, resultSetBuilder, resultSetsController, userId, resultSetId) + "\n";
            return results;
        }
    }
}
