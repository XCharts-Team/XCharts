using UnityEditor;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(TextStyle), true)]
    public class TextStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "TextStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
#if dUI_TextMeshPro
                PropertyField(prop, "m_TMPFont");
#else
                PropertyField(prop, "m_Font");
#endif
                PropertyField(prop, "m_Rotate");
                PropertyField(prop, "m_AutoColor");
                PropertyField(prop, "m_Color");
                PropertyField(prop, "m_FontSize");
                PropertyField(prop, "m_LineSpacing");
#if dUI_TextMeshPro
                PropertyField(prop, "m_TMPFontStyle");
                PropertyField(prop, "m_TMPSpriteAsset");
                PropertyField(prop, "m_TMPAlignment");
#else
                PropertyField(prop, "m_FontStyle");
                PropertyField(prop, "m_Alignment");
                PropertyField(prop, "m_AutoAlign");
                PropertyField(prop, "m_AutoWrap");
#endif
                --EditorGUI.indentLevel;
            }
        }
    }
}