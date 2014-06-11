using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Classifiers.Common
{
    public static class ExtensionMethods
    {
        public static TSource ElementWithMaxValue<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> metric) where TKey : IComparable
        {
            TSource maxElem = source.First();
            TKey maxVal = metric.Invoke(maxElem);
            for (int i = 1; i < source.Count(); i++)
            {
                TSource newElem = source.ElementAt(i);
                TKey newVal = metric.Invoke(newElem);
                if (newVal.CompareTo(maxVal) > 0)
                {
                    maxVal = newVal;
                    maxElem = newElem;
                }
            }
            return maxElem;
        }

        public static List<TSource> TakeKMin<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> metric, int k) where TKey : IComparable
        {
            List<TSource> kbest = source.Take(k).ToList();
            if (source.Count()>k)
            {
                TSource maxElem = kbest.ElementWithMaxValue(metric);
                TKey maxVal = metric.Invoke(maxElem);
                for (int i = k; i < source.Count(); i++)
                {
                    TSource newElem = source.ElementAt(i);
                    TKey newVal = metric(newElem);
                    if (newVal.CompareTo(maxVal) < 0)
                    {
                        kbest.Remove(maxElem);
                        kbest.Add(newElem);
                        maxElem = kbest.ElementWithMaxValue(metric);
                        maxVal = metric.Invoke(maxElem);
                    }

                }
            }
            return kbest;
        }
    }
}
