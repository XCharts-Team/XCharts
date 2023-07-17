using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(ComponentTheme), true)]
    public class ComponentThemeDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return ""; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "", true))
            {
                ++EditorGUI.indentLevel;
#if dUI_TextMeshPro
                PropertyField(prop, "m_TMPFont");
#else
                PropertyField(prop, "m_Font");
#endif
                PropertyField(prop, "m_FontSize");
                PropertyField(prop, "m_TextColor");
                DrawExtendeds(prop);
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(BaseAxisTheme), true)]
    public class BaseAxisThemeDrawer : ComponentThemeDrawer
    {
        public override string ClassName { get { return "Axis"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            PropertyField(prop, "m_LineType");
            PropertyField(prop, "m_LineWidth");
            PropertyField(prop, "m_LineLength");
            PropertyField(prop, "m_LineColor");
            PropertyField(prop, "m_SplitLineType");
            PropertyField(prop, "m_SplitLineWidth");
            PropertyField(prop, "m_SplitLineLength");
            PropertyField(prop, "m_SplitLineColor");
            PropertyField(prop, "m_TickWidth");
            PropertyField(prop, "m_TickLength");
            PropertyField(prop, "m_TickColor");
            PropertyField(prop, "m_SplitAreaColors");
        }
    }

    [CustomPropertyDrawer(typeof(AxisTheme), true)]
    public class AxisThemeDrawer : BaseAxisThemeDrawer
    {
        public override string ClassName { get { return "Axis"; } }
    }

    [CustomPropertyDrawer(typeof(RadiusAxisTheme), true)]
    public class RadiusAxisThemeDrawer : BaseAxisThemeDrawer
    {
        public override string ClassName { get { return "Radius Axis"; } }
        public override List<string> IngorePropertys
        {
            get
            {
                return new List<string>
                {
                    "m_TextBackgroundColor",
                    "m_LineLength",
                    "m_SplitLineLength",
                };
            }
        }
    }

    [CustomPropertyDrawer(typeof(DataZoomTheme), true)]
    public class DataZoomThemeDrawer : ComponentThemeDrawer
    {
        public override string ClassName { get { return "DataZoom"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            PropertyField(prop, "m_BackgroundColor");
            PropertyField(prop, "m_BorderWidth");
            PropertyField(prop, "m_BorderColor");
            PropertyField(prop, "m_DataLineWidth");
            PropertyField(prop, "m_DataLineColor");
            PropertyField(prop, "m_FillerColor");
            PropertyField(prop, "m_DataAreaColor");

        }
    }

    [CustomPropertyDrawer(typeof(LegendTheme), true)]
    public class LegendThemeDrawer : ComponentThemeDrawer
    {
        public override string ClassName { get { return "Legend"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            PropertyField(prop, "m_UnableColor");
        }
    }

    [CustomPropertyDrawer(typeof(TooltipTheme), true)]
    public class TooltipThemeDrawer : ComponentThemeDrawer
    {
        public override string ClassName { get { return "Tooltip"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            PropertyField(prop, "m_LineType");
            PropertyField(prop, "m_LineWidth");
            PropertyField(prop, "m_LineColor");
            PropertyField(prop, "m_AreaColor");
            PropertyField(prop, "m_LabelTextColor");
            PropertyField(prop, "m_LabelBackgroundColor");
        }
    }

    [CustomPropertyDrawer(typeof(VisualMapTheme), true)]
    public class VisualMapThemeDrawer : ComponentThemeDrawer
    {
        public override string ClassName { get { return "VisualMap"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            // PropertyField(prop, "m_BorderWidth");
            // PropertyField(prop, "m_BorderColor");
            // PropertyField(prop, "m_BackgroundColor");
        }
    }

    [CustomPropertyDrawer(typeof(SerieTheme), true)]
    public class SerieThemeDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Serie"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_LineWidth");
                PropertyField(prop, "m_LineSymbolSize");
                PropertyField(prop, "m_ScatterSymbolSize");
                PropertyField(prop, "m_CandlestickColor");
                PropertyField(prop, "m_CandlestickColor0");
                PropertyField(prop, "m_CandlestickBorderColor");
                PropertyField(prop, "m_CandlestickBorderColor0");
                PropertyField(prop, "m_CandlestickBorderWidth");
                --EditorGUI.indentLevel;
            }
        }
    }
}