using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(Settings), true)]
    public class SettingsDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Settings"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", false, new HeaderMenuInfo("Reset", () =>
                {
                    var chart = prop.serializedObject.targetObject as BaseChart;
                    chart.settings.Reset();
                })))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_ReversePainter");
                PropertyField(prop, "m_MaxPainter");
                PropertyField(prop, "m_BasePainterMaterial");
                PropertyField(prop, "m_SeriePainterMaterial");
                PropertyField(prop, "m_UpperPainterMaterial");
                PropertyField(prop, "m_TopPainterMaterial");
                PropertyField(prop, "m_LineSmoothStyle");
                PropertyField(prop, "m_LineSmoothness");
                PropertyField(prop, "m_LineSegmentDistance");
                PropertyField(prop, "m_CicleSmoothness");
                PropertyField(prop, "m_AxisMaxSplitNumber");
                PropertyField(prop, "m_LegendIconLineWidth");
                PropertyListField(prop, "m_LegendIconCornerRadius", true);
                --EditorGUI.indentLevel;
            }
        }
    }
}