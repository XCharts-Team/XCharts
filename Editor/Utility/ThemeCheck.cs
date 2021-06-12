/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.IO;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    internal static class ThemeCheck
    {
        public class ThemeAssetPostprocessor : AssetPostprocessor
        {
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
                string[] movedFromAssetsPaths)
            {
                foreach (var assetPath in importedAssets)
                {
                    CheckAddedAsset(assetPath);
                }
                foreach (var assetPath in deletedAssets)
                {
                    CheckDeletedAsset(assetPath);
                }
            }
        }

        public static void CheckAddedAsset(string assetPath)
        {
            var fileName = Path.GetFileName(assetPath);
            if (fileName.Equals("XChartsSettings.asset"))
            {
                XThemeMgr.ReloadThemeList();
                return;
            }
            if (!IsThemeAsset(assetPath)) return;
            var obj = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
            if (obj == null || obj.text == null) return;
            if (!obj.text.Contains("m_Theme")) return;
            if (XChartsSettings.AddJsonTheme(obj))
            {
                XThemeMgr.ReloadThemeList();
            }
        }

        public static void CheckDeletedAsset(string assetPath)
        {
            if (!IsThemeAsset(assetPath)) return;
            var themes = XChartsSettings.customThemes;
            var changed = false;

            for (int i = themes.Count - 1; i >= 0; i--)
            {
                if (themes[i] == null)
                {
                    themes.RemoveAt(i);
                    changed = true;
                }
            }
            if (changed)
            {
                XThemeMgr.ReloadThemeList();
            }
        }

        private static bool IsThemeAsset(string assetPath)
        {
            if (!assetPath.EndsWith(".json")) return false;
            var assetName = Path.GetFileNameWithoutExtension(assetPath);
            if (!assetName.StartsWith(XChartsSettings.THEME_ASSET_NAME_PREFIX)) return false;
            return true;
        }
    }
}