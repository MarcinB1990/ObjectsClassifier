using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Controllers
{
    public class MessageBuilder:IMessageBuilder
    {
        private string _message=string.Empty;
        private void AddSeparator()
        {
            _message += "|";
        }
        public void BuildGuid(Guid guid)
        {
            if (_message != String.Empty)
            {
                throw new Exception("BuildGuid must be called at first");
            }
            _message += guid.ToString();
        }

        public void BuildResultSetId(string resultSetId)
        {
            AddSeparator();
            _message += resultSetId;
        }

        public void BuildUsedUserIdToResult(string usedUserIdToResult)
        {
            AddSeparator();
            _message += usedUserIdToResult;
        }

        public void BuildRemoveResultAfterClassification(bool removeResultAfterClassification)
        {
            AddSeparator();
            if (removeResultAfterClassification)
            {
                _message += "1";
            }
            else
            {
                _message += "0";
            }
        }

        public void BuildTrainingSetId(string trainingSetId)
        {
            AddSeparator();
            _message += trainingSetId;
        }

        public void BuildUsedUserIdToTraining(string usedUserIdToTraining)
        {
            AddSeparator();
            _message += usedUserIdToTraining;
        }

        public void BuildRemoveTrainingAfterClassification(bool removeTrainingAfterClassification)
        {
            AddSeparator();
            if (removeTrainingAfterClassification)
            {
                _message += "1";
            }
            else
            {
                _message += "0";
            }
        }
        public void BuildMethodOfClassification(int methodOfClassification)
        {
            AddSeparator();
            _message += methodOfClassification.ToString();
        }

        public string GetMessage()
        {
            return _message;
        }
    }
}