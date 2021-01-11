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
    [CustomPropertyDrawer(typeof(Polar), true)]
    public class PolarDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Polar"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyTwoFiled(prop, "m_Center");
                PropertyField(prop, "m_Radius");
                PropertyField(prop, "m_BackgroundColor");
                --EditorGUI.indentLevel;
            }
        }
    }
}