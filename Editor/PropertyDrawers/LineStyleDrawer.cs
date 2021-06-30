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
    [CustomPropertyDrawer(typeof(LineStyle), true)]
    public class LineStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "LineStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Type");
                PropertyField(prop, "m_Color");
                PropertyField(prop, "m_ToColor");
                PropertyField(prop, "m_ToColor2");
                PropertyField(prop, "m_Width");
                PropertyField(prop, "m_Length");
                PropertyField(prop, "m_Opacity");
                --EditorGUI.indentLevel;
            }
        }
    }
}