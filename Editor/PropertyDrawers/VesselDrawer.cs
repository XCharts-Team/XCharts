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
                var shape = (Vessel.Shape)prop.FindPropertyRelative("m_Shape").intValue;
                PropertyField(prop, "m_Shape");
                PropertyField(prop, "m_ShapeWidth");
                PropertyField(prop, "m_Gap");
                PropertyTwoFiled(prop, "m_Center");
                PropertyField(prop, "m_BackgroundColor");
                PropertyField(prop, "m_Color");
                PropertyField(prop, "m_AutoColor");
                switch (shape)
                {
                    case Vessel.Shape.Circle:
                        PropertyField(prop, "m_Radius");
                        PropertyField(prop, "m_Smoothness");
                        break;
                    case Vessel.Shape.Rect:
                        PropertyField(prop, "m_Width");
                        PropertyField(prop, "m_Height");
                        PropertyField(prop, "m_CornerRadius");
                        break;
                }
                --EditorGUI.indentLevel;
            }
        }
    }
}