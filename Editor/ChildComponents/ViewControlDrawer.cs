using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(ViewControl), true)]
    public class ViewControlDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "ViewControl"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Alpha");
                PropertyField(prop, "m_Beta");
                --EditorGUI.indentLevel;
            }
        }
    }
}