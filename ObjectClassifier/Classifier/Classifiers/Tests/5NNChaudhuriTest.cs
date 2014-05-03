using Classifier.Classifiers;
using Classifier.Classifiers.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Classifier.Classifiers.Tests
{
    public class _5NNChaudhuriClassifierTest : ClassifyStrategyAbstract
    {
        private double[] GetCenterOfGravity(IList<TrainingSample> points, double[] testedPoint)
        {
            double[] centerOfGravity = new double[testedPoint.Length];
            for (int i = 0; i < testedPoint.Length; i++)
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



        public override string Classify(Classifiers.Common.TrainingSample[] trainingSampleSet, Classifiers.Common.ResultSample[] resultSampleSet2, Classifiers.Common.IResultSetBuilder resultSetBuilder, WebRole.Controllers.ResultSetsController resultSetsController, string userId, string resultSetId)
        {

            List<TrainingSample> uczacy = new List<TrainingSample>();
            List<TrainingSample> testujacy = new List<TrainingSample>();
            List<TrainingSample> dosprawdzenia = new List<TrainingSample>();
            for (int i = 0; i < trainingSampleSet.Length; i++)
            {
                if (!(i % 5 == 2))
                {
                    uczacy.Add(new TrainingSample(trainingSampleSet[i]));
                }
                else
                {
                    testujacy.Add(new TrainingSample(trainingSampleSet[i]));
                    dosprawdzenia.Add(new TrainingSample(trainingSampleSet[i]));
                }
            }
            trainingSampleSet = uczacy.ToArray();
            TrainingSample[] resultSampleSet = testujacy.ToArray();
            Stopwatch watch = new Stopwatch();
            watch.Start();

            IList<TrainingSample> nearestPointsUsingCenterOfGravity = new List<TrainingSample>();
            for (int i = 0; i < resultSampleSet.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    nearestPointsUsingCenterOfGravity.Add(trainingSampleSet.OrderBy(o => EuclideanMetric(resultSampleSet[i].Attributes, GetCenterOfGravity(nearestPointsUsingCenterOfGravity, o.Attributes))).First());
                }
                resultSampleSet[i].ClassOfSample = nearestPointsUsingCenterOfGravity.GroupBy(o => o.ClassOfSample).OrderByDescending(o => o.Count()).ThenByDescending(o => o.Key).First().Key;
                nearestPointsUsingCenterOfGravity.Clear();
            }

            watch.Stop();
            double good = 0;
            for (int i = 0; i < testujacy.Count; i++)
            {
                if (testujacy.ElementAt(i).ClassOfSample == dosprawdzenia.ElementAt(i).ClassOfSample)
                {
                    good = good + 1;
                }
            }
            return "5nn chaudhuri classifier, poprawnosc:" +(good*1.0 / testujacy.Count).ToString() + "   czas:" + watch.Elapsed;

        }

    }
}
