using System.IO;
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    internal static class XChartsDaemon
    {
        public class XChartsAssetPostprocessor : AssetPostprocessor
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
            if (fileName.Equals("XCSettings.asset"))
            {
                CheckAsmdef();
                XCThemeMgr.ReloadThemeList();
            }
            else if (IsThemeAsset(assetPath))
            {
                var theme = AssetDatabase.LoadAssetAtPath<Theme>(assetPath);
                if (XCSettings.AddCustomTheme(theme))
                {
                    XCThemeMgr.ReloadThemeList();
                }
            }
        }

        public static void CheckAsmdef()
        {
#if UNITY_2017_1_OR_NEWER
#if dUI_TextMeshPro
            XChartsEditor.CheckAsmdefTmpReference(true);
#else
            XChartsEditor.CheckAsmdefTmpReference(false);
#endif
#elif UNITY_2019_1_OR_NEWER
#if INPUT_SYSTEM_ENABLED
            XChartsEditor.CheckAsmdefInputSystemReference(true);
#else
            XChartsEditor.CheckAsmdefInputSystemReference(false);
#endif
#endif
        }

        public static void CheckDeletedAsset(string assetPath)
        {
            if (!IsThemeAsset(assetPath)) return;
            if (XCSettings.Instance == null) return;
            var themes = XCSettings.customThemes;
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
                XCThemeMgr.ReloadThemeList();
            }
        }

        private static bool IsThemeAsset(string assetPath)
        {
            if (!assetPath.EndsWith(".asset")) return false;
            var assetName = Path.GetFileNameWithoutExtension(assetPath);
            if (!assetName.StartsWith(XCSettings.THEME_ASSET_NAME_PREFIX)) return false;
            return true;
        }
    }
}