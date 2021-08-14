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
    [CustomPropertyDrawer(typeof(Legend), true)]
    public class LegendDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Legend"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_IconType");
                PropertyField(prop, "m_ItemWidth");
                PropertyField(prop, "m_ItemHeight");
                PropertyField(prop, "m_ItemGap");
                PropertyField(prop, "m_ItemAutoColor");
                PropertyField(prop, "m_TextAutoColor");
                PropertyField(prop, "m_SelectedMode");
                PropertyField(prop, "m_Orient");
                PropertyField(prop, "m_Location");
                PropertyField(prop, "m_Formatter");
                PropertyField(prop, "m_TextStyle");
                PropertyListField(prop, "m_Icons");
                PropertyListField(prop, "m_Data");
                --EditorGUI.indentLevel;
            }
        }
    }
}