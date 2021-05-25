/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts
{
    [CustomPropertyDrawer(typeof(TextStyle), true)]
    public class TextStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "TextStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, ""))
            {
                ++EditorGUI.indentLevel;
#if dUI_TextMeshPro
                PropertyField(prop, "m_TMPFont");
#else
                PropertyField(prop, "m_Font");
#endif
                PropertyField(prop, "m_Rotate");
                PropertyField(prop, "m_Offset");
                PropertyField(prop, "m_Color");
                PropertyField(prop, "m_BackgroundColor");
                PropertyField(prop, "m_FontSize");
                PropertyField(prop, "m_LineSpacing");
#if dUI_TextMeshPro
                PropertyField(prop, "m_TMPFontStyle");
                PropertyField(prop, "m_TMPAlignment");
#else
                PropertyField(prop, "m_FontStyle");
                PropertyField(prop, "m_Alignment");
                PropertyField(prop, "m_AutoWrap");
                PropertyField(prop, "m_AutoAlign");
#endif
                --EditorGUI.indentLevel;
            }
        }
    }
}