using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRole.Controllers
{
    public interface IMessageBuilder
    {
        void BuildGuid(Guid guid);
        void BuildResultSetId(string resultSetId);
        void BuildUsedUserIdToResult(string usedUserIdToResult);
        void BuildRemoveResultAfterClassification(bool removeResultAfterClassification);
        void BuildTrainingSetId(string trainingSetId);
        void BuildUsedUserIdToTraining(string usedUserIdToTraining);
        void BuildRemoveTrainingAfterClassification(bool removeTrainingAfterClassification);
        string GetMessage();
    }
}
