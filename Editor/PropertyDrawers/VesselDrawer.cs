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
    [CustomPropertyDrawer(typeof(Vessel), true)]
    public class VesselDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Vessel"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Shape");
                PropertyField(prop, "m_ShapeWidth");
                PropertyField(prop, "m_Gap");
                PropertyTwoFiled(prop, "m_Center");
                PropertyField(prop, "m_Radius");
                PropertyField(prop, "m_BackgroundColor");
                PropertyField(prop, "m_Color");
                PropertyField(prop, "m_AutoColor");
                PropertyField(prop, "m_Smoothness");
                --EditorGUI.indentLevel;
            }
        }
    }
}