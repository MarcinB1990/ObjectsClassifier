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
    class _5NNChaudhuriClassifier:ClassifyStrategyAbstract
    {
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

        public override string Classify(Classifiers.Common.TrainingSample[] trainingSampleSet, Classifiers.Common.ResultSample[] resultSampleSet, Classifiers.Common.IResultSetBuilder resultSetBuilder, WebRole.Controllers.ResultSetsController resultSetsController, string userId, string resultSetId)
        {
            IList<TrainingSample> nearestPointsUsingCenterOfGravity = new List<TrainingSample>();
            resultSetsController.UpdateProgress(userId, resultSetId, "0%");
            for (int i = 0; i < resultSampleSet.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    nearestPointsUsingCenterOfGravity.Add(trainingSampleSet.OrderBy(o=>EuclideanMetric(resultSampleSet[i].Attributes,GetCenterOfGravity(nearestPointsUsingCenterOfGravity,o.Attributes))).First());
                }
                resultSampleSet[i].ClassOfSample=nearestPointsUsingCenterOfGravity.GroupBy(o=>o.ClassOfSample).OrderByDescending(o=>o.Count()).ThenByDescending(o=>o.Key).First().Key;
                nearestPointsUsingCenterOfGravity.Clear();
                resultSetBuilder.BuildResultSample(resultSampleSet[i]);
                resultSetsController.UpdateProgress(userId, resultSetId, (i / resultSampleSet.Length).ToString() + "%");
            }
            return resultSetBuilder.GetResultSet();
        }

    }
}
