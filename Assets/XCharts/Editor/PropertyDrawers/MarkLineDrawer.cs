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
    [CustomPropertyDrawer(typeof(MarkLine), true)]
    public class MarkLineDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "MarkLine"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Animation");
                PropertyListField(prop, "m_Data", true);
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(MarkLineData), true)]
    public class MarkLineDataDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "MarkLineData"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, ""))
            {
                ++EditorGUI.indentLevel;
                var type = (MarkLineType)(prop.FindPropertyRelative("m_Type")).enumValueIndex;
                var group = prop.FindPropertyRelative("m_Group").intValue;
                PropertyField(prop, "m_Type");
                PropertyField(prop, "m_Name");
                switch (type)
                {
                    case MarkLineType.None:
                        PropertyField(prop, "m_XPosition");
                        PropertyField(prop, "m_YPosition");
                        PropertyField(prop, "m_XValue");
                        PropertyField(prop, "m_YValue");
                        break;
                    case MarkLineType.Min:
                    case MarkLineType.Max:
                    case MarkLineType.Average:
                    case MarkLineType.Median:
                        PropertyField(prop, "m_Dimension");
                        break;
                }
                PropertyField(prop, "m_Group");
                if (group > 0 && type == MarkLineType.None) PropertyField(prop, "m_ZeroPosition");
                PropertyField(prop, "m_LineStyle");
                PropertyField(prop, "m_StartSymbol");
                PropertyField(prop, "m_EndSymbol");
                PropertyField(prop, "m_Label");
                --EditorGUI.indentLevel;
            }
        }
    }
}