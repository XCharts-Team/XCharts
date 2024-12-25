using System;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class TooltipHelper
    {
        internal static void ResetTooltipParamsByItemFormatter(Tooltip tooltip, BaseChart chart)
        {
            if (!string.IsNullOrEmpty(tooltip.titleFormatter))
            {
                if (IsIgnoreFormatter(tooltip.titleFormatter))
                {
                    tooltip.context.data.title = string.Empty;
                }
                else
                {
                    tooltip.context.data.title = tooltip.titleFormatter;
                    FormatterHelper.ReplaceContent(ref tooltip.context.data.title, -1,
                        tooltip.numericFormatter, null, chart);
                }
            }
            for (int i = tooltip.context.data.param.Count - 1; i >= 0; i--)
            {
                var param = tooltip.context.data.param[i];
                if (IsIgnoreFormatter(param.itemFormatter))
                {
                    tooltip.context.data.param.RemoveAt(i);
                }
            }
            foreach (var param in tooltip.context.data.param)
            {
                if (!string.IsNullOrEmpty(param.itemFormatter))
                {
                    param.columns.Clear();
                    var content = param.itemFormatter;
                    FormatterHelper.ReplaceSerieLabelContent(ref content,
                        param.numericFormatter,
                        param.dataCount,
                        param.value,
                        param.total,
                        param.serieName,
                        param.category,
                        param.serieData.name,
                        param.color,
                        param.serieData,
                        chart,
                        param.serieIndex);
                    foreach (var item in content.Split('|'))
                    {
                        param.columns.Add(item);
                    }
                }
            }
        }

        public static bool IsIgnoreFormatter(string itemFormatter)
        {
            return "-".Equals(itemFormatter) ||"{i}".Equals(itemFormatter, StringComparison.CurrentCultureIgnoreCase);
        }

        public static void LimitInRect(BaseChart chart, Tooltip tooltip, Rect chartRect)
        {
            if (tooltip.view == null)
                return;

            var pos = tooltip.view.GetTargetPos();
            if (pos.x + tooltip.context.width > chartRect.x + chartRect.width)
            {
                pos.x = tooltip.context.pointer.x - tooltip.context.width - tooltip.offset.x;
            }
            else if (pos.x < chartRect.x)
            {
                pos.x = tooltip.context.pointer.x - tooltip.context.width + Mathf.Abs(tooltip.offset.x);
            }
            if (pos.y - tooltip.context.height < chartRect.y)
            {
                pos.y = chartRect.y + tooltip.context.height;
            }
            if (pos.y > chartRect.y + chartRect.height)
                pos.y = chartRect.y + chartRect.height;
            var screenGap = 10;
            var screenPos = chart.LocalPointToScreenPoint(pos);
            if (screenPos.x < screenGap)
                pos.x += Mathf.Abs(screenPos.x) + screenGap;
            if (screenPos.x + tooltip.context.width > Screen.width - screenGap)
                pos.x -= Mathf.Abs(screenPos.x + tooltip.context.width - Screen.width) + screenGap;

            if (screenPos.y < tooltip.context.height + screenGap)
                pos.y += Mathf.Abs(screenPos.y - tooltip.context.height) + screenGap;
            if (screenPos.y > Screen.height - screenGap)
                pos.y -= Mathf.Abs(screenPos.y - Screen.height) + screenGap;

            UpdateContentPos(tooltip, pos, chartRect);
        }

        /// <summary>
        /// 更新文本框位置
        /// </summary>
        /// <param name="pos"></param>
        private static void UpdateContentPos(Tooltip tooltip, Vector2 pos, Rect chartRect)
        {
            if (tooltip.view != null)
            {
                var width = chartRect.width;
                var height = chartRect.height;
                switch (tooltip.position)
                {
                    case Tooltip.Position.Auto:
#if UNITY_ANDROID || UNITY_IOS
                        if (tooltip.fixedY == 0) pos.y = chartRect.x + ChartHelper.GetActualValue(0.7f, height);
                        else pos.y = chartRect.y + ChartHelper.GetActualValue(tooltip.fixedY, height);
#endif
                        break;
                    case Tooltip.Position.Custom:
                        pos = new Vector2(chartRect.x, chartRect.y);
                        pos.x += ChartHelper.GetActualValue(tooltip.fixedX, width);
                        pos.y += ChartHelper.GetActualValue(tooltip.fixedY, height);
                        break;
                    case Tooltip.Position.FixedX:
                        pos = new Vector2(chartRect.x, pos.y);
                        pos.x += ChartHelper.GetActualValue(tooltip.fixedX, width);
                        break;
                    case Tooltip.Position.FixedY:
                        pos = new Vector2(pos.x, chartRect.y);
                        pos.y += ChartHelper.GetActualValue(tooltip.fixedY, height);
                        break;
                }
                tooltip.view.UpdatePosition(pos);
            }
        }

        public static string GetItemNumericFormatter(Tooltip tooltip, Serie serie, SerieData serieData)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!string.IsNullOrEmpty(itemStyle.numericFormatter)) return itemStyle.numericFormatter;
            else return tooltip.numericFormatter;
        }

        public static Color32 GetLineColor(Tooltip tooltip, Color32 defaultColor)
        {
            var lineStyle = tooltip.lineStyle;
            if (!ChartHelper.IsClearColor(lineStyle.color))
            {
                return lineStyle.GetColor();
            }
            else
            {
                var color = defaultColor;
                ChartHelper.SetColorOpacity(ref color, lineStyle.opacity);
                return color;
            }
        }
    }
}