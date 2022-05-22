using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(IconStyle), true)]
    public class IconStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "IconStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Layer");
                PropertyField(prop, "m_Align");
                PropertyField(prop, "m_Sprite");
                PropertyField(prop, "m_Type");
                PropertyField(prop, "m_Color");
                PropertyField(prop, "m_Width");
                PropertyField(prop, "m_Height");
                PropertyField(prop, "m_Offset");
                PropertyField(prop, "m_AutoHideWhenLabelEmpty");
                --EditorGUI.indentLevel;
            }
        }
    }
}