

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if dUI_TextMeshPro
using TMPro;
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
                    theme.font = GetCustomThemeFont(theme);
#if dUI_TextMeshPro
                    theme.tmpFont = GetCustomThemeTMPFont(theme);
#endif
                    AddTheme(theme);
                }
            }
        }

        private static Font GetCustomThemeFont(ChartTheme theme)
        {
            Font font = null;
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(theme.fontName)) return null;
            if (theme.fontName.Equals("Arial")) return Resources.GetBuiltinResource<Font>("Arial.ttf");
            var guids = AssetDatabase.FindAssets("t:Font");
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var tempFont = AssetDatabase.LoadAssetAtPath<Font>(assetPath);
                if (tempFont.name.Equals(theme.fontName))
                {
                    font = tempFont;
                    break;
                }
            }
#else
            font = FindObjectByInstanceId(theme.fontInstanceId) as Font;
#endif
            return font;
        }

#if dUI_TextMeshPro
         private static TMP_FontAsset GetCustomThemeTMPFont(ChartTheme theme)
        {
            TMP_FontAsset font = null;
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(theme.tmpFontName)){
                //TODO: how to find TMP_FontAsset asset
                var guids = AssetDatabase.FindAssets("t:Texture");
                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if(!assetPath.EndsWith(".asset"))continue;
                    var tempFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(assetPath);
                    if (tempFont && tempFont.name.Equals(theme.tmpFontName))
                    {
                        font = tempFont;
                        break;
                    }
                }
            }
#else
            font = FindObjectByInstanceId(theme.fontInstanceId) as TMP_FontAsset;
#endif
            return font;
        }
#endif

        public static Object FindObjectByInstanceId(int instanceId)
        {
            return (Object)typeof(Object).GetMethod("FindObjectFromInstanceID",
                BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { instanceId });
        }

        public static void AddTheme(ChartTheme theme)
        {
            if (theme == null) return;
            if (!XChartsMgr.Instance.m_ThemeDict.ContainsKey(theme.themeName))
            {
                XChartsMgr.Instance.m_ThemeDict.Add(theme.themeName, theme);
                XChartsMgr.Instance.m_ThemeNames.Add(theme.themeName);
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

        public static List<ChartTheme> GetThemeList()
        {
            var list = new List<ChartTheme>();
            foreach (var theme in XChartsMgr.Instance.m_ThemeDict.Values)
            {
                list.Add(theme);
            }
            return list;
        }

        public static bool ContainsTheme(string themeName)
        {
            return XChartsMgr.Instance.m_ThemeNames.Contains(themeName);
        }

        public static void SwitchTheme(BaseChart chart, string themeName)
        {
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
            chart.UpdateTheme(target);
        }

        public static bool ExportTheme(ChartTheme theme, string themeNewName)
        {
#if UNITY_EDITOR
            var newtheme = ChartTheme.EmptyTheme;
            newtheme.CopyTheme(theme);
            newtheme.theme = Theme.Custom;
            newtheme.themeName = themeNewName;

            ExportTheme(newtheme);
            var themeAssetName = XChartsSettings.THEME_ASSET_NAME_PREFIX + theme.themeName;
            var obj = Resources.Load<TextAsset>(themeAssetName);
            XChartsSettings.AddJsonTheme(obj);
            ReloadThemeList();
            return true;
#else
            return false;
#endif
        }

        public static bool ExportTheme(ChartTheme theme)
        {
#if UNITY_EDITOR
            theme.SyncFontName();
            var themeAssetName = XChartsSettings.THEME_ASSET_NAME_PREFIX + theme.themeName;
            var themeAssetPath = Application.dataPath + "/../" + XChartsSettings.THEME_ASSET_FOLDER;
            if (!Directory.Exists(themeAssetPath))
            {
                Directory.CreateDirectory(themeAssetPath);
            }
            var themeAssetFilePath = string.Format("{0}/{1}.json", themeAssetPath, themeAssetName);
            var json = JsonUtility.ToJson(theme, true);
            File.WriteAllText(themeAssetFilePath, json);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return true;
#else
            return false;
#endif
        }

        public static void ExportAllCustomTheme()
        {
            var list = new List<ChartTheme>();
            foreach (var theme in XChartsMgr.Instance.m_ThemeDict.Values)
            {
                if (theme.theme == Theme.Custom)
                {
                    list.Add(theme);
                }
            }
            foreach (var theme in list)
            {
                ExportTheme(theme);
            }
        }

        public static string GetThemeAssetPath(string themeName)
        {
            return string.Format("{0}/{1}{2}.json", XChartsSettings.THEME_ASSET_FOLDER,
                XChartsSettings.THEME_ASSET_NAME_PREFIX, themeName);
        }
    }
}