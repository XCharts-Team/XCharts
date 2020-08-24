using System;
/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// 升级旧版本的颜色配置到1.6.0以上版本,参考[问答29](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts问答.md)进行升级
    /// 导出：菜单栏->XCharts->ExportColorConfig
    /// 导入：菜单栏->XCharts->ImportColorConfig
    /// </summary>
    public static class UpgradeChartColor
    {
        private const string CONFIG_PATH = "/colors.config"; // /Assets/

        [MenuItem("XCharts/ExportColorConfig")]
        /// <summary>
        /// Export all the color configuration associated with drawing.
        /// 导出所有图表的和绘制相关的颜色配置。保存在Assets目录下的colors.config文件里。
        /// </summary>
        public static void ExportColorConfig()
        {
            Debug.Log("ExportColorConfig");
            var charts = Resources.FindObjectsOfTypeAll(typeof(BaseChart));
            var sb = new StringBuilder();
            foreach (var chart in charts)
            {
                if (chart is CoordinateChart) ExportCoordinateChart(sb, chart as CoordinateChart);
                else if (chart is LiquidChart) ExportLiquidChart(sb, chart as LiquidChart); //如果这里编译失败，说明该版本不存在LiquidChart，可以整行注释掉。
                else if (chart is RadarChart) ExportRadarChart(sb, chart as RadarChart);
                else ExportSeries(sb, chart as BaseChart);
                sb.Append("\n");
            }
            Debug.LogFormat("ExportColorConfig DONE: {0} charts.", charts.Length);
            File.WriteAllText(Application.dataPath + CONFIG_PATH, sb.ToString());
        }

        [MenuItem("XCharts/ImportColorConfig")]
        /// <summary>
        /// 导入旧版本的颜色配置。
        /// </summary>
        public static void ImportColorConfig()
        {
            Debug.Log("ImportColorConfig");
            var configPath = Application.dataPath + CONFIG_PATH;
            if (!File.Exists(configPath))
            {
                Debug.LogError("ImportColorConfig ERROR:can't found config:" + configPath);
                return;
            }
            var charts = Resources.FindObjectsOfTypeAll(typeof(BaseChart));
            var chartDic = new Dictionary<int, BaseChart>();
            foreach (var chart in charts)
            {
                chartDic[chart.GetInstanceID()] = chart as BaseChart;
            }
            var allLines = File.ReadAllLines(configPath);
            var chartSet = new HashSet<int>();
            foreach (var line in allLines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var temp = line.Split('=');
                var instanceId = int.Parse(temp[0]);
                if (!chartDic.ContainsKey(instanceId))
                {
                    Debug.LogError("can't find chart:" + instanceId);
                    continue;
                }
                if (!chartSet.Contains(instanceId)) chartSet.Add(instanceId);
                var chart = chartDic[instanceId];
                var colorList = ConvertToColorList(temp[2]);
                var temp2 = temp[1].Split('_');
                var allType = temp[1];
                var strType = temp2[0];
                if (allType.Equals("visualMap")) ImportColorList((chart as CoordinateChart).visualMap.inRange, colorList);
                else if (allType.Equals("xAxis0_splitArea")) ImportColorList((chart as CoordinateChart).xAxis0.splitArea.color, colorList);
                else if (allType.Equals("xAxis1_splitArea")) ImportColorList((chart as CoordinateChart).xAxis1.splitArea.color, colorList);
                else if (allType.Equals("yAxis0_splitArea")) ImportColorList((chart as CoordinateChart).yAxis0.splitArea.color, colorList);
                else if (allType.Equals("yAxis1_splitArea")) ImportColorList((chart as CoordinateChart).yAxis1.splitArea.color, colorList);
                else if (allType.Equals("xAxis0_splitLine")) (chart as CoordinateChart).xAxis0.splitLine.lineStyle.color = colorList[0];
                else if (allType.Equals("xAxis1_splitLine")) (chart as CoordinateChart).xAxis1.splitLine.lineStyle.color = colorList[0];
                else if (allType.Equals("yAxis0_splitLine")) (chart as CoordinateChart).yAxis0.splitLine.lineStyle.color = colorList[0];
                else if (allType.Equals("yAxis1_splitLine")) (chart as CoordinateChart).yAxis1.splitLine.lineStyle.color = colorList[0];
                else if (strType.Equals("vessel")) ImportVesselColor(chart, int.Parse(temp2[1]), colorList); //没有LiquidChart的版本该行可以注释掉。
                else if (strType.Equals("radarSplitLine")) ImportRadarSplitLineColor(chart, int.Parse(temp2[1]), colorList);
                else if (strType.Equals("radarSplitArea")) ImportRadarSplitAreaColor(chart, int.Parse(temp2[1]), colorList);
                else if (strType.Equals("serie"))
                {
                    var index = int.Parse(temp2[1]);
                    var strSubType = temp2[2];
                    var serie = chart.series.GetSerie(index);
                    if (strSubType.Equals("lineStyle")) serie.lineStyle.color = colorList[0];
                    else if (strSubType.Equals("areaStyle")) ImportSerieAreaColor(serie.areaStyle, colorList);
                    else if (strSubType.Equals("label")) ImportLabelColor(serie.label, colorList);
                    else if (strSubType.Equals("labelEmphasis")) ImportLabelColor(serie.emphasis.label, colorList);
                    else if (strSubType.Equals("itemStyle")) ImportItemStyleColor(serie.itemStyle, colorList);
                    else if (strSubType.Equals("itemStyleEmphasis")) ImportItemStyleColor(serie.emphasis.itemStyle, colorList);
                    else if (strSubType.Equals("gaugeAxisLine")) ImportGaugeAxisLineColor(serie.gaugeAxis, colorList);
                    else if (strSubType.Equals("gaugeSplitLine")) serie.gaugeAxis.splitLine.lineStyle.color = colorList[0];
                    else if (strSubType.Equals("gaugeAxisTick")) serie.gaugeAxis.axisTick.lineStyle.color = colorList[0];
                }
                else if (strType.Equals("serieData"))
                {
                    var index = int.Parse(temp2[1]);
                    var dataIndex = int.Parse(temp2[2]);
                    var strSubType = temp2[3];
                    var serieData = chart.series.GetSerie(index).GetSerieData(dataIndex);
                    if (strSubType.Equals("label")) ImportLabelColor(serieData.label, colorList);
                    else if (strSubType.Equals("labelEmphasis")) ImportLabelColor(serieData.emphasis.label, colorList);
                    else if (strSubType.Equals("itemStyle")) ImportItemStyleColor(serieData.itemStyle, colorList);
                    else if (strSubType.Equals("itemStyleEmphasis")) ImportItemStyleColor(serieData.emphasis.itemStyle, colorList);
                }
                chart.RefreshChart();
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.LogFormat("ImportColorConfig DONE: {0} charts.", chartSet.Count);
        }

        private static void ExportCoordinateChart(StringBuilder sb, CoordinateChart chart)
        {
            var instanceId = chart.GetInstanceID();
            if (chart.visualMap.show)
            {
                AppendColor(sb, instanceId, "visualMap", chart.visualMap.inRange);
            }
            if (chart.xAxis0.show)
            {
                if (chart.xAxis0.splitArea.show)
                    AppendColor(sb, instanceId, "xAxis0_splitArea", chart.xAxis0.splitArea.color);
                if (chart.xAxis0.splitLine.show)
                    AppendColor(sb, instanceId, "xAxis0_splitLine", chart.xAxis0.splitLine.lineStyle.color);
            }
            if (chart.xAxis1.show)
            {
                if (chart.xAxis1.splitArea.show)
                    AppendColor(sb, instanceId, "xAxis1_splitArea", chart.xAxis1.splitArea.color);
                if (chart.xAxis1.splitLine.show)
                    AppendColor(sb, instanceId, "xAxis1_splitLine", chart.xAxis1.splitLine.lineStyle.color);
            }
            if (chart.yAxis0.show)
            {
                if (chart.yAxis0.splitArea.show)
                    AppendColor(sb, instanceId, "yAxis0_splitArea", chart.yAxis0.splitArea.color);
                if (chart.yAxis0.splitLine.show)
                    AppendColor(sb, instanceId, "yAxis0_splitLine", chart.yAxis0.splitLine.lineStyle.color);
            }
            if (chart.yAxis1.show)
            {
                if (chart.yAxis1.splitArea.show)
                    AppendColor(sb, instanceId, "yAxis1_splitArea", chart.yAxis1.splitArea.color);
                if (chart.yAxis1.splitLine.show)
                    AppendColor(sb, instanceId, "yAxis1_splitLine", chart.yAxis1.splitLine.lineStyle.color);
            }
            ExportSeries(sb, chart);
        }

        /// <summary>
        /// LiquidChart不在该版本时整个函数可以注释掉
        /// </summary>
        private static void ExportLiquidChart(StringBuilder sb, LiquidChart chart)
        {
            var instanceId = chart.GetInstanceID();
            var key = "vessel_";
            for (int i = 0; i < chart.vessels.Count; i++)
            {
                var vessel = chart.vessels[i];
                AppendColor(sb, instanceId, key + i, vessel.color, vessel.backgroundColor);
            }
            ExportSeries(sb, chart);
        }

        private static void ExportRadarChart(StringBuilder sb, RadarChart chart)
        {
            var instanceId = chart.GetInstanceID();
            for (int i = 0; i < chart.radars.Count; i++)
            {
                var radar = chart.radars[i];
                AppendColor(sb, instanceId, "radarSplitLine_" + i, radar.splitLine.lineStyle.color);
                AppendColor(sb, instanceId, "radarSplitArea_" + i, radar.splitArea.color);
            }
            ExportSeries(sb, chart);
        }

        private static void ExportSeries(StringBuilder sb, BaseChart chart)
        {
            var instanceId = chart.GetInstanceID();
            for (int i = 0; i < chart.series.list.Count; i++)
            {
                var serie = chart.series.GetSerie(i);
                var key = "serie_" + i;
                AppendColor(sb, instanceId, key + "_itemStyle", serie.itemStyle.color, serie.itemStyle.toColor, serie.itemStyle.toColor2, serie.itemStyle.backgroundColor, serie.itemStyle.borderColor);
                AppendColor(sb, instanceId, key + "_label", serie.label.backgroundColor, serie.label.borderColor);
                AppendColor(sb, instanceId, key + "_itemStyleEmphasis", serie.emphasis.itemStyle.color, serie.emphasis.itemStyle.toColor, serie.emphasis.itemStyle.toColor2, serie.emphasis.itemStyle.backgroundColor, serie.emphasis.itemStyle.borderColor);
                AppendColor(sb, instanceId, key + "_labelEmphasis", serie.emphasis.label.backgroundColor, serie.emphasis.label.borderColor);
                if (serie.type == SerieType.Line)
                {
                    AppendColor(sb, instanceId, key + "_lineStyle", serie.lineStyle.color);
                    AppendColor(sb, instanceId, key + "_areaStyle", serie.areaStyle.color, serie.areaStyle.toColor, serie.areaStyle.highlightColor, serie.areaStyle.highlightToColor);
                }
                if (serie.type == SerieType.Gauge)
                {
                    var axisLineColors = new List<Color>();
                    axisLineColors.Add(serie.gaugeAxis.axisLine.barColor);
                    axisLineColors.Add(serie.gaugeAxis.axisLine.barBackgroundColor);
                    for (int n = 0; n < serie.gaugeAxis.axisLine.stageColor.Count; n++)
                    {
                        axisLineColors.Add(serie.gaugeAxis.axisLine.stageColor[n].color);
                    }
                    AppendColor(sb, instanceId, key + "_gaugeAxisLine", axisLineColors);
                    AppendColor(sb, instanceId, key + "_gaugeSplitLine", serie.gaugeAxis.splitLine.lineStyle.color);
                    AppendColor(sb, instanceId, key + "_gaugeAxisTick", serie.gaugeAxis.axisTick.lineStyle.color);
                }
                for (int j = 0; j < serie.dataCount; j++)
                {
                    key = string.Format("serieData_{0}_{1}", i, j);
                    var serieData = serie.GetSerieData(j);
                    if (serieData.enableItemStyle)
                        AppendColor(sb, instanceId, key + "_itemStyle", serieData.itemStyle.color, serieData.itemStyle.toColor, serieData.itemStyle.toColor2, serieData.itemStyle.backgroundColor, serieData.itemStyle.borderColor);
                    if (serieData.enableLabel)
                        AppendColor(sb, instanceId, key + "_label", serieData.label.backgroundColor, serieData.label.borderColor);
                    if (serieData.enableEmphasis)
                        AppendColor(sb, instanceId, key + "_itemStyleEmphasis", serieData.emphasis.itemStyle.color, serieData.emphasis.itemStyle.toColor, serieData.emphasis.itemStyle.toColor2, serieData.emphasis.itemStyle.backgroundColor, serieData.emphasis.itemStyle.borderColor);
                    if (serieData.enableEmphasis)
                        AppendColor(sb, instanceId, key + "_labelEmphasis", serieData.emphasis.label.backgroundColor, serieData.emphasis.label.borderColor);
                }
            }
        }

        private static void ExportGague(GaugeAxis gauge, StringBuilder sb)
        {

        }

        private static void AppendColor(StringBuilder sb, int instanceId, string key, List<Color> list)
        {
            if (list.Count <= 0) return;
            sb.AppendFormat("{0}={1}=", instanceId, key);
            for (int i = 0; i < list.Count; i++)
            {
                sb.AppendFormat("{0}|", GetColorRGBA(list[i]));
            }
            sb.Append("\n");
        }

        private static void AppendColor(StringBuilder sb, int instanceId, string key, params Color[] color)
        {
            sb.AppendFormat("{0}={1}=", instanceId, key);
            for (int i = 0; i < color.Length; i++)
            {
                sb.AppendFormat("{0}|", GetColorRGBA(color[i]));
            }
            sb.Append("\n");
        }

        private static void AppendColor(StringBuilder sb, int instanceId, string key, List<Color32> list)
        {
            if (list.Count <= 0) return;
            sb.AppendFormat("{0}={1}=", instanceId, key);
            for (int i = 0; i < list.Count; i++)
            {
                sb.AppendFormat("{0}|", GetColorRGBA(list[i]));
            }
            sb.Append("\n");
        }

        private static string GetColorRGBA(Color color)
        {
            Color32 color32 = color;
            return string.Format("{0},{1},{2},{3}", color32.r, color32.g, color32.b, color32.a);
        }

        private static List<Color32> ConvertToColorList(string strInfo)
        {
            var temp = strInfo.Split('|');
            var list = new List<Color32>();
            for (int i = 0; i < temp.Length; i++)
            {
                if (!string.IsNullOrEmpty(temp[i]))
                {
                    var temp2 = temp[i].Split(',');
                    var r = byte.Parse(temp2[0]);
                    var g = byte.Parse(temp2[1]);
                    var b = byte.Parse(temp2[2]);
                    var a = byte.Parse(temp2[3]);
                    list.Add(new Color32(r, g, b, a));
                }
            }
            return list;
        }

        private static void ImportColorList(List<Color32> target, List<Color32> colorList)
        {
            target.Clear();
            for (int i = 0; i < colorList.Count; i++)
            {
                target.Add(colorList[i]);
            }
        }

        private static void ImportColorList(List<Color> target, List<Color32> colorList)
        {
            target.Clear();
            for (int i = 0; i < colorList.Count; i++)
            {
                target.Add(colorList[i]);
            }
        }

        private static void ImportSerieAreaColor(AreaStyle areaStyle, List<Color32> colorList)
        {
            areaStyle.color = colorList[0];
            areaStyle.toColor = colorList[1];
            areaStyle.highlightColor = colorList[2];
            areaStyle.highlightToColor = colorList[3];
        }

        private static void ImportLabelColor(SerieLabel label, List<Color32> colorList)
        {
            label.backgroundColor = colorList[0];
            label.borderColor = colorList[1];
        }

        private static void ImportItemStyleColor(ItemStyle itemStyle, List<Color32> colorList)
        {
            itemStyle.color = colorList[0];
            itemStyle.toColor = colorList[1];
            itemStyle.toColor2 = colorList[2];//没有toColor2的版本可以注释掉改行
            itemStyle.backgroundColor = colorList[3];
            itemStyle.borderColor = colorList[4];
        }

        private static void ImportGaugeAxisLineColor(GaugeAxis gauge, List<Color32> colorList)
        {
            gauge.axisLine.barColor = colorList[0];
            gauge.axisLine.barBackgroundColor = colorList[1];
            for (int i = 2; i < colorList.Count; i++)
            {
                gauge.axisLine.stageColor[i - 2].color = colorList[i];
            }
        }

        /// <summary>
        /// 没有LiquidChart的版本这个函数可以注释掉
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="index"></param>
        /// <param name="colorList"></param>
        private static void ImportVesselColor(BaseChart chart, int index, List<Color32> colorList)
        {
            var vessel = (chart as LiquidChart).GetVessel(index);
            vessel.color = colorList[0];
            vessel.backgroundColor = colorList[1];
        }

        private static void ImportRadarSplitLineColor(BaseChart chart, int index, List<Color32> colorList)
        {
            var radar = (chart as RadarChart).GetRadar(index);
            radar.splitLine.lineStyle.color = colorList[0];
        }

        private static void ImportRadarSplitAreaColor(BaseChart chart, int index, List<Color32> colorList)
        {
            var radar = (chart as RadarChart).GetRadar(index);
            ImportColorList(radar.splitArea.color, colorList);
        }
    }
}