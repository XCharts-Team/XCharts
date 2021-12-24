
using UnityEditor;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(GridCoord))]
    public class GridCoordEditor : MainComponentEditor<GridCoord>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_Left");
            PropertyField("m_Right");
            PropertyField("m_Top");
            PropertyField("m_Bottom");
            PropertyField("m_BackgroundColor");
            --EditorGUI.indentLevel;
        }
    }
}