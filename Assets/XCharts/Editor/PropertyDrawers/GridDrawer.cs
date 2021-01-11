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
    [CustomPropertyDrawer(typeof(Grid), true)]
    public class GridDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Grid"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Left");
                PropertyField(prop, "m_Right");
                PropertyField(prop, "m_Top");
                PropertyField(prop, "m_Bottom");
                PropertyField(prop, "m_BackgroundColor");
                --EditorGUI.indentLevel;
            }
        }
    }
}