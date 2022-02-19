
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
            if (MakeComponentFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Position");
                PropertyField(prop, "m_Offset");
                PropertyField(prop, "m_AutoOffset");
                PropertyField(prop, "m_AutoColor");
                PropertyField(prop, "m_Distance");
                PropertyField(prop, "m_Formatter");
                PropertyField(prop, "m_NumericFormatter");
                PropertyField(prop, "m_BackgroundWidth");
                PropertyField(prop, "m_BackgroundHeight");
                PropertyField(prop, "m_PaddingLeftRight");
                PropertyField(prop, "m_PaddingTopBottom");
                PropertyField(prop, "m_TextStyle");
                --EditorGUI.indentLevel;
            }
        }
    }
}