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
        public override string Classify(TrainingSample[] trainingSampleSet, ResultSample[] resultSampleSet, IResultSetBuilder resultSetBuilder, WebRole.Controllers.ResultSetsController resultSetsController, string userId, string resultSetId, int k)
        {
            resultSetsController.UpdateProgress(userId, resultSetId, "0%");
            string results = "Classifier;Correctness;Time\n";
            KNNClassifierTest nt = new KNNClassifierTest();
            results += nt.Classify(trainingSampleSet, resultSampleSet, resultSetBuilder, resultSetsController, userId, resultSetId,k) + "\n";
            resultSetsController.UpdateProgress(userId, resultSetId, "33%");
            KNNChaudhuriClassifierTest ct = new KNNChaudhuriClassifierTest();
            results += ct.Classify(trainingSampleSet, resultSampleSet, resultSetBuilder, resultSetsController, userId, resultSetId,k) + "\n";
            resultSetsController.UpdateProgress(userId, resultSetId, "66%");
            KNNKellerTest kt = new KNNKellerTest();
            results += kt.Classify(trainingSampleSet, resultSampleSet, resultSetBuilder, resultSetsController, userId, resultSetId,k) + "\n";
            resultSetsController.UpdateProgress(userId, resultSetId, "100%");
            AreasOfClassesClassifierTest at = new AreasOfClassesClassifierTest();
            results += at.Classify(trainingSampleSet, resultSampleSet, resultSetBuilder, resultSetsController, userId, resultSetId,k) + "\n";
            resultSetsController.UpdateProgress(userId, resultSetId, "100%");
            return results;
        }
    }
}
