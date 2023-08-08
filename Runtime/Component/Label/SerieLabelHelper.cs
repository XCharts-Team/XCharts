using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class SerieLabelHelper
    {

        public static Color GetLabelColor(Serie serie, ThemeStyle theme, int index)
        {
            if (serie.label != null && !ChartHelper.IsClearColor(serie.label.textStyle.color))
            {
                return serie.label.textStyle.color;
            }
            else
            {
                return theme.GetColor(index);
            }
        }

        public static bool CanShowLabel(Serie serie, SerieData serieData, LabelStyle label, int dimesion)
        {
            return serie.show && serieData.context.canShowLabel && !serie.IsIgnoreValue(serieData, dimesion);
        }

        public static string GetFormatterContent(Serie serie, SerieData serieData,
            double dataValue, double dataTotal, LabelStyle serieLabel, Color color)
        {
            if (serieLabel == null)
            {
                serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            }
            var numericFormatter = serieLabel == null ? "" : serieLabel.numericFormatter;
            var serieName = serie.serieName;
            var dataName = serieData != null ? serieData.name : null;
            if (string.IsNullOrEmpty(serieLabel.formatter))
            {
                var currentContent = ChartCached.NumberToStr(dataValue, numericFormatter);
                if (serieLabel.formatterFunction == null)
                    return currentContent;
                else
                    return serieLabel.formatterFunction(serieData.index, dataValue, null, currentContent);
            }
            else
            {
                var content = serieLabel.formatter;
                FormatterHelper.ReplaceSerieLabelContent(ref content, numericFormatter, serie.dataCount, dataValue,
                    dataTotal, serieName, dataName, dataName, color, serieData);
                if (serieLabel.formatterFunction == null)
                    return content;
                else
                    return serieLabel.formatterFunction(serieData.index, dataValue, null, content);
            }
        }

        public static void SetGaugeLabelText(Serie serie)
        {
            var serieData = serie.GetSerieData(0);
            if (serieData == null) return;
            if (serieData.labelObject == null) return;
            var label = SerieHelper.GetSerieLabel(serie, serieData);
            if (label == null) return;
            var value = serieData.GetData(1);
            var total = serie.max;
            var content = SerieLabelHelper.GetFormatterContent(serie, serieData, value, total, null, Color.clear);
            serieData.labelObject.SetText(content);
            serieData.labelObject.SetPosition(serie.context.center + label.offset);
            if (!ChartHelper.IsClearColor(label.textStyle.color))
            {
                serieData.labelObject.text.SetColor(label.textStyle.color);
            }
        }

        
    }
}