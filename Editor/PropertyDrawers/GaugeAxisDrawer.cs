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
    [CustomPropertyDrawer(typeof(GaugeAxis), true)]
    public class GaugeAxisDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Gauge Axis"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_AxisLine");
                PropertyField(prop, "m_SplitLine");
                PropertyField(prop, "m_AxisTick");
                PropertyField(prop, "m_AxisLabel");
                PropertyField(prop, "m_AxisLabelText");
                --EditorGUI.indentLevel;
            }
        }
    }


    [CustomPropertyDrawer(typeof(StageColor), true)]
    public class GaugeAxisLineStageColorDrawer : BasePropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty m_Percent = prop.FindPropertyRelative("m_Percent");
            SerializedProperty m_Color = prop.FindPropertyRelative("m_Color");

            ChartEditorHelper.MakeTwoField(ref drawRect, drawRect.width, m_Percent, m_Color, "Stage");
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
        }
    }

    [CustomPropertyDrawer(typeof(GaugePointer), true)]
    public class GaugePointerDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Gauge Pointer"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Width");
                PropertyField(prop, "m_Length");
                --EditorGUI.indentLevel;
            }
        }
    }
}