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
using WebRole.Controllers;

namespace Classifier.Classifiers.Tests
{
    /// <summary>
    /// Klasa testująca czas działania i dokładność klasyfikacji metodą 5NN
    /// </summary>
    public class _5NNClassifierTest : ClassifyStrategyAbstract
    {
        public override string Classify(TrainingSample[] trainingSampleSet, ResultSample[] resultSampleSet2, IResultSetBuilder resultSetBuilder, ResultSetsController resultSetsController, string userId, string resultSetId)
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
            for (int i = 0; i < resultSampleSet.Length; i++)
            {
                resultSampleSet[i].ClassOfSample = trainingSampleSet.OrderBy(o => EuclideanMetric(resultSampleSet[i].Attributes, o.Attributes)).Take(5).Select(o => o.ClassOfSample).GroupBy(o => o).OrderByDescending(o => o.Count()).ThenByDescending(o => o.Key).First().Key;
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
            return "5nn classifier, poprawnosc:" + (good * 1.0 / testujacy.Count).ToString() + "   czas:" + watch.Elapsed;
        }
    }
}
