using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(SymbolStyle), true)]
    public class SymbolStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Symbol"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                var type = (SymbolType) prop.FindPropertyRelative("m_Type").enumValueIndex;
                PropertyField(prop, "m_Type");
                if (type == SymbolType.Custom)
                {
                    PropertyField(prop, "m_Image");
                    PropertyField(prop, "m_ImageType");
                    PropertyField(prop, "m_Width");
                    PropertyField(prop, "m_Height");
                }
                PropertyField(prop, "m_Color");
                PropertyField(prop, "m_Size");
                PropertyField(prop, "m_Gap");
                PropertyField(prop, "m_Offset");
                --EditorGUI.indentLevel;
            }
        }
    }
}