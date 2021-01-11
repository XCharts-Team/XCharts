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
    [CustomPropertyDrawer(typeof(Background), true)]
    public class BackgroundDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Background"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Image");
                PropertyField(prop, "m_ImageType");
                PropertyField(prop, "m_ImageColor");
                PropertyField(prop, "m_HideThemeBackgroundColor");
                --EditorGUI.indentLevel;
            }
        }
    }
}