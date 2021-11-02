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
    [CustomPropertyDrawer(typeof(Axis), true)]
    public class AxisDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Axis"; } }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                SerializedProperty m_Type = prop.FindPropertyRelative("m_Type");
                SerializedProperty m_LogBase = prop.FindPropertyRelative("m_LogBase");
                SerializedProperty m_MinMaxType = prop.FindPropertyRelative("m_MinMaxType");
                Axis.AxisType type = (Axis.AxisType)m_Type.enumValueIndex;
                var chart = prop.serializedObject.targetObject as BaseChart;
                var isPolar = chart is PolarChart;
                EditorGUI.indentLevel++;
                PropertyField(prop, isPolar ? "m_PolarIndex" : "m_GridIndex");
                PropertyField(prop, "m_Type");
                if (type == Axis.AxisType.Time)
                {
                    var chartTypeName = prop.serializedObject.targetObject.GetType().Name;
                    if (!chartTypeName.Equals("GanttChart"))
                        EditorGUILayout.HelpBox("The Time axis is currently only supported in GanttChart.", MessageType.Warning);
                }
                PropertyField(prop, "m_Position");
                PropertyField(prop, "m_Offset");
                if (type == Axis.AxisType.Log)
                {
                    PropertyField(prop, "m_LogBaseE");
                    EditorGUI.BeginChangeCheck();
                    PropertyField(prop, "m_LogBase");
                    if (m_LogBase.floatValue <= 0 || m_LogBase.floatValue == 1)
                    {
                        m_LogBase.floatValue = 10;
                    }
                    EditorGUI.EndChangeCheck();
                }
                if (type == Axis.AxisType.Value || type == Axis.AxisType.Time)
                {
                    PropertyField(prop, "m_MinMaxType");
                    Axis.AxisMinMaxType minMaxType = (Axis.AxisMinMaxType)m_MinMaxType.enumValueIndex;
                    switch (minMaxType)
                    {
                        case Axis.AxisMinMaxType.Default:
                            break;
                        case Axis.AxisMinMaxType.MinMax:
                            break;
                        case Axis.AxisMinMaxType.Custom:
                            EditorGUI.indentLevel++;
                            PropertyField(prop, "m_Min");
                            PropertyField(prop, "m_Max");
                            EditorGUI.indentLevel--;
                            break;
                    }
                    PropertyField(prop, "m_CeilRate");
                    if (type == Axis.AxisType.Value)
                    {
                        PropertyField(prop, "m_Inverse");
                    }
                }
                PropertyField(prop, "m_SplitNumber");
                if (type == Axis.AxisType.Category)
                {
                    PropertyField(prop, "m_InsertDataToHead");
                    PropertyField(prop, "m_MaxCache");
                    PropertyField(prop, "m_BoundaryGap");
                }
                else
                {
                    PropertyField(prop, "m_Interval");
                }
                DrawExtendeds(prop);
                PropertyField(prop, "m_AxisLine");
                PropertyField(prop, "m_AxisName");
                PropertyField(prop, "m_AxisTick");
                PropertyField(prop, "m_AxisLabel");
                PropertyField(prop, "m_SplitLine");
                PropertyField(prop, "m_SplitArea");
                PropertyField(prop, "m_IconStyle");
                PropertyListField(prop, "m_Icons", true);
                if (type == Axis.AxisType.Category)
                {
                    PropertyListField(prop, "m_Data", true);
                }
                EditorGUI.indentLevel--;
            }
        }
    }

    [CustomPropertyDrawer(typeof(XAxis), true)]
    public class XAxisDrawer : AxisDrawer
    {
        public override string ClassName { get { return "XAxis"; } }
    }

    [CustomPropertyDrawer(typeof(YAxis), true)]
    public class YAxisDrawer : AxisDrawer
    {
        public override string ClassName { get { return "YAxis"; } }
    }

    [CustomPropertyDrawer(typeof(AngleAxis), true)]
    public class AngleAxisDrawer : AxisDrawer
    {
        public override string ClassName { get { return "AngleAxis"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            PropertyField(prop, "m_StartAngle");
            PropertyField(prop, "m_Clockwise");
        }
    }

    [CustomPropertyDrawer(typeof(RadiusAxis), true)]
    public class RadiusAxisDrawer : AxisDrawer
    {
        public override string ClassName { get { return "RadiusAxis"; } }
    }

    [CustomPropertyDrawer(typeof(AxisLabel), true)]
    public class AxisLabelDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "AxisLabel"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Inside");
                PropertyField(prop, "m_Interval");
                PropertyField(prop, "m_Margin");
                PropertyField(prop, "m_Width");
                PropertyField(prop, "m_Height");
                PropertyField(prop, "m_Formatter");
                PropertyField(prop, "m_NumericFormatter");
                PropertyField(prop, "m_ShowAsPositiveNumber");
                PropertyField(prop, "m_OnZero");
                PropertyField(prop, "m_ShowStartLabel");
                PropertyField(prop, "m_ShowEndLabel");
                PropertyField(prop, "m_TextLimit");
                PropertyField(prop, "m_TextStyle");
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(AxisName), true)]
    public class AxisNameDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "AxisName"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Name");
                PropertyField(prop, "m_Location");
                PropertyField(prop, "m_TextStyle");
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(AxisSplitArea), true)]
    public class AxisSplitAreaDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "SplitArea"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Color");
                --EditorGUI.indentLevel;
            }
        }
    }
}