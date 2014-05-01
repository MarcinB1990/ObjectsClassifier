using Classifier.Classifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Classifier
{
    public class AreasOfClassessClassifier:ClassifyStrategyAbstract
    {

        public override string Classify(Classifiers.Common.TrainingSample[] trainingSampleSet, Classifiers.Common.ResultSample[] resultSampleSet, Classifiers.Common.IResultSetBuilder resultSetBuilder, WebRole.Controllers.ResultSetsController resultSetsController, string userId, string resultSetId)
        {
            throw new NotImplementedException();
        }
    }
}
