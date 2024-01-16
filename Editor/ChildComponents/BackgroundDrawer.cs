using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(Background), true)]
    public class BackgroundDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Background"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Image");
                PropertyField(prop, "m_ImageType");
                PropertyField(prop, "m_ImageColor");
                PropertyField(prop, "m_ImageWidth");
                PropertyField(prop, "m_ImageHeight");
                PropertyField(prop, "m_AutoColor");
                PropertyField(prop, "m_BorderStyle");
                --EditorGUI.indentLevel;
            }
        }
    }
}