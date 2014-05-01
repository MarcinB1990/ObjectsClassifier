using Classifier.Classifiers.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRole.Controllers;

namespace Classifier
{
    public interface IClassifyStrategy
    {
        string Classify(TrainingSample[] trainingSampleSet, ResultSample[] resultSampleSet, IResultSetBuilder resultSetBuilder,ResultSetsController resultSetsController, string userId, string resultSetId);
        double EuclideanMetric(double[] attributes1, double[] attributes2);
    }
}
