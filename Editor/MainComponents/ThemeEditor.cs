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
        static class Styles
        {
            internal static GUIContent btnReset = new GUIContent("Reset", "Reset to default theme");
            internal static GUIContent btnSync = new GUIContent("Sync Font", "Sync main theme font to sub theme font");
        }

        private Theme m_Theme;

        void OnEnable()
        {
            m_Theme = target as Theme;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button(Styles.btnReset))
            {
                m_Theme.ResetTheme();
            }
            if (GUILayout.Button(Styles.btnSync))
            {
                m_Theme.SyncFontToSubComponent();
            }
        }
    }
}