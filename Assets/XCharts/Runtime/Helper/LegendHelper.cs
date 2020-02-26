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
            if (active) return textStyle.color != Color.clear ? textStyle.color : (Color)themeInfo.legendTextColor;
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

            var btnObj = ChartHelper.AddObject(objName, parent, objAnchorMin, objAnchorMax, objPivot, sizeDelta);
            var iconObj = ChartHelper.AddObject("icon", btnObj.transform, anchorMin, anchorMax, pivot, iconSizeDelta);
            var contentObj = ChartHelper.AddObject("content", btnObj.transform, anchorMin, anchorMax, pivot, sizeDelta);
            var img = ChartHelper.GetOrAddComponent<Image>(btnObj);
            img.color = Color.clear;
            ChartHelper.GetOrAddComponent<Button>(btnObj);
            ChartHelper.GetOrAddComponent<Image>(iconObj);
            ChartHelper.GetOrAddComponent<Image>(contentObj);
            Text txt = ChartHelper.AddTextObject("Text", contentObj.transform, font, contentColor,
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

        public static void ResetItemPosition(Legend legend)
        {
            var startX = 0f;
            var startY = 0f;
            var currWidth = 0f;
            var currHeight = 0f;

            switch (legend.orient)
            {
                case Orient.Vertical:
                    switch (legend.location.align)
                    {
                        case Location.Align.TopCenter:
                            startX = legend.runtimeWidth / 2;
                            currHeight = 0f;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                item.SetPosition(new Vector3(-startX + item.width / 2, -currHeight));
                                currHeight += item.height + legend.itemGap;
                            }
                            break;
                        case Location.Align.TopLeft:
                            currHeight = 0f;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                item.SetPosition(new Vector3(0, -currHeight));
                                currHeight += item.height + legend.itemGap;
                            }
                            break;

                        case Location.Align.TopRight:
                            currHeight = 0f;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                item.SetPosition(new Vector3(item.width - legend.runtimeWidth, -currHeight));
                                currHeight += item.height + legend.itemGap;
                            }
                            break;

                        case Location.Align.Center:
                            startX = legend.runtimeWidth / 2;
                            currHeight = legend.runtimeHeight;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                item.SetPosition(new Vector3(-startX + item.width / 2, currHeight - item.height));
                                currHeight -= item.height + legend.itemGap;
                            }
                            break;
                        case Location.Align.CenterLeft:
                            currHeight = legend.runtimeHeight;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                item.SetPosition(new Vector3(0, currHeight - item.height));
                                currHeight -= item.height + legend.itemGap;
                            }
                            break;
                        case Location.Align.CenterRight:
                            currHeight = legend.runtimeHeight;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                item.SetPosition(new Vector3(item.width - legend.runtimeWidth, currHeight - item.height));
                                currHeight -= item.height + legend.itemGap;
                            }
                            break;
                        case Location.Align.BottomCenter:
                            startX = legend.runtimeWidth / 2;
                            currHeight = legend.runtimeHeight;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                item.SetPosition(new Vector3(-startX + item.width / 2, currHeight - item.height));
                                currHeight -= item.height + legend.itemGap;
                            }
                            break;

                        case Location.Align.BottomLeft:
                            currHeight = legend.runtimeHeight;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                item.SetPosition(new Vector3(0, currHeight - item.height));
                                currHeight -= item.height + legend.itemGap;
                            }
                            break;
                        case Location.Align.BottomRight:
                            currHeight = legend.runtimeHeight;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                item.SetPosition(new Vector3(item.width - legend.runtimeWidth, currHeight - item.height));
                                currHeight -= item.height + legend.itemGap;
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
                            currWidth = 0f;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                item.SetPosition(new Vector3(currWidth, 0));
                                currWidth += item.width + legend.itemGap;
                            }
                            break;
                        case Location.Align.TopCenter:
                        case Location.Align.Center:
                        case Location.Align.BottomCenter:
                            startX = legend.runtimeWidth / 2;
                            currWidth = 0f;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                item.SetPosition(new Vector3(-startX + item.width / 2 + currWidth, 0));
                                currWidth += item.width + legend.itemGap;
                            }
                            break;
                        case Location.Align.TopRight:
                        case Location.Align.CenterRight:
                        case Location.Align.BottomRight:
                            startX = -legend.runtimeWidth;
                            currWidth = 0f;
                            foreach (var kv in legend.buttonList)
                            {
                                var item = kv.Value;
                                item.SetPosition(new Vector3(startX + currWidth + item.width, 0));
                                currWidth += item.width + legend.itemGap;
                            }
                            break;
                            break;
                    }
                    break;
            }
        }
    }
}