using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(ParallelCoord))]
    public class ParallelCoordEditor : MainComponentEditor<ParallelCoord>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_Orient");
            PropertyField("m_Left");
            PropertyField("m_Right");
            PropertyField("m_Top");
            PropertyField("m_Bottom");
            PropertyField("m_BackgroundColor");
            --EditorGUI.indentLevel;
        }
    }
}