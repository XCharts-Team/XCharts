using System.IO;
using UnityEditor;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(ThemeStyle), true)]
    public class ThemeStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Theme"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            var defaultWidth = pos.width;
            var defaultX = pos.x;
            var chart = prop.serializedObject.targetObject as BaseChart;
            if (MakeComponentFoldout(prop, "m_Show", false, new HeaderMenuInfo("Reset|Reset to theme default color", () =>
                {
                    chart.theme.sharedTheme.ResetTheme();
                    chart.RefreshAllComponent();
                }), new HeaderMenuInfo("Export|Export theme to asset for a new theme", () =>
                {
                    ExportThemeWindow.target = chart;
                    EditorWindow.GetWindow(typeof(ExportThemeWindow));
                }), new HeaderMenuInfo("Sync color to custom|Sync shared theme color to custom color", () =>
                {
                    chart.theme.SyncSharedThemeColorToCustom();
                })))
            {
                ++EditorGUI.indentLevel;
                var chartNameList = XCThemeMgr.GetAllThemeNames();
                var lastIndex = chartNameList.IndexOf(chart.theme.themeName);
                var y = pos.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                var selectedIndex = EditorGUI.Popup(new Rect(pos.x, y, pos.width, EditorGUIUtility.singleLineHeight),
                    "Shared Theme", lastIndex, chartNameList.ToArray());
                AddSingleLineHeight();
                if (lastIndex != selectedIndex)
                {
                    XCThemeMgr.SwitchTheme(chart, chartNameList[selectedIndex]);
                }
                PropertyField(prop, "m_SharedTheme");
                PropertyField(prop, "m_TransparentBackground");
                PropertyField(prop, "m_EnableCustomTheme");
                using(new EditorGUI.DisabledScope(!prop.FindPropertyRelative("m_EnableCustomTheme").boolValue))
                    {
                        PropertyField(prop, "m_CustomBackgroundColor");
                        PropertyField(prop, "m_CustomColorPalette");
                    }
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

    public class ExportThemeWindow : UnityEditor.EditorWindow
    {
        public static BaseChart target;
        private static ExportThemeWindow window;
        private string m_ChartName;
        static void Init()
        {
            window = (ExportThemeWindow) EditorWindow.GetWindow(typeof(ExportThemeWindow), false, "Export Theme", true);
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
                GUILayout.Label(XCThemeMgr.GetThemeAssetPath(m_ChartName));
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Export"))
            {
                if (string.IsNullOrEmpty(m_ChartName))
                {
                    ShowNotification(new GUIContent("ERROR:Need input a new name!"));
                }
                else if (XCThemeMgr.ContainsTheme(m_ChartName))
                {
                    ShowNotification(new GUIContent("ERROR:The name you entered is already in use!"));
                }
                else if (IsAssetsExist(XCThemeMgr.GetThemeAssetPath(m_ChartName)))
                {
                    ShowNotification(new GUIContent("ERROR:The asset is exist! \npath=" +
                        XCThemeMgr.GetThemeAssetPath(m_ChartName)));
                }
                else
                {
                    XCThemeMgr.ExportTheme(target.theme.sharedTheme, m_ChartName);
                    ShowNotification(new GUIContent("SUCCESS:The theme is exported. \npath=" +
                        XCThemeMgr.GetThemeAssetPath(m_ChartName)));
                }
            }
        }

        private bool IsAssetsExist(string path)
        {
            return File.Exists(Application.dataPath + "/../" + path);
        }
    }
}