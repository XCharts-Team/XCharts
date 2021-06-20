/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(IconStyle), true)]
    public class IconStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "IconStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Layer");
                PropertyField(prop, "m_Align");
                PropertyField(prop, "m_Sprite");
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