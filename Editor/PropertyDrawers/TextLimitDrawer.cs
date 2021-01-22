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
    [CustomPropertyDrawer(typeof(TextLimit), true)]
    public class TextLimitDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "TextLimit"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Enable"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_MaxWidth");
                PropertyField(prop, "m_Gap");
                PropertyField(prop, "m_Suffix");
                --EditorGUI.indentLevel;
            }
        }
    }
}