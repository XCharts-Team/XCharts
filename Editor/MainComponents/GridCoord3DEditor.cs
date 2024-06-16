using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(GridCoord3D))]
    public class GridCoord3DEditor : MainComponentEditor<GridCoord3D>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_Left");
            PropertyField("m_Bottom");
            PropertyField("m_BoxWidth");
            PropertyField("m_BoxHeight");
            PropertyField("m_BoxDepth");
            PropertyField("m_XYExchanged");
            PropertyField("m_ShowBorder");
            PropertyField("m_ViewControl");
            --EditorGUI.indentLevel;
        }
    }
}