/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using System.Reflection;
using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    [SerieHandler(typeof(BarHandler), true)]
    [SerieConvert(typeof(Line),typeof(Pie))]
    [RequireChartComponent(typeof(GridCoord))]
    [DefaultAnimation(AnimationType.BottomToTop)]
    public class Bar : Serie, INeedSerieContainer
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }

        public static void AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Bar>(serieName);
            for (int i = 0; i < 5; i++)
            {
                chart.AddData(serie.index, UnityEngine.Random.Range(10, 90));
            }
        }

        public static Bar CovertSerie(Serie serie)
        {
            var newSerie = SerieHelper.CloneSerie<Bar>(serie);
            return newSerie;
        }
    }
}