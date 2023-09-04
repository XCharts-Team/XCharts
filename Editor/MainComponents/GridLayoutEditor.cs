using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(GridLayout))]
    public class GridLayoutEditor : MainComponentEditor<GridLayout>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_Left");
            PropertyField("m_Right");
            PropertyField("m_Top");
            PropertyField("m_Bottom");
            PropertyField("m_Row");
            PropertyField("m_Column");
            PropertyField("m_Spacing");
            PropertyField("m_Inverse");
            --EditorGUI.indentLevel;
        }
    }
}