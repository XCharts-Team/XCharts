/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(SerieLabel), true)]
    public class SerieLabelDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Label"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Position");
                PropertyField(prop, "m_Offset");
                PropertyField(prop, "m_AutoOffset");
                PropertyField(prop, "m_Margin");
                PropertyField(prop, "m_Formatter");
                PropertyField(prop, "m_NumericFormatter");
                PropertyField(prop, "m_BackgroundWidth");
                PropertyField(prop, "m_BackgroundHeight");
                PropertyField(prop, "m_PaddingLeftRight");
                PropertyField(prop, "m_PaddingTopBottom");
                PropertyField(prop, "m_Border");
                PropertyField(prop, "m_BorderWidth");
                PropertyField(prop, "m_BorderColor");
                PropertyField(prop, "m_Line");
                PropertyField(prop, "m_LineType");
                PropertyField(prop, "m_LineColor");
                PropertyField(prop, "m_LineWidth");
                PropertyField(prop, "m_LineGap");
                PropertyField(prop, "m_LineLength1");
                PropertyField(prop, "m_LineLength2");
                PropertyField(prop, "m_TextStyle");
                --EditorGUI.indentLevel;
            }
        }
    }
}