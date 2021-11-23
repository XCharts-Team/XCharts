
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.IO;
using UnityEditor;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts
{
    [CustomEditor(typeof(Theme))]
    public class ThemeEditor : Editor
    {
        private Theme m_Theme;
        void OnEnable()
        {
            m_Theme = target as Theme;
        }

        public override void OnInspectorGUI()
        {
            // serializedObject.Update();
            // EditorGUILayout.PropertyField(m_BackgroundColor);
            // EditorGUILayout.PropertyField(m_ColorPalette);
            // serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
            if (GUILayout.Button(new GUIContent("Reset", "Reset to default theme")))
            {
                m_Theme.ResetTheme();
            }
        }
    }
}