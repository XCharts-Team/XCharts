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
        public override string ClassName { get { return "Theme"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            var defaultWidth = pos.width;
            var defaultX = pos.x;
            var btnWidth = 50;
            if (MakeFoldout(prop, ""))
            {
                var btnRect = new Rect(m_DrawRect);
                btnRect.x = defaultX + defaultWidth - 2 * btnWidth - 2;
                btnRect.y = m_DrawRect.y - EditorGUIUtility.singleLineHeight - 3;
                btnRect.width = btnWidth;
                var chart = prop.serializedObject.targetObject as BaseChart;
                if (GUI.Button(btnRect, new GUIContent("Reset", "Reset to theme default color")))
                {
                    chart.theme.ResetTheme();
                    chart.RefreshAllComponent();
                }
                btnRect.x = defaultX + defaultWidth - btnWidth;
                btnRect.width = btnWidth;
                if (GUI.Button(btnRect, new GUIContent("Export", "Export theme to asset for a new theme")))
                {
                    ExportThemeWindow.target = chart;
                    EditorWindow.GetWindow(typeof(ExportThemeWindow));
                }
                ++EditorGUI.indentLevel;
                var chartNameList = XThemeMgr.GetAllThemeNames();
                var lastIndex = chartNameList.IndexOf(chart.theme.themeName);
                var y = pos.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                var selectedIndex = EditorGUI.Popup(new Rect(pos.x, y, pos.width, EditorGUIUtility.singleLineHeight),
                    "Theme", lastIndex, chartNameList.ToArray());
                AddSingleLineHeight();
                if (lastIndex != selectedIndex)
                {
                    XThemeMgr.SwitchTheme(chart, chartNameList[selectedIndex]);
                }
#if dUI_TextMeshPro
                PropertyField(prop, "m_TMPFont");
                if(chart.theme.tmpFont == null && !string.IsNullOrEmpty(chart.theme.tmpFontName))
                {
                    var msg = string.Format("Can't find theme font asset:{0} in project.", chart.theme.tmpFontName);
                    EditorGUILayout.HelpBox(msg, MessageType.Error);
                }
#else
                PropertyField(prop, "m_Font");
                if (chart.theme.font == null && !string.IsNullOrEmpty(chart.theme.fontName))
                {
                    var msg = string.Format("Can't find theme font asset:{0} in project.", chart.theme.fontName);
                    EditorGUILayout.HelpBox(msg, MessageType.Error);
                }
#endif
                PropertyField(prop, "m_ContrastColor");
                PropertyField(prop, "m_BackgroundColor");
                PropertyField(prop, "m_ColorPalette");
                PropertyField(prop, "m_Common");
                PropertyField(prop, "m_Title");
                PropertyField(prop, "m_SubTitle");
                PropertyField(prop, "m_Legend");
                PropertyField(prop, "m_Axis");
                PropertyField(prop, "m_RadiusAxis");
                PropertyField(prop, "m_AngleAxis");
                PropertyField(prop, "m_Polar");
                PropertyField(prop, "m_Gauge");
                PropertyField(prop, "m_Radar");
                PropertyField(prop, "m_Tooltip");
                PropertyField(prop, "m_DataZoom");
                PropertyField(prop, "m_VisualMap");
                PropertyField(prop, "m_Serie");
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
                GUILayout.Label(XThemeMgr.GetThemeAssetPath(m_ChartName));
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Export"))
            {
                if (string.IsNullOrEmpty(m_ChartName))
                {
                    ShowNotification(new GUIContent("ERROR:Need input a new name!"));
                }
                else if (XThemeMgr.ContainsTheme(m_ChartName))
                {
                    ShowNotification(new GUIContent("ERROR:The name you entered is already in use!"));
                }
                else if (IsAssetsExist(XThemeMgr.GetThemeAssetPath(m_ChartName)))
                {
                    ShowNotification(new GUIContent("ERROR:The asset is exist! \npath="
                        + XThemeMgr.GetThemeAssetPath(m_ChartName)));
                }
                else
                {
                    XThemeMgr.ExportTheme(target.theme, m_ChartName);
                    ShowNotification(new GUIContent("SUCCESS:The theme is exported. \npath="
                        + XThemeMgr.GetThemeAssetPath(m_ChartName)));
                }
            }
        }

        private bool IsAssetsExist(string path)
        {
            return File.Exists(Application.dataPath + "/../" + path);
        }
    }
}