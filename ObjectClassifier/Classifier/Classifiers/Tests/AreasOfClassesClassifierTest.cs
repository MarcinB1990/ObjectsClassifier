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
    public class AreasOfClassesClassifierTest : ClassifyStrategyAbstract
    {

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

            IDictionary<int, double> areasOfClasses = new Dictionary<int, double>();
            IList<int> classes = trainingSampleSet.GroupBy(o => o.ClassOfSample).Select(o => o.Key).ToList();
            for (int i = 0; i < classes.Count; i++)
            {
                areasOfClasses.Add(classes.ElementAt(i), 0);
            }
            for (int i = 0; i < trainingSampleSet.Length; i++)
            {
                double min = trainingSampleSet.Where(o => o.ClassOfSample == trainingSampleSet[i].ClassOfSample && o != trainingSampleSet[i]).Min(o => EuclideanMetric(o.Attributes, trainingSampleSet[i].Attributes));
                if (min > areasOfClasses[trainingSampleSet[i].ClassOfSample])
                {
                    areasOfClasses[trainingSampleSet[i].ClassOfSample] = min;
                }
            }
            for (int i = 0; i < resultSampleSet.Length; i++)
            {
                IList<int> resultsOfAreas = trainingSampleSet.Where(o => EuclideanMetric(o.Attributes, resultSampleSet[i].Attributes) < areasOfClasses[o.ClassOfSample]).GroupBy(o => o.ClassOfSample).Select(o => o.Key).ToList();
                if (resultsOfAreas.Count == 1)
                {
                    resultSampleSet[i].ClassOfSample = resultsOfAreas.ElementAt(0);
                }
                else if (resultsOfAreas.Count == 0)
                {
                    resultSampleSet[i].ClassOfSample = -1;
                }
                else
                {
                    resultSampleSet[i].ClassOfSample = trainingSampleSet.OrderBy(o => EuclideanMetric(resultSampleSet[i].Attributes, o.Attributes)).Take(5).Select(o => o.ClassOfSample).GroupBy(o => o).OrderByDescending(o => o.Count()).ThenByDescending(o => o.Key).First().Key;
                }
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
            return "areas of classes, poprawnosc:" + (good * 1.0 / testujacy.Count).ToString() + "   czas:" + watch.Elapsed;

        }
    }
}
