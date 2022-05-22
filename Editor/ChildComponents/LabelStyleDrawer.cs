using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(LabelStyle), true)]
    public class LabelStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Label"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Position");
                PropertyField(prop, "m_Formatter");
                PropertyField(prop, "m_NumericFormatter");
                PropertyField(prop, "m_AutoOffset");
                PropertyField(prop, "m_Offset");
                PropertyField(prop, "m_Distance");
                PropertyField(prop, "m_Rotate");
                PropertyField(prop, "m_Width");
                PropertyField(prop, "m_Height");
                PropertyField(prop, "m_Icon");
                PropertyField(prop, "m_Background");
                PropertyField(prop, "m_TextStyle");
                PropertyField(prop, "m_TextPadding");
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(EndLabelStyle), true)]
    public class EndLabelStyleDrawer : LabelStyleDrawer
    {
        public override string ClassName { get { return "End Label"; } }
    }
}