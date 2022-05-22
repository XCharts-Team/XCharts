using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(ImageStyle), true)]
    public class ImageStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "ImageStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Sprite");
                PropertyField(prop, "m_Type");
                PropertyField(prop, "m_AutoColor");
                PropertyField(prop, "m_Color");
                PropertyField(prop, "m_Width");
                PropertyField(prop, "m_Height");
                --EditorGUI.indentLevel;
            }
        }
    }
}