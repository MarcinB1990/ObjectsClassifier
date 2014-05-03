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
    public class _5NNClassifierTestOfKNumber : ClassifyStrategyAbstract
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
            string wyniki = string.Empty;
            for (int jj = 1; jj <= 10; jj++)
            {
                watch.Start();
                for (int i = 0; i < resultSampleSet.Length; i++)
                {
                    resultSampleSet[i].ClassOfSample = trainingSampleSet.OrderBy(o => EuclideanMetric(resultSampleSet[i].Attributes, o.Attributes)).Take(jj).Select(o => o.ClassOfSample).GroupBy(o => o).OrderByDescending(o => o.Count()).ThenByDescending(o => o.Key).First().Key;
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
                wyniki += "5nn classifier, liczba k:"+jj+" poprawnosc:" + (good * 1.0 / testujacy.Count).ToString() + "   czas:" + watch.Elapsed;
                watch.Reset();
            }
            return wyniki;
        }
    }
}
