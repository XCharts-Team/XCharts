using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class DataHelper
    {
        private static List<double> s_SampleSumPrefix = new List<double>();

        public static bool IsAnyDataChanged(ref List<SerieData> showData, int minCount, int maxCount)
        {
            for (int i = minCount; i < maxCount; i++)
            {
                if (showData[i].IsDataChanged())
                    return true;
            }
            return false;
        }

        public static List<double> BuildSampleSumPrefix(ref List<SerieData> showData, int maxCount, bool inverse)
        {
            return BuildSampleSumPrefix(ref showData, maxCount, inverse, s_SampleSumPrefix);
        }

        public static List<double> BuildSampleSumPrefix(ref List<SerieData> showData, int maxCount, bool inverse,
            List<double> sampleSumPrefix)
        {
            if (sampleSumPrefix == null)
                sampleSumPrefix = new List<double>();
            if (maxCount < 0) maxCount = 0;
            var targetCount = maxCount + 1;
            if (sampleSumPrefix.Count < targetCount)
            {
                var addCount = targetCount - sampleSumPrefix.Count;
                for (int i = 0; i < addCount; i++)
                    sampleSumPrefix.Add(0);
            }
            else if (sampleSumPrefix.Count > targetCount)
            {
                sampleSumPrefix.RemoveRange(targetCount, sampleSumPrefix.Count - targetCount);
            }

            sampleSumPrefix[0] = 0;
            for (int i = 0; i < maxCount; i++)
            {
                sampleSumPrefix[i + 1] = sampleSumPrefix[i] + showData[i].GetData(1, inverse);
            }
            return sampleSumPrefix;
        }

        public static double DataAverage(ref List<SerieData> showData, SampleType sampleType,
            int minCount, int maxCount, int rate)
        {
            double totalAverage = 0;
            if (rate > 1 && sampleType == SampleType.Peak)
            {
                double total = 0;
                for (int i = minCount; i < maxCount; i++)
                {
                    total += showData[i].data[1];
                }
                totalAverage = total / (maxCount - minCount);
            }
            return totalAverage;
        }

        public static double SampleValue(ref List<SerieData> showData, SampleType sampleType, int rate,
            int minCount, int maxCount, double totalAverage, int index, float dataAddDuration, float dataChangeDuration,
            ref bool dataChanging, Axis axis, bool unscaledTime, bool useCurrentData = true,
            bool checkDataChanging = true, List<double> sampleSumPrefix = null)
        {
            var inverse = axis.inverse;
            var minValue = 0;
            var maxValue = 0;
            if (!useCurrentData)
            {
                if (rate <= 1 || index == minCount)
                    return showData[index].GetData(1, inverse);

                switch (sampleType)
                {
                    case SampleType.Sum:
                    case SampleType.Average:
                        if (sampleSumPrefix != null)
                        {
                            var right = index + 1;
                            var left = index - rate + 1;
                            if (left >= 0 && right >= 0 && right < sampleSumPrefix.Count)
                            {
                                var totalByPrefix = sampleSumPrefix[right] - sampleSumPrefix[left];
                                if (sampleType == SampleType.Average)
                                    return totalByPrefix / rate;
                                else
                                    return totalByPrefix;
                            }
                        }
                        double total = 0;
                        for (int i = index; i > index - rate; i--)
                        {
                            total += showData[i].GetData(1, inverse);
                        }
                        if (sampleType == SampleType.Average)
                            return total / rate;
                        else
                            return total;

                    case SampleType.Max:
                        double max = double.MinValue;
                        for (int i = index; i > index - rate; i--)
                        {
                            var value = showData[i].GetData(1, inverse);
                            if (value > max)
                                max = value;
                        }
                        return max;

                    case SampleType.Min:
                        double min = double.MaxValue;
                        for (int i = index; i > index - rate; i--)
                        {
                            var value = showData[i].GetData(1, inverse);
                            if (value < min)
                                min = value;
                        }
                        return min;

                    case SampleType.Peak:
                        max = double.MinValue;
                        min = double.MaxValue;
                        total = 0;
                        for (int i = index; i > index - rate; i--)
                        {
                            var value = showData[i].GetData(1, inverse);
                            total += value;
                            if (value < min)
                                min = value;
                            if (value > max)
                                max = value;
                        }
                        var average = total / rate;
                        if (average >= totalAverage)
                            return max;
                        else
                            return min;
                }
                return showData[index].GetData(1, inverse);
            }

            if (rate <= 1 || index == minCount)
            {
                if (checkDataChanging && showData[index].IsDataChanged())
                    dataChanging = true;

                return showData[index].GetCurrData(1, dataAddDuration, dataChangeDuration, inverse, minValue, maxValue, unscaledTime);
            }
            switch (sampleType)
            {
                case SampleType.Sum:
                case SampleType.Average:
                    double total = 0;
                    var count = 0;
                    for (int i = index; i > index - rate; i--)
                    {
                        count++;
                        total += showData[i].GetCurrData(1, dataAddDuration, dataChangeDuration, inverse, minValue, maxValue, unscaledTime);
                        if (checkDataChanging && showData[i].IsDataChanged())
                            dataChanging = true;
                    }
                    if (sampleType == SampleType.Average)
                        return total / rate;
                    else
                        return total;

                case SampleType.Max:
                    double max = double.MinValue;
                    for (int i = index; i > index - rate; i--)
                    {
                        var value = showData[i].GetCurrData(1, dataAddDuration, dataChangeDuration, inverse, minValue, maxValue, unscaledTime);
                        if (value > max)
                            max = value;

                        if (checkDataChanging && showData[i].IsDataChanged())
                            dataChanging = true;
                    }
                    return max;

                case SampleType.Min:
                    double min = double.MaxValue;
                    for (int i = index; i > index - rate; i--)
                    {
                        var value = showData[i].GetCurrData(1, dataAddDuration, dataChangeDuration, inverse, minValue, maxValue, unscaledTime);
                        if (value < min)
                            min = value;

                        if (checkDataChanging && showData[i].IsDataChanged())
                            dataChanging = true;
                    }
                    return min;

                case SampleType.Peak:
                    max = double.MinValue;
                    min = double.MaxValue;
                    total = 0;
                    for (int i = index; i > index - rate; i--)
                    {
                        var value = showData[i].GetCurrData(1, dataAddDuration, dataChangeDuration, inverse, minValue, maxValue, unscaledTime);
                        total += value;
                        if (value < min)
                            min = value;
                        if (value > max)
                            max = value;

                        if (checkDataChanging && showData[i].IsDataChanged())
                            dataChanging = true;
                    }
                    var average = total / rate;
                    if (average >= totalAverage)
                        return max;
                    else
                        return min;
            }
            if (checkDataChanging && showData[index].IsDataChanged())
                dataChanging = true;

            return showData[index].GetCurrData(1, dataAddDuration, dataChangeDuration, inverse, minValue, maxValue, unscaledTime);
        }
    }
}