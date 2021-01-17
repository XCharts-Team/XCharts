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
    [CustomPropertyDrawer(typeof(VisualMap), true)]
    public class VisualMapDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "VisualMap"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Enable"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Type");
                PropertyField(prop, "m_Direction");
                PropertyField(prop, "m_AutoMinMax");
                PropertyField(prop, "m_Min");
                PropertyField(prop, "m_Max");
                PropertyField(prop, "m_SplitNumber");
                PropertyField(prop, "m_Dimension");
                PropertyListField(prop, "m_InRange");
                PropertyListField(prop, "m_OutOfRange");
                PropertyField(prop, "m_Show");
                if (prop.FindPropertyRelative("m_Show").boolValue)
                {
                    PropertyField(prop, "m_SelectedMode");
                    PropertyTwoFiled(prop, "m_Range");
                    PropertyTwoFiled(prop, "m_Text");
                    PropertyTwoFiled(prop, "m_TextGap");
                    PropertyField(prop, "m_HoverLink");
                    PropertyField(prop, "m_Calculable");
                    PropertyField(prop, "m_ItemWidth");
                    PropertyField(prop, "m_ItemHeight");
                    PropertyField(prop, "m_BorderWidth");
                    PropertyField(prop, "m_Orient");
                    PropertyField(prop, "m_Location");
                }
                --EditorGUI.indentLevel;
            }
        }
    }
}