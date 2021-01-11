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
    [CustomPropertyDrawer(typeof(Title), true)]
    public class TitleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Title"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Text");
                PropertyField(prop, "m_SubText");
                PropertyField(prop, "m_ItemGap");
                PropertyField(prop, "m_Location");
                PropertyField(prop, "m_TextStyle");
                PropertyField(prop, "m_SubTextStyle");
                --EditorGUI.indentLevel;
            }
        }
    }
}