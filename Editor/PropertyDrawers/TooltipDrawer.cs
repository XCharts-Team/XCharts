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
    [CustomPropertyDrawer(typeof(Tooltip), true)]
    public class TooltipDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Tooltip"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Type");
                PropertyField(prop, "m_Formatter");
                PropertyField(prop, "m_TitleFormatter");
                PropertyField(prop, "m_ItemFormatter");
                PropertyField(prop, "m_NumericFormatter");
                PropertyField(prop, "m_FixedWidth");
                PropertyField(prop, "m_FixedHeight");
                PropertyField(prop, "m_MinWidth");
                PropertyField(prop, "m_MinHeight");
                PropertyField(prop, "m_PaddingLeftRight");
                PropertyField(prop, "m_PaddingTopBottom");
                PropertyField(prop, "m_BackgroundImage");
                PropertyField(prop, "m_IgnoreDataDefaultContent");
                PropertyField(prop, "m_Offset");
                PropertyField(prop, "m_LineStyle");
                PropertyField(prop, "m_TextStyle");
                --EditorGUI.indentLevel;
            }
        }
    }
}