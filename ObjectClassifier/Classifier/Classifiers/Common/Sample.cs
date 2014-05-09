using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Classifiers.Common
{
    /// <summary>
    /// Element
    /// </summary>
    public class Sample
    {
        /// <summary>
        /// Klasa elementu
        /// </summary>
        public int ClassOfSample { get; set; }
        /// <summary>
        /// Tablica cech elementu
        /// </summary>
        public double[] Attributes { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="data">Tablica cech elementu</param>
        public Sample(string[] data)
        {
            this.ClassOfSample = Int32.Parse(data[0]);
            this.Attributes = new double[data.Length];
            for (int i = 1; i < data.Length; i++)
            {
                this.Attributes[i] = double.Parse(data[i], CultureInfo.InvariantCulture);
            }
        }
    }
}
