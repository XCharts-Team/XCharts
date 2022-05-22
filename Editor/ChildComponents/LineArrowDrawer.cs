using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(ArrowStyle), true)]
    public class ArrowDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Arrow"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Width");
                PropertyField(prop, "m_Height");
                PropertyField(prop, "m_Offset");
                PropertyField(prop, "m_Dent");
                PropertyField(prop, "m_Color");
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(LineArrow), true)]
    public class LineArrowStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "LineArrow"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Position");
                PropertyField(prop, "m_Arrow");
                --EditorGUI.indentLevel;
            }
        }
    }
}