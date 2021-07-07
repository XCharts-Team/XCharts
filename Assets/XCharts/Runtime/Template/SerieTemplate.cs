
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public static class SerieTemplate
    {
        public static void AddDefaultSerie(BaseChart chart, SerieType serieType, string serieName)
        {
            switch (serieType)
            {
                case SerieType.Line: AddDefaultLineSerie(chart, serieName); break;
                case SerieType.Bar: AddDefaultBarSerie(chart, serieName); break;
                case SerieType.Pie: AddDefaultPieSerie(chart, serieName); break;
                case SerieType.Radar: AddDefaultRadarSerie(chart, serieName); break;
                case SerieType.Scatter: AddDefaultScatterSerie(chart, serieName); break;
                case SerieType.EffectScatter: AddDefaultEffectScatterSerie(chart, serieName); break;
                case SerieType.Heatmap: AddDefaultHeatmapSerie(chart, serieName); break;
                case SerieType.Liquid: AddDefaultLiquidSerie(chart, serieName); break;
                case SerieType.Gauge: AddDefaultGaugeSerie(chart, serieName); break;
                case SerieType.Ring: AddDefaultRingSerie(chart, serieName); break;
                case SerieType.Candlestick: AddDefaultCandlestickSerie(chart, serieName); break;
                case SerieType.Custom: chart.AddDefaultCustomSerie(serieName); break;
                default: Debug.LogError("AddDefaultSerie: not support serieType yet:" + serieType); break;
            }
        }

        public static void AddDefaultLineSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie(SerieType.Line, serieName);
            serie.symbol.show = true;
            for (int i = 0; i < 5; i++)
            {
                chart.AddData(serie.index, UnityEngine.Random.Range(10, 90));
            }
        }

        public static void AddDefaultBarSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie(SerieType.Bar, serieName);
            for (int i = 0; i < 5; i++)
            {
                chart.AddData(serie.index, UnityEngine.Random.Range(10, 90));
            }
        }
        public static void AddDefaultPieSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie(SerieType.Pie, serieName);
            chart.AddData(serie.index, 70, "pie1");
            chart.AddData(serie.index, 20, "pie2");
            chart.AddData(serie.index, 10, "pie3");
        }

        public static void AddDefaultScatterSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie(SerieType.Scatter, serieName);
            serie.symbol.show = true;
            serie.symbol.type = SerieSymbolType.Circle;
            serie.itemStyle.opacity = 0.8f;
            serie.clip = false;
            for (int i = 0; i < 10; i++)
            {
                chart.AddData(serie.index, Random.Range(10, 100), Random.Range(10, 100));
            }
        }

        public static void AddDefaultEffectScatterSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie(SerieType.EffectScatter, serieName);
            serie.symbol.show = true;
            serie.symbol.type = SerieSymbolType.Circle;
            serie.itemStyle.opacity = 0.8f;
            serie.clip = false;
            for (int i = 0; i < 10; i++)
            {
                chart.AddData(serie.index, Random.Range(10, 100), Random.Range(10, 100));
            }
        }

        public static void AddDefaultHeatmapSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie(SerieType.Heatmap, serieName);
            serie.itemStyle.show = true;
            serie.itemStyle.borderWidth = 1;
            serie.itemStyle.borderColor = Color.clear;
            serie.emphasis.show = true;
            serie.emphasis.itemStyle.show = true;
            serie.emphasis.itemStyle.borderWidth = 1;
            serie.emphasis.itemStyle.borderColor = Color.black;
        }

        public static void AddDefaultLiquidSerie(BaseChart chart, string serieName)
        {
            if (chart.vessels.Count == 0)
            {
                chart.AddVessel(Vessel.defaultVessel);
            }
            var serie = chart.AddSerie(SerieType.Liquid, serieName);
            serie.min = 0;
            serie.max = 100;
            serie.label.show = true;
            serie.label.textStyle.fontSize = 40;
            serie.label.formatter = "{d}%";
            serie.label.textStyle.color = new Color32(70, 70, 240, 255);
            chart.AddData(serie.index, UnityEngine.Random.Range(0, 100));
        }

        public static void AddDefaultRadarSerie(BaseChart chart, string serieName)
        {
            if (chart.radars.Count == 0)
            {
                chart.AddRadar(Radar.defaultRadar);
            }
            var serie = chart.AddSerie(SerieType.Radar, serieName);
            serie.symbol.show = true;
            serie.symbol.type = SerieSymbolType.EmptyCircle;
            serie.symbol.size = 4;
            serie.symbol.selectedSize = 6;
            serie.showDataName = true;
            List<double> data = new List<double>();
            for (int i = 0; i < 5; i++)
            {
                data.Add(Random.Range(20, 90));
            }
            chart.AddData(serie.index, data, "legendName");
        }

        public static void AddDefaultGaugeSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie(SerieType.Gauge, serieName);
            serie.min = 0;
            serie.max = 100;
            serie.startAngle = -125;
            serie.endAngle = 125;
            serie.center[0] = 0.5f;
            serie.center[1] = 0.5f;
            serie.radius[0] = 80;
            serie.splitNumber = 5;
            serie.animation.dataChangeEnable = true;
            serie.titleStyle.show = true;
            serie.titleStyle.textStyle.offset = new Vector2(0, 20);
            serie.label.show = true;
            serie.label.offset = new Vector3(0, -30);
            serie.itemStyle.show = true;
            serie.gaugeAxis.axisLabel.show = true;
            serie.gaugeAxis.axisLabel.margin = 18;
            chart.AddData(serie.index, UnityEngine.Random.Range(10, 90), "title");
        }

        public static void AddDefaultRingSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie(SerieType.Ring, serieName);
            serie.roundCap = true;
            serie.radius = new float[] { 0.3f, 0.35f };
            serie.titleStyle.show = false;
            serie.titleStyle.textStyle.offset = new Vector2(0, 30);
            serie.label.show = true;
            serie.label.position = SerieLabel.Position.Center;
            serie.label.formatter = "{d:f0}%";
            serie.label.textStyle.fontSize = 28;
            var value = Random.Range(30, 90);
            var max = 100;
            chart.AddData(serie.index, value, max, "data1");
        }
        public static int AddDefaultCandlestickSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie(SerieType.Candlestick, serieName);
            var defaultDataCount = 5;
            for (int i = 0; i < defaultDataCount; i++)
            {
                var open = Random.Range(20, 60);
                var close = Random.Range(40, 90);
                var lowest = Random.Range(0, 50);
                var heighest = Random.Range(50, 100);
                chart.AddData(serie.index, open, close, lowest, heighest);
            }
            return defaultDataCount;
        }
    }
}