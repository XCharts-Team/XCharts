using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(ItemStyle), true)]
    public class ItemStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "ItemStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", false))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Color");
                PropertyField(prop, "m_Color0");
                PropertyField(prop, "m_ToColor");
                PropertyField(prop, "m_ToColor2");
                PropertyField(prop, "m_BackgroundColor");
                PropertyField(prop, "m_BackgroundWidth");
                PropertyField(prop, "m_CenterColor");
                PropertyField(prop, "m_CenterGap");
                PropertyField(prop, "m_BorderWidth");
                PropertyField(prop, "m_BorderGap");
                PropertyField(prop, "m_BorderColor");
                PropertyField(prop, "m_BorderColor0");
                PropertyField(prop, "m_BorderToColor");
                PropertyField(prop, "m_Opacity");
                PropertyField(prop, "m_ItemMarker");
                PropertyField(prop, "m_ItemFormatter");
                PropertyField(prop, "m_NumericFormatter");
                PropertyListField(prop, "m_CornerRadius", true);
                --EditorGUI.indentLevel;
            }
        }
    }
}