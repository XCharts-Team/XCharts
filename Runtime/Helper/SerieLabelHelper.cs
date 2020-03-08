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
                            if (serieData.IsInitLabel())
                            {
                                serieData.SetLabelActive(true);
                                m_UpdateLabelText = true;
                            }
                            else
                            {
                                m_ReinitLabel = true;
                            }
                        }
                        else
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
            if (serie.label.color != Color.clear)
            {
                return serie.label.color;
            }
            else
            {
                return themeInfo.GetColor(index);
            }
        }

        private static void SetGaugeLabelText(Serie serie)
        {
            var serieData = serie.GetSerieData(0);
            if (serieData != null)
            {
                if (serieData.IsInitLabel())
                {
                    var value = serieData.GetData(1);
                    var total = serie.max;
                    var content = serie.label.GetFormatterContent(serie.name, serieData.name, value, total);
                    serieData.SetLabelText(content);
                    serieData.SetLabelPosition(serie.runtimeCenterPos + serie.label.offset);
                    if (serie.label.color != Color.clear)
                    {
                        serieData.SetLabelColor(serie.label.color);
                    }
                }
            }
        }

        private static void SetRingLabelText(Serie serie, ThemeInfo themeInfo)
        {
            for (int i = 0; i < serie.dataCount; i++)
            {
                var serieData = serie.data[i];
                if (serieData.IsInitLabel())
                {
                    if (!serie.show || !serieData.show)
                    {
                        serieData.SetLabelActive(false);
                        continue;
                    }
                    var value = serieData.GetData(0);
                    var total = serieData.GetData(1);
                    var content = serie.label.GetFormatterContent(serie.name, serieData.name, value, total);
                    serieData.SetLabelActive(true);
                    serieData.SetLabelText(content);
                    serieData.SetLabelColor(GetLabelColor(serie, themeInfo, i));

                    if (serie.label.position == SerieLabel.Position.Bottom)
                    {
                        var labelWidth = serieData.GetLabelWidth();
                        if (serie.clockwise)
                            serieData.SetLabelPosition(serieData.labelPosition - new Vector3(labelWidth / 2, 0));
                        else
                            serieData.SetLabelPosition(serieData.labelPosition + new Vector3(labelWidth / 2, 0));
                    }
                    else
                    {
                        serieData.SetLabelPosition(serieData.labelPosition);
                    }
                }
            }
        }
    }
}