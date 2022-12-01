using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(CommentItem), true)]
    public class CommentItemDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "CommentItem"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", "m_Content", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Content");
                PropertyField(prop, "m_Location");
                //PropertyField(prop, "m_MarkRect");
                //PropertyField(prop, "m_MarkStyle");
                PropertyField(prop, "m_LabelStyle");
                --EditorGUI.indentLevel;
            }
        }
    }
}