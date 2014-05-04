using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Classifiers.Common
{
    /// <summary>
    /// Element uczący
    /// </summary>
    public class TrainingSample
    {
        /// <summary>
        /// Klasa elementu uczącego
        /// </summary>
        public int ClassOfSample { get; set; }
        /// <summary>
        /// Tablica cech elementu uczącego
        /// </summary>
        public double[] Attributes { get; set; }

        /// <summary>
        /// Konstruktor kopiujący
        /// </summary>
        /// <param name="s">Element uczący do skopiowania</param>
        public TrainingSample(TrainingSample s)
        {
            this.ClassOfSample = s.ClassOfSample;
            this.Attributes = new double[s.Attributes.Length];
            for (int i = 0; i < s.Attributes.Length; i++)
            {
                this.Attributes[i] = s.Attributes[i];
            }
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="data">Tablica zawierająca na pierwszym miejscu klasę elementu uczącego a na kolejnych cechy elementu uczącego</param>
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
