

using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XCharts
{
    public static class XThemeMgr
    {

        /// <summary>
        /// 重新加载主题列表
        /// </summary>
        public static void ReloadThemeList()
        {
            //Debug.Log("LoadThemesFromResources");
            XChartsMgr.Instance.m_ThemeDict.Clear();
            XChartsMgr.Instance.m_ThemeNames.Clear();
            AddTheme(ChartTheme.Default);
            AddTheme(ChartTheme.Light);
            AddTheme(ChartTheme.Dark);
            foreach (var json in XChartsSettings.customThemes)
            {
                if (json != null && !string.IsNullOrEmpty(json.text))
                {
                    var theme = JsonUtility.FromJson<ChartTheme>(json.text);
                    AddTheme(theme);
                }
            }
            //Debug.Log("LoadThemesFromResources DONE: theme count=" + m_ThemeDict.Keys.Count);
        }

        public static void AddTheme(ChartTheme theme)
        {
            if (theme == null) return;
            if (!XChartsMgr.Instance.m_ThemeDict.ContainsKey(theme.themeName))
            {
                XChartsMgr.Instance.m_ThemeDict.Add(theme.themeName, theme);
                XChartsMgr.Instance.m_ThemeNames.Add(theme.themeName);
            }
            else
            {
                Debug.LogError("Theme name is exist:" + theme.themeName);
            }
        }

        public static ChartTheme GetTheme(string themeName)
        {
            if (!XChartsMgr.Instance.m_ThemeDict.ContainsKey(themeName))
            {
                return null;
            }
            return XChartsMgr.Instance.m_ThemeDict[themeName];
        }

        public static List<string> GetAllThemeNames()
        {
            return XChartsMgr.Instance.m_ThemeNames;
        }

        public static bool ContainsTheme(string themeName)
        {
            return XChartsMgr.Instance.m_ThemeNames.Contains(themeName);
        }

        public static void SwitchTheme(BaseChart chart, string themeName)
        {
            Debug.Log("SwitchTheme:" + themeName);
#if UNITY_EDITOR
            if (XChartsMgr.Instance.m_ThemeDict.Count == 0)
            {
                ReloadThemeList();
            }
#endif
            if (!XChartsMgr.Instance.m_ThemeDict.ContainsKey(themeName))
            {
                Debug.LogError("SwitchTheme ERROR: not exist theme:" + themeName);
                return;
            }
            var target = XChartsMgr.Instance.m_ThemeDict[themeName];
            chart.theme.CopyTheme(target);
            chart.RefreshAllComponent();
        }

        public static bool ExportTheme(ChartTheme theme, string themeNewName)
        {
#if UNITY_EDITOR
            var newtheme = ChartTheme.EmptyTheme;
            newtheme.CopyTheme(theme);
            newtheme.theme = Theme.Custom;
            newtheme.themeName = themeNewName;

            var themeFileName = "XTheme-" + newtheme.themeName;
            var assetPath = string.Format("Assets/XCharts/Resources/{0}", themeFileName);
            var filePath = string.Format("{0}/../{1}.json", Application.dataPath, assetPath);
            var json = JsonUtility.ToJson(newtheme, true);
            File.WriteAllText(filePath, json);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            var obj = Resources.Load<TextAsset>(themeFileName);
            XChartsSettings.AddJsonTheme(obj);
            ReloadThemeList();
            return true;
#else
            return false;
#endif
        }

        public static string GetThemeAssetPath(string themeName)
        {
            return string.Format("Assets/XCharts/Resources/XTheme-{0}.json", themeName);
        }
    }
}