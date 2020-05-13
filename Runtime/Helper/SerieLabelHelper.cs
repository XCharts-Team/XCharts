/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    internal static class SerieLabelHelper
    {
        public static void CheckLabel(Serie serie, ref bool m_ReinitLabel, ref bool m_UpdateLabelText)
        {
            switch (serie.type)
            {
                case SerieType.Gauge:
                case SerieType.Ring:
                    var serieData = serie.GetSerieData(0);
                    if (serieData != null)
                    {
                        if (serie.label.show && serie.show)
                        {
                            if (serieData.labelObject != null)
                            {
                                serieData.SetLabelActive(true);
                                m_UpdateLabelText = true;
                            }
                            else
                            {
                                m_ReinitLabel = true;
                            }
                        }
                        else if (serieData.labelObject != null)
                        {
                            serieData.SetLabelActive(false);
                        }
                    }
                    break;
            }
        }

        public static void UpdateLabelText(Series series, ThemeInfo themeInfo)
        {
            foreach (var serie in series.list)
            {
                if (!serie.label.show) continue;
                switch (serie.type)
                {
                    case SerieType.Gauge:
                        SetGaugeLabelText(serie);
                        break;
                    case SerieType.Ring:
                        SetRingLabelText(serie, themeInfo);
                        break;
                }
            }
        }

        public static Color GetLabelColor(Serie serie, ThemeInfo themeInfo, int index)
        {
            if (!ChartHelper.IsClearColor(serie.label.color))
            {
                return serie.label.color;
            }
            else
            {
                return themeInfo.GetColor(index);
            }
        }

        public static void ResetLabel(SerieData serieData, SerieLabel label, ThemeInfo themeInfo, int colorIndex)
        {
            if (serieData.labelObject == null) return;
            if (serieData.labelObject.label == null) return;
            serieData.labelObject.label.color = !ChartHelper.IsClearColor(label.color) ? label.color :
                (Color)themeInfo.GetColor(colorIndex);
            serieData.labelObject.label.fontSize = label.fontSize;
            serieData.labelObject.label.fontStyle = label.fontStyle;
        }

        public static string GetFormatterContent(Serie serie, SerieData serieData,
            float dataValue, float dataTotal, SerieLabel serieLabel = null)
        {
            if (serieLabel == null)
            {
                serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            }
            var numericFormatter = GetLabelNumericFormatter(serie, serieData);
            var serieName = serie.name;
            var dataName = serieData != null ? serieData.name : null;
            if (string.IsNullOrEmpty(serieLabel.formatter))
                return ChartCached.NumberToStr(dataValue, numericFormatter);
            else
            {
                var content = serieLabel.formatter.Replace("{a}", serieName);
                content = content.Replace("{b}", dataName);
                content = content.Replace("{c}", ChartCached.NumberToStr(dataValue, numericFormatter));
                content = content.Replace("{c:f0}", ChartCached.IntToStr((int)Mathf.Round(dataValue)));
                content = content.Replace("{c:f1}", ChartCached.FloatToStr(dataValue, string.Empty, 1));
                content = content.Replace("{c:f2}", ChartCached.FloatToStr(dataValue, string.Empty, 2));
                if (dataTotal > 0)
                {
                    var percent = dataValue / dataTotal * 100;
                    content = content.Replace("{d}", ChartCached.NumberToStr(percent, numericFormatter));
                    content = content.Replace("{d:f0}", ChartCached.IntToStr((int)Mathf.Round(percent)));
                    content = content.Replace("{d:f1}", ChartCached.FloatToStr(percent, string.Empty, 1));
                    content = content.Replace("{d:f2}", ChartCached.FloatToStr(percent, string.Empty, 2));
                }
                content = content.Replace("\\n", "\n");
                content = content.Replace("<br/>", "\n");
                return content;
            }
        }

        private static string GetLabelNumericFormatter(Serie serie, SerieData serieData)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!string.IsNullOrEmpty(itemStyle.numericFormatter)) return itemStyle.numericFormatter;
            else return serie.label.numericFormatter;
        }


        private static void SetGaugeLabelText(Serie serie)
        {
            var serieData = serie.GetSerieData(0);
            if (serieData == null) return;
            if (serieData.labelObject == null) return;
            var value = serieData.GetData(1);
            var total = serie.max;
            var content = SerieLabelHelper.GetFormatterContent(serie, serieData, value, total);
            serieData.labelObject.SetText(content);
            serieData.labelObject.SetLabelPosition(serie.runtimeCenterPos + serie.label.offset);
            if (!ChartHelper.IsClearColor(serie.label.color))
            {
                serieData.labelObject.label.color = serie.label.color;
            }
        }

        private static void SetRingLabelText(Serie serie, ThemeInfo themeInfo)
        {
            for (int i = 0; i < serie.dataCount; i++)
            {
                var serieData = serie.data[i];
                var serieLabel = SerieHelper.GetSerieLabel(serie, serieData, serieData.highlighted);
                if (serieLabel.show && serieData.labelObject != null)
                {
                    if (!serie.show || !serieData.show)
                    {
                        serieData.SetLabelActive(false);
                        continue;
                    }
                    var value = serieData.GetData(0);
                    var total = serieData.GetData(1);
                    var content = SerieLabelHelper.GetFormatterContent(serie, serieData, value, total);
                    serieData.SetLabelActive(true);
                    serieData.labelObject.SetText(content);
                    serieData.labelObject.SetLabelColor(GetLabelColor(serie, themeInfo, i));

                    if (serie.label.position == SerieLabel.Position.Bottom)
                    {
                        var labelWidth = serieData.GetLabelWidth();
                        if (serie.clockwise)
                            serieData.labelObject.SetLabelPosition(serieData.labelPosition - new Vector3(labelWidth / 2, 0));
                        else
                            serieData.labelObject.SetLabelPosition(serieData.labelPosition + new Vector3(labelWidth / 2, 0));
                    }
                    else
                    {
                        serieData.labelObject.SetLabelPosition(serieData.labelPosition);
                    }
                }
            }
        }
    }
}