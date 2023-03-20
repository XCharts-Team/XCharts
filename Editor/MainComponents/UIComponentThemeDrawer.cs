using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(UIComponentTheme), true)]
    public class UIComponentThemeDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Theme"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_SharedTheme");
                PropertyField(prop, "m_TransparentBackground");
                --EditorGUI.indentLevel;
            }
        }
    }
}