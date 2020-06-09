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
    internal static class LegendHelper
    {
        public static Color GetContentColor(Legend legend, ThemeInfo themeInfo, bool active)
        {
            var textStyle = legend.textStyle;
            if (active) return !ChartHelper.IsClearColor(textStyle.color) ? textStyle.color : (Color)themeInfo.legendTextColor;
            else return (Color)themeInfo.legendUnableColor;
        }

        public static Color GetIconColor(Legend legend, int readIndex, ThemeInfo themeInfo, bool active)
        {
            if (active)
            {
                if (legend.itemAutoColor || legend.GetIcon(readIndex) == null)
                    return (Color)themeInfo.GetColor(readIndex);
                else
                    return Color.white;
            }
            else return (Color)themeInfo.legendUnableColor;
        }

        public static LegendItem AddLegendItem(Legend legend, int i, string legendName, Transform parent, ThemeInfo themeInfo,
            string content, Color itemColor, bool active)
        {
            var objName = i + "_" + legendName;
            var anchorMin = new Vector2(0, 0.5f);
            var anchorMax = new Vector2(0, 0.5f);
            var pivot = new Vector2(0, 0.5f);
            var sizeDelta = new Vector2(100, 30);
            var iconSizeDelta = new Vector2(legend.itemWidth, legend.itemHeight);
            var textStyle = legend.textStyle;
            var font = textStyle.font ? textStyle.font : themeInfo.font;
            var contentColor = GetContentColor(legend, themeInfo, active);

            var objAnchorMin = new Vector2(0, 1);
            var objAnchorMax = new Vector2(0, 1);
            var objPivot = new Vector2(0, 1);
            var btnObj = ChartHelper.AddObject(objName, parent, objAnchorMin, objAnchorMax, objPivot, sizeDelta, i);
            var iconObj = ChartHelper.AddObject("icon", btnObj.transform, anchorMin, anchorMax, pivot, iconSizeDelta);
            var contentObj = ChartHelper.AddObject("content", btnObj.transform, anchorMin, anchorMax, pivot, sizeDelta);
            var img = ChartHelper.GetOrAddComponent<Image>(btnObj);
            img.color = Color.clear;
            ChartHelper.GetOrAddComponent<Button>(btnObj);
            ChartHelper.GetOrAddComponent<Image>(iconObj);
            ChartHelper.GetOrAddComponent<Image>(contentObj);
            ChartHelper.AddTextObject("Text", contentObj.transform, font, contentColor,
                    TextAnchor.MiddleLeft, anchorMin, anchorMax, pivot, sizeDelta, textStyle.fontSize,
                    textStyle.rotate, textStyle.fontStyle, textStyle.lineSpacing);
            var item = new LegendItem();
            item.index = i;
            item.name = objName;
            item.legendName = legendName;
            item.SetObject(btnObj);
            item.SetIconSize(legend.itemWidth, legend.itemHeight);
            item.SetIconColor(itemColor);
            item.SetIconImage(legend.GetIcon(i));
            item.SetContentPosition(textStyle.offsetv3);
            item.SetContent(content);
            item.SetContentBackgroundColor(textStyle.backgroundColor);
            return item;
        }

        public static void ResetItemPosition(Legend legend, Vector3 chartPos, float chartWidth, float chartHeight)
        {
            var startX = 0f;
            var startY = 0f;
            var legendMaxWidth = chartWidth - legend.location.left - legend.location.right;
            var legendMaxHeight = chartHeight - legend.location.top - legend.location.bottom;
            UpdateLegendWidthAndHeight(legend, legendMaxWidth, legendMaxHeight);
            var legendRuntimeWidth = legend.runtimeWidth;
            var legendRuntimeHeight = legend.runtimeHeight;
            var isVertical = legend.orient == Orient.Vertical;
            switch (legend.location.align)
            {
                case Location.Align.TopCenter:
                    startX = chartPos.x + chartWidth / 2 - legendRuntimeWidth / 2;
                    startY = chartPos.y + chartHeight - legend.location.top;
                    break;
                case Location.Align.TopLeft:
                    startX = chartPos.x + legend.location.left;
                    startY = chartPos.y + chartHeight - legend.location.top;
                    break;
                case Location.Align.TopRight:
                    startX = chartPos.x + chartWidth - legendRuntimeWidth - legend.location.right;
                    startY = chartPos.y + chartHeight - legend.location.top;
                    break;
                case Location.Align.Center:
                    startX = chartPos.x + chartWidth / 2 - legendRuntimeWidth / 2;
                    startY = chartPos.y + chartHeight / 2 + legendRuntimeHeight / 2;
                    break;
                case Location.Align.CenterLeft:
                    startX = chartPos.x + legend.location.left;
                    startY = chartPos.y + chartHeight / 2 + legendRuntimeHeight / 2;
                    break;
                case Location.Align.CenterRight:
                    startX = chartPos.x + chartWidth - legendRuntimeWidth - legend.location.right;
                    startY = chartPos.y + chartHeight / 2 + legendRuntimeHeight / 2;
                    break;
                case Location.Align.BottomCenter:
                    startX = chartPos.x + chartWidth / 2 - legendRuntimeWidth / 2;
                    startY = chartPos.y + legendRuntimeHeight + legend.location.bottom;
                    break;
                case Location.Align.BottomLeft:
                    startX = chartPos.x + legend.location.left;
                    startY = chartPos.y + legendRuntimeHeight + legend.location.bottom;
                    break;
                case Location.Align.BottomRight:
                    startX = chartPos.x + chartWidth - legendRuntimeWidth - legend.location.right;
                    startY = chartPos.y + legendRuntimeHeight + legend.location.bottom;
                    break;
            }
            if (isVertical) SetVerticalItemPosition(legend, legendMaxHeight, startX, startY);
            else SetHorizonalItemPosition(legend, legendMaxWidth, startX, startY);
        }

        private static void SetVerticalItemPosition(Legend legend, float legendMaxHeight, float startX, float startY)
        {
            var currHeight = 0f;
            var offsetX = 0f;
            var row = 0;
            foreach (var kv in legend.buttonList)
            {
                var item = kv.Value;
                if (currHeight + item.height > legendMaxHeight)
                {
                    currHeight = 0;
                    offsetX += legend.runtimeEachWidth[row];
                    row++;
                }
                item.SetPosition(new Vector3(startX + offsetX, startY - currHeight));
                currHeight += item.height + legend.itemGap;
            }
        }
        private static void SetHorizonalItemPosition(Legend legend, float legendMaxWidth, float startX, float startY)
        {
            var currWidth = 0f;
            var offsetY = 0f;
            foreach (var kv in legend.buttonList)
            {
                var item = kv.Value;
                if (currWidth + item.width > legendMaxWidth)
                {
                    currWidth = 0;
                    offsetY += legend.runtimeEachHeight;
                }
                item.SetPosition(new Vector3(startX + currWidth, startY - offsetY));
                currWidth += item.width + legend.itemGap;
            }
        }

        private static void UpdateLegendWidthAndHeight(Legend legend, float maxWidth, float maxHeight)
        {
            var width = 0f;
            var height = 0f;
            var realHeight = 0f;
            var realWidth = 0f;
            legend.runtimeEachWidth.Clear();
            legend.runtimeEachHeight = 0;
            if (legend.orient == Orient.Horizonal)
            {
                foreach (var kv in legend.buttonList)
                {
                    if (width + kv.Value.width > maxWidth)
                    {
                        realWidth = width - legend.itemGap;
                        realHeight += height + legend.itemGap;
                        if (legend.runtimeEachHeight < height + legend.itemGap)
                        {
                            legend.runtimeEachHeight = height + legend.itemGap;
                        }
                        height = 0;
                        width = 0;
                    }
                    width += kv.Value.width + legend.itemGap;
                    if (kv.Value.height > height)
                        height = kv.Value.height;
                }
                width -= legend.itemGap;
                legend.runtimeHeight = realHeight + height;
                legend.runtimeWidth = realWidth > 0 ? realWidth : width;
            }
            else
            {
                var row = 0;
                foreach (var kv in legend.buttonList)
                {
                    if (height + kv.Value.height > maxHeight)
                    {
                        realHeight = height - legend.itemGap;
                        realWidth += width + legend.itemGap;
                        legend.runtimeEachWidth[row] = width + legend.itemGap;
                        row++;
                        height = 0;
                        width = 0;
                    }
                    height += kv.Value.height + legend.itemGap;
                    if (kv.Value.width > width)
                        width = kv.Value.width;
                }
                height -= legend.itemGap;
                legend.runtimeHeight = realHeight > 0 ? realHeight : height;
                legend.runtimeWidth = realWidth + width;
            }
        }

        private static bool IsBeyondWidth(Legend legend, float maxWidth)
        {
            var totalWidth = 0f;
            foreach (var kv in legend.buttonList)
            {
                var item = kv.Value;
                totalWidth += item.width + legend.itemGap;
                if (totalWidth > maxWidth) return true;
            }
            return false;
        }

        public static bool CheckDataShow(Series series, string legendName, bool show)
        {
            bool needShow = false;
            foreach (var serie in series.list)
            {
                if (legendName.Equals(serie.name))
                {
                    serie.show = show;
                    serie.highlighted = false;
                    if (serie.show) needShow = true;
                }
                else
                {
                    foreach (var data in serie.data)
                    {
                        if (legendName.Equals(data.name))
                        {
                            data.show = show;
                            data.highlighted = false;
                            if (data.show) needShow = true;
                        }
                    }
                }
            }
            return needShow;
        }

        public static bool CheckDataHighlighted(Series series, string legendName, bool heighlight)
        {
            bool show = false;
            foreach (var serie in series.list)
            {
                if (legendName.Equals(serie.name))
                {
                    serie.highlighted = heighlight;
                }
                else
                {
                    foreach (var data in serie.data)
                    {
                        if (legendName.Equals(data.name))
                        {
                            data.highlighted = heighlight;
                            if (data.highlighted) show = true;
                        }
                    }
                }
            }
            return show;
        }
    }
}