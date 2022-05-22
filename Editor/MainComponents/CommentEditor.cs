using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(Comment))]
    public class CommentEditor : MainComponentEditor<Comment>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_LabelStyle");
            //PropertyField("m_MarkStyle");
            PropertyListField("m_Items", true);
            --EditorGUI.indentLevel;
        }
    }
}