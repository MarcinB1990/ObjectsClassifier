using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Classifiers.Common
{
    /// <summary>
    /// Element wynikowy
    /// </summary>
    public class ResultSample
    {
        /// <summary>
        /// Klasa elementu wynikowego
        /// </summary>
        public int ClassOfSample { get; set; }
        /// <summary>
        /// Tablica cech elementu wynikowego
        /// </summary>
        public double[] Attributes { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="data">Tablica cech elementu wynikowego</param>
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
