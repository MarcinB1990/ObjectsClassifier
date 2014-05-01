using Classifier.Classifiers;
using Classifier.Classifiers.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebRole.Controllers;

namespace Classifier
{
    public class _5NNClassifier:ClassifyStrategyAbstract
    {
        public override string Classify(TrainingSample[] trainingSampleSet, ResultSample[] resultSampleSet, IResultSetBuilder resultSetBuilder, ResultSetsController resultSetsController, string userId, string resultSetId)
        {
            resultSetsController.UpdateProgress(userId, resultSetId, "0%");
            for (int i = 0; i < resultSampleSet.Length; i++)
            {
                resultSampleSet[i].ClassOfSample = trainingSampleSet.OrderBy(o => EuclideanMetric(resultSampleSet[i].Attributes, o.Attributes)).Take(5).Select(o => o.ClassOfSample).GroupBy(o => o).OrderByDescending(o => o.Count()).ThenByDescending(o => o.Key).First().Key;
                resultSetBuilder.BuildResultSample(resultSampleSet[i]);
                resultSetsController.UpdateProgress(userId, resultSetId, (i / resultSampleSet.Length).ToString() + "%");
            }
            return resultSetBuilder.GetResultSet();
        }
    }
}
