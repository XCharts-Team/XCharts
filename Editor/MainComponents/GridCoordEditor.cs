using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(GridCoord))]
    public class GridCoordEditor : MainComponentEditor<GridCoord>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            var layoutIndex = baseProperty.FindPropertyRelative("m_LayoutIndex").intValue;
            PropertyField("m_LayoutIndex");
            PropertyField("m_Left");
            PropertyField("m_Right");
            PropertyField("m_Top");
            PropertyField("m_Bottom");
            PropertyField("m_BackgroundColor");
            PropertyField("m_ShowBorder");
            PropertyField("m_BorderWidth");
            PropertyField("m_BorderColor");
            --EditorGUI.indentLevel;
        }
    }
}