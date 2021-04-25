/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(ItemStyle), true)]
    public class ItemStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "ItemStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
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
                PropertyField(prop, "m_BorderType");
                PropertyField(prop, "m_BorderWidth");
                PropertyField(prop, "m_BorderColor");
                PropertyField(prop, "m_BorderColor0");
                PropertyField(prop, "m_BorderToColor");
                PropertyField(prop, "m_Opacity");
                PropertyField(prop, "m_TooltipFormatter");
                PropertyField(prop, "m_NumericFormatter");
                PropertyListField(prop, "m_CornerRadius", true);
                --EditorGUI.indentLevel;
            }
        }
    }
}