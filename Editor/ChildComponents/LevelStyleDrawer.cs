using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(LevelStyle), true)]
    public class LevelStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "LevelStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyListField(prop, "m_Levels");
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(Level), true)]
    public class LevelDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Level"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            ++EditorGUI.indentLevel;
            PropertyField(prop, "m_Label");
            PropertyField(prop, "m_UpperLabel");
            PropertyField(prop, "m_ItemStyle");
            --EditorGUI.indentLevel;
        }
    }
}