using Classifier.Classifiers.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRole.Controllers;

namespace Classifier.Classifiers
{
    public abstract class ClassifyStrategyAbstract:IClassifyStrategy
    {
        public abstract string Classify(TrainingSample[] trainingSampleSet, ResultSample[] resultSampleSet, IResultSetBuilder resultSetBuilder, ResultSetsController resultSetsController, string userId, string resultSetId);
        public double EuclideanMetric(double[] attributes1, double[] attributes2)
        {
            if (attributes1.Length != attributes2.Length)
                throw new ArithmeticException("Input sets sizes into euclidan metric aren't eqal");
            double sum = 0;
            for (int i = 0; i < attributes1.Length; i++)
            {
                sum += (attributes1[i] - attributes2[i]) * (attributes1[i] - attributes2[i]);
            }
            return sum;
        }
    }
}
