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
    [CustomPropertyDrawer(typeof(ChartTheme), true)]
    public class ThemeDrawer : BasePropertyDrawer
    {
        private bool m_ThemeModuleToggle = false;
        public override string ClassName { get { return "Theme"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            if (prop.objectReferenceValue == null)
            {
                EditorGUI.ObjectField(pos, prop, new GUIContent("Theme"));
                return;
            }
            base.OnGUI(pos, prop, label);
            var defaultWidth = pos.width;
            var defaultX = pos.x;
            var btnWidth = 45;
            ChartEditorHelper.MakeFoldout(ref m_DrawRect, ref m_ThemeModuleToggle, "Theme");
            m_Heights[m_KeyName] += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_ThemeModuleToggle)
            {
                m_DrawRect.x = defaultX + defaultWidth - 2 * btnWidth - 2;
                m_DrawRect.width = btnWidth;
                var chart = prop.serializedObject.targetObject as BaseChart;
                var lastFont = chart.theme.font;
#if dUI_TextMeshPro
                var lastTMPFont = chart.theme.tmpFont;
#endif
                if (GUI.Button(m_DrawRect, new GUIContent("Reset", "Reset to theme default color")))
                {
                    chart.theme.ResetTheme();
                    chart.RefreshAllComponent();
                }
                m_DrawRect.x = defaultX + defaultWidth - btnWidth;
                m_DrawRect.width = btnWidth;
                if (GUI.Button(m_DrawRect, new GUIContent("Export", "Export theme to asset for a new theme")))
                {
                    ExportThemeWindow.target = chart;
                    EditorWindow.GetWindow(typeof(ExportThemeWindow));
                }

                var data = (ScriptableObject)prop.objectReferenceValue;
                SerializedObject serializedObject = new SerializedObject(data);
                SerializedProperty newProp = serializedObject.GetIterator();
                float y = pos.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                ++EditorGUI.indentLevel;

                var chartNameList = XChartsMgr.GetAllThemeNames();
                var lastIndex = chartNameList.IndexOf(chart.theme.themeName);
                var selectedIndex = EditorGUI.Popup(new Rect(pos.x, y, pos.width, EditorGUIUtility.singleLineHeight),
                    "Theme", lastIndex, chartNameList.ToArray());
                m_Heights[m_KeyName] += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (lastIndex != selectedIndex)
                {
                    GUI.changed = true;
                    XChartsMgr.SwitchTheme(chart, chartNameList[selectedIndex]);
                }
                if (newProp.NextVisible(true))
                {
                    do
                    {
                        if (newProp.name == "m_Script") continue;
                        if (newProp.name == "m_ThemeName") continue;
                        if (newProp.name == "m_Theme") continue;
                        AddPropertyField(pos, newProp, ref y);
                    } while (newProp.NextVisible(false));
                }
                if (GUI.changed)
                {
                    chart.RefreshAllComponent();
                    serializedObject.ApplyModifiedProperties();
                }
                if (chart.theme.font != lastFont)
                {
                    chart.theme.SyncFontToSubComponent();
                }
#if dUI_TextMeshPro
                if (chart.theme.tmpFont != lastTMPFont)
                {
                    chart.theme.SyncTMPFontToSubComponent();
                }
#endif
                --EditorGUI.indentLevel;
            }
        }

        private void AddPropertyField(Rect pos, SerializedProperty prop, ref float y)
        {
            float height = EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
            EditorGUI.PropertyField(new Rect(pos.x, y, pos.width, height), prop, true);
            y += height + EditorGUIUtility.standardVerticalSpacing;
            m_Heights[m_KeyName] += height + EditorGUIUtility.standardVerticalSpacing;
        }
    }

    public class ExportThemeWindow : EditorWindow
    {
        public static BaseChart target;
        private static ExportThemeWindow window;
        private string m_ChartName;
        static void Init()
        {
            window = (ExportThemeWindow)EditorWindow.GetWindow(typeof(ExportThemeWindow), false, "Export Theme", true);
            window.minSize = new Vector2(600, 50);
            window.maxSize = new Vector2(600, 50);
            window.Show();
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            if (target == null)
            {
                Close();
                return;
            }
            GUILayout.Space(10);
            GUILayout.Label("Input a new name for theme:");
            m_ChartName = GUILayout.TextField(m_ChartName);

            GUILayout.Space(10);
            GUILayout.Label("Export path:");
            if (string.IsNullOrEmpty(m_ChartName))
            {
                GUILayout.Label("Need input a new name.");
            }
            else
            {
                GUILayout.Label(XChartsMgr.GetThemeAssetPath(m_ChartName));
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Export"))
            {
                if (string.IsNullOrEmpty(m_ChartName))
                {
                    ShowNotification(new GUIContent("ERROR:Need input a new name!"));
                }
                else if (XChartsMgr.ContainsTheme(m_ChartName))
                {
                    ShowNotification(new GUIContent("ERROR:The name you entered is already in use!"));
                }
                else if (IsAssetsExist(XChartsMgr.GetThemeAssetPath(m_ChartName)))
                {
                    ShowNotification(new GUIContent("ERROR:The asset is exist! \npath="
                        + XChartsMgr.GetThemeAssetPath(m_ChartName)), 5);
                }
                else
                {
                    XChartsMgr.ExportTheme(target.theme, m_ChartName);
                    ShowNotification(new GUIContent("SUCCESS:The theme is exported. \npath="
                        + XChartsMgr.GetThemeAssetPath(m_ChartName)), 5);
                }
            }
        }

        private bool IsAssetsExist(string path)
        {
            return File.Exists(Application.dataPath + "/../" + path);
        }
    }
}