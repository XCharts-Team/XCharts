

using UnityEditor;
using UnityEngine;
using XCharts.Runtime;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts.Editor
{
    [CustomEditor(typeof(Theme))]
    public class ThemeEditor : UnityEditor.Editor
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