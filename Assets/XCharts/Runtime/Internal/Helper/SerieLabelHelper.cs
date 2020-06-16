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

        public static bool CanShowLabel(Serie serie, SerieData serieData, SerieLabel label, int dimesion)
        {
            return serie.show && serieData.canShowLabel && !serie.IsIgnoreValue(serieData.GetData(dimesion));
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
                var percent = dataValue == 0 && dataTotal == 0 ? 0 : dataValue / dataTotal * 100;
                content = content.Replace("{d}", ChartCached.NumberToStr(percent, numericFormatter));
                content = content.Replace("{d:f0}", ChartCached.IntToStr((int)Mathf.Round(percent)));
                content = content.Replace("{d:f1}", ChartCached.FloatToStr(percent, string.Empty, 1));
                content = content.Replace("{d:f2}", ChartCached.FloatToStr(percent, string.Empty, 2));
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

        public static void UpdatePieLabelPosition(Serie serie, SerieData serieData)
        {
            if (serieData.labelObject == null) return;
            var currAngle = serieData.runtimePieHalfAngle;
            var currRad = currAngle * Mathf.Deg2Rad;
            var offsetRadius = serieData.runtimePieOffsetRadius;
            var insideRadius = serieData.runtimePieInsideRadius;
            var outsideRadius = serieData.runtimePieOutsideRadius;
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            switch (serieLabel.position)
            {
                case SerieLabel.Position.Center:
                    serieData.labelPosition = serie.runtimeCenterPos;
                    break;
                case SerieLabel.Position.Inside:
                    var labelRadius = offsetRadius + insideRadius + (outsideRadius - insideRadius) / 2;
                    var labelCenter = new Vector2(serie.runtimeCenterPos.x + labelRadius * Mathf.Sin(currRad),
                        serie.runtimeCenterPos.y + labelRadius * Mathf.Cos(currRad));
                    serieData.labelPosition = labelCenter;
                    break;
                case SerieLabel.Position.Outside:
                    if (serieLabel.lineType == SerieLabel.LineType.HorizontalLine)
                    {
                        var radius1 = serie.runtimeOutsideRadius;
                        var radius3 = insideRadius + (outsideRadius - insideRadius) / 2;
                        var currSin = Mathf.Sin(currRad);
                        var currCos = Mathf.Cos(currRad);
                        var pos0 = new Vector3(serie.runtimeCenterPos.x + radius3 * currSin, serie.runtimeCenterPos.y + radius3 * currCos);
                        if (currAngle > 180)
                        {
                            currSin = Mathf.Sin((360 - currAngle) * Mathf.Deg2Rad);
                            currCos = Mathf.Cos((360 - currAngle) * Mathf.Deg2Rad);
                        }
                        var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos * radius3, 2)) - currSin * radius3;
                        r4 += serieLabel.lineLength1 + serieLabel.lineWidth * 4;
                        r4 += serieData.labelObject.label.preferredWidth / 2;
                        serieData.labelPosition = pos0 + (currAngle > 180 ? Vector3.left : Vector3.right) * r4;
                    }
                    else
                    {
                        labelRadius = serie.runtimeOutsideRadius + serieLabel.lineLength1;
                        labelCenter = new Vector2(serie.runtimeCenterPos.x + labelRadius * Mathf.Sin(currRad),
                            serie.runtimeCenterPos.y + labelRadius * Mathf.Cos(currRad));
                        float labelWidth = serieData.labelObject.label.preferredWidth;
                        serieData.labelPosition = labelCenter;
                    }
                    break;
            }
        }

        internal static void AvoidLabelOverlap(Serie serie)
        {
            if (!serie.avoidLabelOverlap) return;
            var lastCheckPos = Vector3.zero;
            var data = serie.data;
            var splitCount = 0;
            for (int n = 0; n < data.Count; n++)
            {
                var serieData = data[n];
                if (serieData.labelPosition.x != 0 && serieData.labelPosition.x < serie.runtimeCenterPos.x)
                {
                    splitCount = n;
                    break;
                }
            }
            if (splitCount <= 0) return;
            for (int n = 0; n < splitCount; n++)
            {
                var serieData = data[n];
                CheckSerieDataLabel(serie, serieData, false, ref lastCheckPos);
            }
            lastCheckPos = Vector3.zero;
            for (int n = data.Count - 1; n >= splitCount; n--)
            {
                var serieData = data[n];
                CheckSerieDataLabel(serie, serieData, true, ref lastCheckPos);
            }
        }

        private static void CheckSerieDataLabel(Serie serie, SerieData serieData, bool isLeft, ref Vector3 lastCheckPos)
        {
            if (!serieData.canShowLabel)
            {
                serieData.SetLabelActive(false);
                return;
            }
            if (!serieData.show) return;
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            if (serieLabel.position != SerieLabel.Position.Outside) return;
            if (lastCheckPos == Vector3.zero)
            {
                lastCheckPos = serieData.labelPosition;
            }
            else if (serieData.labelPosition.x != 0)
            {
                float hig = serieLabel.fontSize;
                if (lastCheckPos.y - serieData.labelPosition.y < hig)
                {
                    var labelRadius = serie.runtimeOutsideRadius + serieLabel.lineLength1;
                    var y1 = lastCheckPos.y - hig;
                    var cy = serie.runtimeCenterPos.y;
                    var diff = Mathf.Abs(y1 - cy);
                    var diffX = labelRadius * labelRadius - diff * diff;
                    diffX = diffX <= 0 ? 0 : diffX;
                    var x1 = serie.runtimeCenterPos.x + Mathf.Sqrt(diffX) * (isLeft ? -1 : 1);
                    serieData.labelPosition = new Vector3(x1, y1);
                }
                lastCheckPos = serieData.labelPosition;
                serieData.labelObject.SetPosition(SerieLabelHelper.GetRealLabelPosition(serieData, serieLabel));
            }
        }

        internal static Vector3 GetRealLabelPosition(SerieData serieData, SerieLabel label)
        {
            if (label.position == SerieLabel.Position.Outside && label.lineType != SerieLabel.LineType.HorizontalLine)
            {
                var currAngle = serieData.runtimePieHalfAngle;
                var offset = label.lineLength2 + serieData.labelObject.GetLabelWidth() / 2;
                if (currAngle > 180)
                    return serieData.labelPosition + new Vector3(-offset, 0, 0);
                else
                    return serieData.labelPosition + new Vector3(offset, 0, 0);
            }
            else
            {
                return serieData.labelPosition;
            }
        }
    }
}