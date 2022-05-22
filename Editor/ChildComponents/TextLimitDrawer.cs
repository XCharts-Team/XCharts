using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(TextLimit), true)]
    public class TextLimitDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "TextLimit"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Enable", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_MaxWidth");
                PropertyField(prop, "m_Gap");
                PropertyField(prop, "m_Suffix");
                --EditorGUI.indentLevel;
            }
        }
    }
}