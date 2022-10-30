using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(Axis))]
    public class AxisEditor : MainComponentEditor<Axis>
    {
        public override void OnInspectorGUI()
        {
            var m_Type = baseProperty.FindPropertyRelative("m_Type");
            var m_LogBase = baseProperty.FindPropertyRelative("m_LogBase");
            var m_MinMaxType = baseProperty.FindPropertyRelative("m_MinMaxType");
            var type = (Axis.AxisType) m_Type.enumValueIndex;
            EditorGUI.indentLevel++;
            if (component is ParallelAxis)
            {
                PropertyField("m_ParallelIndex");
            }
            else if (!(component is SingleAxis))
            {
                PropertyField("m_GridIndex");
                PropertyField("m_PolarIndex");
            }
            PropertyField("m_Type");
            PropertyField("m_Position");
            PropertyField("m_Offset");
            if (type == Axis.AxisType.Log)
            {
                PropertyField("m_LogBaseE");
                EditorGUI.BeginChangeCheck();
                PropertyField("m_LogBase");
                if (m_LogBase.floatValue <= 0 || m_LogBase.floatValue == 1)
                {
                    m_LogBase.floatValue = 10;
                }
                EditorGUI.EndChangeCheck();
            }
            if (type == Axis.AxisType.Value || type == Axis.AxisType.Time)
            {
                PropertyField("m_MinMaxType");
                Axis.AxisMinMaxType minMaxType = (Axis.AxisMinMaxType) m_MinMaxType.enumValueIndex;
                switch (minMaxType)
                {
                    case Axis.AxisMinMaxType.Default:
                        break;
                    case Axis.AxisMinMaxType.MinMax:
                        break;
                    case Axis.AxisMinMaxType.Custom:
                        EditorGUI.indentLevel++;
                        PropertyField("m_Min");
                        PropertyField("m_Max");
                        EditorGUI.indentLevel--;
                        break;
                }
                PropertyField("m_CeilRate");
                if (type == Axis.AxisType.Value)
                {
                    PropertyField("m_Inverse");
                }
            }
            PropertyField("m_SplitNumber");
            if (type == Axis.AxisType.Category)
            {
                //PropertyField("m_InsertDataToHead");
                PropertyField("m_MaxCache");
                PropertyField("m_BoundaryGap");
            }
            else
            {
                PropertyField("m_Interval");
            }
            DrawExtendeds();
            PropertyField("m_AxisLine");
            PropertyField("m_AxisName");
            PropertyField("m_AxisTick");
            PropertyField("m_AxisLabel");
            PropertyField("m_SplitLine");
            PropertyField("m_SplitArea");
            PropertyField("m_IndicatorLabel");
            if (type != Axis.AxisType.Category)
            {
                PropertyField("m_MinorTick");
                PropertyField("m_MinorSplitLine");
            }
            PropertyListField("m_Icons", true);
            if (type == Axis.AxisType.Category)
            {
                PropertyListField("m_Data", true, new HeaderMenuInfo("Import ECharts Axis Data", () =>
                {
                    PraseExternalDataEditor.UpdateData(chart, null, component as Axis);
                    PraseExternalDataEditor.ShowWindow();
                }));
            }
            EditorGUI.indentLevel--;
        }
    }

    [ComponentEditor(typeof(XAxis))]
    public class XAxisEditor : AxisEditor { }

    [ComponentEditor(typeof(YAxis))]
    public class YAxisEditor : AxisEditor { }

    [ComponentEditor(typeof(SingleAxis))]
    public class SingleAxisEditor : AxisEditor
    {
        protected override void DrawExtendeds()
        {
            base.DrawExtendeds();
            PropertyField("m_Orient");
            PropertyField("m_Left");
            PropertyField("m_Right");
            PropertyField("m_Top");
            PropertyField("m_Bottom");
            PropertyField("m_Width");
            PropertyField("m_Height");
        }
    }

    [ComponentEditor(typeof(AngleAxis))]
    public class AngleAxisEditor : AxisEditor
    {
        protected override void DrawExtendeds()
        {
            base.DrawExtendeds();
            PropertyField("m_StartAngle");
            PropertyField("m_Clockwise");
        }
    }

    [ComponentEditor(typeof(RadiusAxis))]
    public class RadiusAxisEditor : AxisEditor { }

    [ComponentEditor(typeof(ParallelAxis))]
    public class ParallelAxisEditor : AxisEditor { }

    [CustomPropertyDrawer(typeof(AxisLabel), true)]
    public class AxisLabelDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "AxisLabel"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Inside");
                PropertyField(prop, "m_Interval");

                PropertyField(prop, "m_ShowAsPositiveNumber");
                PropertyField(prop, "m_OnZero");
                PropertyField(prop, "m_ShowStartLabel");
                PropertyField(prop, "m_ShowEndLabel");

                PropertyField(prop, "m_Rotate");
                PropertyField(prop, "m_Offset");
                PropertyField(prop, "m_Distance");
                PropertyField(prop, "m_Formatter");
                PropertyField(prop, "m_NumericFormatter");
                PropertyField(prop, "m_Width");
                PropertyField(prop, "m_Height");
                PropertyField(prop, "m_Icon");
                PropertyField(prop, "m_Background");
                PropertyField(prop, "m_TextStyle");
                PropertyField(prop, "m_TextPadding");
                PropertyField(prop, "m_TextLimit");
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
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Name");
                PropertyField(prop, "m_OnZero");
                PropertyField(prop, "m_LabelStyle");
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
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Color");
                --EditorGUI.indentLevel;
            }
        }
    }
}