using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(BorderStyle), true)]
    public class BorderStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Border"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_BorderWidth");
                PropertyField(prop, "m_BorderColor");
                PropertyField(prop, "m_RoundedCorner");
                PropertyListField(prop, "m_CornerRadius", true);
                --EditorGUI.indentLevel;
            }
        }
    }
}