using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Classifiers.Common
{
    public class ResultSample
    {
        public int ClassOfSample { get; set; }
        public double[] Attributes { get; set; }

        public ResultSample(string[] data)
        {
            this.ClassOfSample = -1;
            this.Attributes = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                this.Attributes[i] = double.Parse(data[i], CultureInfo.InvariantCulture);
            }
        }
    }
}
