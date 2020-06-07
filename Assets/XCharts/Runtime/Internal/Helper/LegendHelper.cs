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

            var objAnchorMin = legend.location.runtimeAnchorMin;
            var objAnchorMax = legend.location.runtimeAnchorMax;
            var objPivot = legend.location.runtimePivot;
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

        public static void ResetItemPosition(Legend legend, float chartWidth, float chartHeight)
        {
            var startX = 0f;
            var startY = 0f;
            var offsetX = 0f;
            var currWidth = 0f;
            var currHeight = 0f;
            var legendWidth = chartWidth - legend.location.left - legend.location.right;
            var legendHeight = chartHeight - legend.location.top - legend.location.bottom;
            var legendRuntimeWidth = legend.runtimeWidth;
            var legendRuntimeHeight = legend.runtimeHeight;
            switch (legend.orient)
            {
                case Orient.Vertical:
                    switch (legend.location.align)
                    {
                        case Location.Align.TopCenter:
                            startX = legendRuntimeWidth / 2;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                if (currHeight + item.height > legendHeight)
                                {
                                    currHeight = 0;
                                    offsetX += legendRuntimeWidth + legend.itemGap;
                                }
                                item.SetPosition(new Vector3(-startX + item.width / 2 + offsetX, -currHeight));
                                currHeight += item.height + legend.itemGap;
                            }
                            break;
                        case Location.Align.TopLeft:
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                if (currHeight + item.height > legendHeight)
                                {
                                    currHeight = 0;
                                    offsetX += legendRuntimeWidth + legend.itemGap;
                                }
                                item.SetPosition(new Vector3(0 + offsetX, -currHeight));
                                currHeight += item.height + legend.itemGap;
                            }
                            break;
                        case Location.Align.TopRight:
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                if (currHeight + item.height > legendHeight)
                                {
                                    currHeight = 0;
                                    offsetX -= legendRuntimeWidth + legend.itemGap;
                                }
                                item.SetPosition(new Vector3(item.width - legendRuntimeWidth + offsetX, -currHeight));
                                currHeight += item.height + legend.itemGap;
                            }
                            break;

                        case Location.Align.Center:
                            startX = legendRuntimeWidth / 2;
                            startY = legendRuntimeHeight > legendHeight ? legendHeight / 2 : legendRuntimeHeight / 2;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                if (currHeight + item.height > legendHeight)
                                {
                                    currHeight = 0;
                                    offsetX += legendRuntimeWidth + legend.itemGap;
                                }
                                item.SetPosition(new Vector3(-startX + item.width / 2 + offsetX, startY - currHeight));
                                currHeight += item.height + legend.itemGap;
                            }
                            break;
                        case Location.Align.CenterLeft:
                            startY = legendRuntimeHeight > legendHeight ? legendHeight / 2 : legendRuntimeHeight / 2;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                if (currHeight + item.height > legendHeight)
                                {
                                    currHeight = 0;
                                    offsetX += legendRuntimeWidth + legend.itemGap;
                                }
                                item.SetPosition(new Vector3(offsetX, startY - currHeight - item.height));
                                currHeight += item.height + legend.itemGap;
                            }
                            break;
                        case Location.Align.CenterRight:
                            startX = 0;
                            startY = legendRuntimeHeight > legendHeight ? legendHeight / 2 : legendRuntimeHeight / 2;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                if (currHeight + item.height > legendHeight)
                                {
                                    currHeight = 0;
                                    offsetX -= legendRuntimeWidth + legend.itemGap;
                                }
                                item.SetPosition(new Vector3(item.width - legendRuntimeWidth + offsetX, startY - currHeight - item.height));
                                currHeight += item.height + legend.itemGap;
                            }
                            break;
                        case Location.Align.BottomCenter:
                            startX = legendRuntimeWidth / 2;
                            startY = legendRuntimeHeight > legendHeight ? legendHeight : legendRuntimeHeight;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                if (currHeight + item.height > legendHeight)
                                {
                                    currHeight = 0;
                                    offsetX += legendRuntimeWidth + legend.itemGap;
                                }
                                item.SetPosition(new Vector3(-startX + item.width / 2 + offsetX, startY - currHeight - item.height));
                                currHeight += item.height + legend.itemGap;
                            }
                            break;
                        case Location.Align.BottomLeft:
                            startX = 0;
                            startY = legendRuntimeHeight > legendHeight ? legendHeight : legendRuntimeHeight;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                if (currHeight + item.height > legendHeight)
                                {
                                    currHeight = 0;
                                    offsetX += legendRuntimeWidth + legend.itemGap;
                                }
                                item.SetPosition(new Vector3(startX + offsetX, startY - currHeight - item.height));
                                currHeight += item.height + legend.itemGap;
                            }
                            break;
                        case Location.Align.BottomRight:
                            startX = 0;
                            startY = legendRuntimeHeight > legendHeight ? legendHeight : legendRuntimeHeight;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                if (currHeight + item.height > legendHeight)
                                {
                                    currHeight = 0;
                                    offsetX -= legendRuntimeWidth + legend.itemGap;
                                }
                                item.SetPosition(new Vector3(item.width - legendRuntimeWidth + offsetX, startY - currHeight - item.height));
                                currHeight += item.height + legend.itemGap;
                            }
                            break;
                    }
                    break;
                case Orient.Horizonal:
                    switch (legend.location.align)
                    {
                        case Location.Align.TopLeft:
                        case Location.Align.CenterLeft:
                        case Location.Align.BottomLeft:
                            var isBottom = legend.location.align == Location.Align.BottomLeft;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                if (currWidth + item.width > legendWidth)
                                {
                                    currWidth = 0f;
                                    if (isBottom) currHeight += legend.itemGap + item.height;
                                    else currHeight -= legend.itemGap + item.height;
                                }
                                item.SetPosition(new Vector3(currWidth, currHeight));
                                currWidth += item.width + legend.itemGap;
                            }
                            break;
                        case Location.Align.TopCenter:
                        case Location.Align.Center:
                        case Location.Align.BottomCenter:
                            isBottom = legend.location.align == Location.Align.BottomCenter;
                            startX = legendRuntimeWidth > legendWidth ? legendWidth / 2 : legendRuntimeWidth / 2;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                if (currWidth + item.width > legendWidth)
                                {
                                    currWidth = 0f;
                                    if (isBottom) currHeight += legend.itemGap + item.height;
                                    else currHeight -= legend.itemGap + item.height;
                                }
                                item.SetPosition(new Vector3(-startX + item.width / 2 + currWidth, currHeight));
                                currWidth += item.width + legend.itemGap;
                            }
                            break;
                        case Location.Align.TopRight:
                        case Location.Align.CenterRight:
                        case Location.Align.BottomRight:
                            isBottom = legend.location.align == Location.Align.BottomRight;
                            startX = legendRuntimeWidth > legendWidth ? -legendWidth : -legendRuntimeWidth;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                if (currWidth + item.width > legendWidth)
                                {
                                    currWidth = 0f;
                                    if (isBottom) currHeight += legend.itemGap + item.height;
                                    else currHeight -= legend.itemGap + item.height;
                                }
                                item.SetPosition(new Vector3(startX + currWidth + item.width, currHeight));
                                currWidth += item.width + legend.itemGap;
                            }
                            break;
                    }
                    break;
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