using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(AreaStyle), true)]
    public class AreaStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "AreaStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Origin");
                PropertyField(prop, "m_Color");
                PropertyField(prop, "m_ToColor");
                PropertyField(prop, "m_HighlightColor");
                PropertyField(prop, "m_HighlightToColor");
                PropertyField(prop, "m_Opacity");
                --EditorGUI.indentLevel;
            }
        }
    }
}