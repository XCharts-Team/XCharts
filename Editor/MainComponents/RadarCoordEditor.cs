using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(RadarCoord))]
    public class RadarCoordEditor : MainComponentEditor<RadarCoord>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_Shape");
            PropertyField("m_PositionType");
            PropertyTwoFiled("m_Center");
            PropertyField("m_Radius");
            PropertyField("m_SplitNumber");
            PropertyField("m_StartAngle");
            PropertyField("m_CeilRate");
            PropertyField("m_IsAxisTooltip");
            PropertyField("m_OutRangeColor");
            PropertyField("m_ConnectCenter");
            PropertyField("m_LineGradient");
            PropertyField("m_AxisLine");
            PropertyField("m_AxisName");
            PropertyField("m_SplitLine");
            PropertyField("m_SplitArea");
            PropertyListField("m_IndicatorList");
            --EditorGUI.indentLevel;
        }
    }

    [CustomPropertyDrawer(typeof(RadarCoord.Indicator), true)]
    public class RadarIndicatorDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Indicator"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Name");
                PropertyField(prop, "m_Min");
                PropertyField(prop, "m_Max");
                PropertyTwoFiled(prop, "m_Range");
                --EditorGUI.indentLevel;
            }
        }
    }
}