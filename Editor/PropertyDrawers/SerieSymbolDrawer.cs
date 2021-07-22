/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(SerieSymbol), true)]
    public class SerieSymbolDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Symbol"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                var type = (SerieSymbolType)prop.FindPropertyRelative("m_Type").enumValueIndex;
                PropertyField(prop, "m_Type");
                if (type == SerieSymbolType.Custom)
                {
                    PropertyField(prop, "m_Image");
                    PropertyField(prop, "m_ImageType");
                    PropertyField(prop, "m_Width");
                    // PropertyField(prop, "m_Height");
                    // PropertyField(prop, "m_Offset");
                }
                PropertyField(prop, "m_Gap");
                PropertyField(prop, "m_SizeType");
                switch ((SerieSymbolSizeType)prop.FindPropertyRelative("m_SizeType").enumValueIndex)
                {
                    case SerieSymbolSizeType.Custom:
                        PropertyField(prop, "m_Size");
                        PropertyField(prop, "m_SelectedSize");
                        break;
                    case SerieSymbolSizeType.FromData:
                        PropertyField(prop, "m_DataIndex");
                        PropertyField(prop, "m_DataScale");
                        PropertyField(prop, "m_SelectedDataScale");
                        break;
                    case SerieSymbolSizeType.Callback:
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