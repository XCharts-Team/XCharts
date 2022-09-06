using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(SerieSymbol), true)]
    public class SerieSymbolDrawer : BasePropertyDrawer
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
                    // PropertyField(prop, "m_Height");
                    // PropertyField(prop, "m_Offset");
                }
                PropertyField(prop, "m_Gap");
                PropertyField(prop, "m_SizeType");
                switch ((SymbolSizeType) prop.FindPropertyRelative("m_SizeType").enumValueIndex)
                {
                    case SymbolSizeType.Custom:
                        PropertyField(prop, "m_Size");
                        break;
                    case SymbolSizeType.FromData:
                        PropertyField(prop, "m_DataIndex");
                        PropertyField(prop, "m_DataScale");
                        PropertyField(prop, "m_MinSize");
                        PropertyField(prop, "m_MaxSize");
                        break;
                    case SymbolSizeType.Function:
                        break;
                }
                PropertyField(prop, "m_StartIndex");
                PropertyField(prop, "m_Interval");
                PropertyField(prop, "m_ForceShowLast");
                PropertyField(prop, "m_Repeat");
                --EditorGUI.indentLevel;
            }
        }
    }
}