using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Classifiers.Common
{
    public class TrainingSample
    {
        public int ClassOfSample { get; set; }
        public double[] Attributes { get; set; }

        public TrainingSample(TrainingSample s)
        {
            this.ClassOfSample = s.ClassOfSample;
            this.Attributes = new double[s.Attributes.Length];
            for (int i = 0; i < s.Attributes.Length; i++)
            {
                this.Attributes[i] = s.Attributes[i];
            }
        }

        public TrainingSample(string[] data)
        {
            this.ClassOfSample = Int32.Parse(data[0]);
            this.Attributes = new double[data.Length - 1];
            for (int i = 1; i < data.Length; i++)
            {
                this.Attributes[i - 1] = double.Parse(data[i], CultureInfo.InvariantCulture);
            }
        }

    }
}
