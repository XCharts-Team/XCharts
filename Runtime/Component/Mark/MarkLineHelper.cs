using UnityEngine;

namespace XCharts.Runtime
{
    internal static class MarkLineHelper
    {
        public static string GetFormatterContent(Serie serie, MarkLineData data)
        {
            var serieLabel = data.label;
            var numericFormatter = serieLabel.numericFormatter;
            if (string.IsNullOrEmpty(serieLabel.formatter))
            {
                var content = ChartCached.NumberToStr(data.runtimeValue, numericFormatter);
                return serieLabel.formatterFunction == null? content:
                    serieLabel.formatterFunction(data.index, data.runtimeValue, null, content);
            }
            else
            {
                var content = serieLabel.formatter;
                FormatterHelper.ReplaceSerieLabelContent(ref content, numericFormatter, serie.dataCount, data.runtimeValue,
                    0, serie.serieName, data.name, data.name, Color.clear, null);
                return serieLabel.formatterFunction == null? content:
                    serieLabel.formatterFunction(data.index, data.runtimeValue, null, content);
            }
        }

        public static Vector3 GetLabelPosition(MarkLineData data)
        {
            if (!data.label.show) return Vector3.zero;
            var dir = (data.runtimeEndPosition - data.runtimeStartPosition).normalized;
            var horizontal = Mathf.Abs(Vector3.Dot(dir, Vector3.right)) == 1;
            var labelWidth = data.runtimeLabel == null ? 50 : data.runtimeLabel.GetTextWidth();
            var labelHeight = data.runtimeLabel == null ? 20 : data.runtimeLabel.GetTextHeight();
            switch (data.label.position)
            {
                case LabelStyle.Position.Start:
                    if (data.runtimeStartPosition == Vector3.zero) return Vector3.zero;
                    if (horizontal) return data.runtimeStartPosition + data.label.offset + labelWidth / 2 * Vector3.left;
                    else return data.runtimeStartPosition + data.label.offset + labelHeight / 2 * Vector3.down;
                case LabelStyle.Position.Middle:
                    if (data.runtimeCurrentEndPosition == Vector3.zero) return Vector3.zero;
                    var center = (data.runtimeStartPosition + data.runtimeCurrentEndPosition) / 2;
                    if (horizontal) return center + data.label.offset + labelHeight / 2 * Vector3.up;
                    else return center + data.label.offset + labelWidth / 2 * Vector3.right;
                default:
                    if (data.runtimeCurrentEndPosition == Vector3.zero) return Vector3.zero;
                    if (horizontal) return data.runtimeCurrentEndPosition + data.label.offset + labelWidth / 2 * Vector3.right;
                    else return data.runtimeCurrentEndPosition + data.label.offset + labelHeight / 2 * Vector3.up;
            }
        }
    }
}