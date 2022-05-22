using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(CommentMarkStyle), true)]
    public class CommentMarkStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "MarkStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_LineStyle");
                --EditorGUI.indentLevel;
            }
        }
    }
}