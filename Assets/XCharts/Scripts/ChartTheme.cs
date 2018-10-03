using UnityEngine;
using System.Collections;

namespace xcharts
{
    [System.Serializable]
    public enum Theme
    {
        Default = 1,
        Light,
        Dark
    }

    [System.Serializable]
    public class ThemeInfo
    {
        public Font font;
        public Color backgroundColor;
        public Color contrastColor;
        public Color textColor;
        public Color subTextColor;
        public Color unableColor;
  
        public Color axisLineColor;
        public Color axisSplitLineColor;

        public Color[] colorPalette;

        public Color GetColor(int index)
        {
            if(index < 0)
            {
                index = 0;
            }
            index = index % colorPalette.Length;
            return colorPalette[index];
        }

        public void Copy(ThemeInfo theme)
        {
            font = theme.font;
            backgroundColor = theme.backgroundColor;
            contrastColor = theme.contrastColor;
            unableColor = theme.unableColor;
            textColor = theme.textColor;
            subTextColor = theme.subTextColor;
            axisLineColor = theme.axisLineColor;
            axisSplitLineColor = theme.axisSplitLineColor;
            colorPalette = new Color[theme.colorPalette.Length];
            for(int i = 0; i < theme.colorPalette.Length; i++)
            {
                colorPalette[i] = theme.colorPalette[i];
            }
        }

        public static ThemeInfo Default
        {
            get
            {
                return new ThemeInfo()
                {
                    font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
                    backgroundColor = new Color32(255, 255, 255, 255),
                    contrastColor = GetColor("#514D4D"),
                    unableColor = GetColor("#cccccc"),
                    textColor = GetColor("#514D4D"),
                    subTextColor = GetColor("#514D4D"),
                    axisLineColor = GetColor("#514D4D"),
                    axisSplitLineColor = GetColor("#AB9999"),
                    colorPalette = new Color[]
                    {
                        new Color32(194, 53, 49, 255),
                        new Color32(47, 69, 84, 255),
                        new Color32(97, 160, 168, 255),
                        new Color32(212, 130, 101, 255),
                        new Color32(145, 199, 174, 255),
                        new Color32(116, 159, 131, 255),
                        new Color32(202, 134, 34, 255),
                        new Color32(189, 162, 154, 255),
                        new Color32(110, 112, 116, 255),
                        new Color32(84, 101, 112, 255),
                        new Color32(196, 204, 211, 255)
                    }
                };
            }
        }

        public static ThemeInfo Light
        {
            get
            {
                return new ThemeInfo()
                {
                    font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
                    backgroundColor = new Color32(255, 255, 255, 255),
                    contrastColor = GetColor("#514D4D"),
                    unableColor = GetColor("#cccccc"),
                    textColor = GetColor("#514D4D"),
                    subTextColor = GetColor("#514D4D"),
                    axisLineColor = GetColor("#514D4D"),
                    axisSplitLineColor = GetColor("#AB9999"),
                    colorPalette = new Color[]
                    {
                        new Color32(55, 162, 218, 255),
                        new Color32(255, 159, 127, 255),
                        new Color32(50, 197, 233, 255),
                        new Color32(251, 114, 147, 255),
                        new Color32(103, 224, 227, 255),
                        new Color32(224, 98, 174, 255),
                        new Color32(159, 230, 184, 255),
                        new Color32(230, 144, 209, 255),
                        new Color32(255, 219, 92, 255),
                        new Color32(230, 188, 243, 255),
                        new Color32(157, 150, 245, 255),
                        new Color32(131, 120, 234, 255),
                        new Color32(150, 191, 255, 255)
                    }
                };
            }
        }

        public static ThemeInfo Dark
        {
            get
            {
                return new ThemeInfo()
                {
                    font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
                    unableColor = GetColor("#cccccc"),
                    backgroundColor = new Color32(34, 34, 34, 255),
                    contrastColor = GetColor("#eee"),
                    textColor = GetColor("#eee"),
                    subTextColor = GetColor("#eee"),
                    axisLineColor = GetColor("#eee"),
                    axisSplitLineColor = GetColor("#aaa"),
                    colorPalette = new Color[]
                    {
                        new Color32(221, 107, 102, 255),
                        new Color32(117, 154, 160, 255),
                        new Color32(230, 157, 135, 255),
                        new Color32(141, 193, 169, 255),
                        new Color32(234, 126, 83, 255),
                        new Color32(238, 221, 120, 255),
                        new Color32(115, 163, 115, 255),
                        new Color32(115, 185, 188, 255),
                        new Color32(114, 137, 171, 255),
                        new Color32(145, 202, 140, 255),
                        new Color32(244, 159, 66, 255)
                    }
                };
            }
        }

        public static Color GetColor(string hexColorStr)
        {
            Color color;
            ColorUtility.TryParseHtmlString(hexColorStr,out color);
            return color;
        }
    }
}

