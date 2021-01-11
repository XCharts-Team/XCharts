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
    [CustomPropertyDrawer(typeof(Arrow), true)]
    public class ArrowDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Arrow"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, ""))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Width");
                PropertyField(prop, "m_Height");
                PropertyField(prop, "m_Offset");
                PropertyField(prop, "m_Dent");
                PropertyField(prop, "m_Color");
                --EditorGUI.indentLevel;
            }
        }
    }
    
    [CustomPropertyDrawer(typeof(LineArrow), true)]
    public class LineArrowStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "LineArrow"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Position");
                PropertyField(prop, "m_Arrow");
                --EditorGUI.indentLevel;
            }
        }
    }
}