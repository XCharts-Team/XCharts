using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(PolarCoord))]
    public class PolarCoordEditor : MainComponentEditor<PolarCoord>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyTwoFiled("m_Center");
            PropertyTwoFiled("m_Radius");
            PropertyField("m_BackgroundColor");
            PropertyField("m_IndicatorLabelOffset");
            --EditorGUI.indentLevel;
        }
    }
}