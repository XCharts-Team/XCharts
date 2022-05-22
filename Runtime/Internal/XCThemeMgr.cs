using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts.Runtime
{
    public static class XCThemeMgr
    {
        /// <summary>
        /// 重新加载主题列表
        /// </summary>
        public static void ReloadThemeList()
        {
            XChartsMgr.themes.Clear();
            XChartsMgr.themeNames.Clear();
            AddTheme(LoadTheme(ThemeType.Default));
            AddTheme(LoadTheme(ThemeType.Dark));
            if (XCSettings.Instance != null)
            {
                foreach (var theme in XCSettings.customThemes)
                {
                    AddTheme(theme);
                }
            }
        }

        public static void CheckReloadTheme()
        {
            if (XChartsMgr.themeNames.Count < 0)
                ReloadThemeList();
        }

        public static void AddTheme(Theme theme)
        {
            if (theme == null) return;
            if (!XChartsMgr.themes.ContainsKey(theme.themeName))
            {
                XChartsMgr.themes.Add(theme.themeName, theme);
                XChartsMgr.themeNames.Add(theme.themeName);
                XChartsMgr.themeNames.Sort();
            }
        }

        public static Theme GetTheme(ThemeType type)
        {
            return GetTheme(type.ToString());
        }

        public static Theme GetTheme(string themeName)
        {
            if (!XChartsMgr.themes.ContainsKey(themeName))
            {
                return null;
            }
            return XChartsMgr.themes[themeName];
        }

        public static Theme LoadTheme(ThemeType type)
        {
            return LoadTheme(type.ToString());
        }

        public static Theme LoadTheme(string themeName)
        {
            var theme = Resources.Load<Theme>(XCSettings.THEME_ASSET_NAME_PREFIX + themeName);
            if (theme == null)
                theme = Resources.Load<Theme>(themeName);
            return theme;
        }

        public static List<string> GetAllThemeNames()
        {
            return XChartsMgr.themeNames;
        }

        public static List<Theme> GetThemeList()
        {
            var list = new List<Theme>();
            foreach (var theme in XChartsMgr.themes.Values)
            {
                list.Add(theme);
            }
            return list;
        }

        public static bool ContainsTheme(string themeName)
        {
            return XChartsMgr.themeNames.Contains(themeName);
        }

        public static void SwitchTheme(BaseChart chart, string themeName)
        {
#if UNITY_EDITOR
            if (XChartsMgr.themes.Count == 0)
            {
                ReloadThemeList();
            }
#endif
            if (!XChartsMgr.themes.ContainsKey(themeName))
            {
                Debug.LogError("SwitchTheme ERROR: not exist theme:" + themeName);
                return;
            }
            var target = XChartsMgr.themes[themeName];
            chart.UpdateTheme(target);
        }

        public static bool ExportTheme(Theme theme, string themeNewName)
        {
#if UNITY_EDITOR
            var newtheme = Theme.EmptyTheme;
            newtheme.CopyTheme(theme);
            newtheme.themeType = ThemeType.Custom;
            newtheme.themeName = themeNewName;
            ExportTheme(newtheme);
            return true;
#else
            return false;
#endif
        }

        public static bool ExportTheme(Theme theme)
        {
#if UNITY_EDITOR
            var themeAssetName = XCSettings.THEME_ASSET_NAME_PREFIX + theme.themeName;
            var themeAssetPath = Application.dataPath + "/../" + XCSettings.THEME_ASSET_FOLDER;
            if (!Directory.Exists(themeAssetPath))
            {
                Directory.CreateDirectory(themeAssetPath);
            }
            var themeAssetFilePath = string.Format("{0}/{1}.asset", XCSettings.THEME_ASSET_FOLDER, themeAssetName);
            AssetDatabase.CreateAsset(theme, themeAssetFilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return true;
#else
            return false;
#endif
        }

        public static string GetThemeAssetPath(string themeName)
        {
            return string.Format("{0}/{1}{2}.asset", XCSettings.THEME_ASSET_FOLDER,
                XCSettings.THEME_ASSET_NAME_PREFIX, themeName);
        }
    }
}