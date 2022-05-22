using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomEditor(typeof(XCSettings))]
    public class XCSettingsEditor : UnityEditor.Editor
    {
        internal class Styles
        {
            public static readonly GUIContent defaultFontAssetLabel = new GUIContent("Default Font Asset", "The Font Asset that will be assigned by default to newly created text objects when no Font Asset is specified.");
            public static readonly GUIContent defaultFontAssetPathLabel = new GUIContent("Path:        Resources/", "The relative path to a Resources folder where the Font Assets and Material Presets are located.\nExample \"Fonts & Materials/\"");
        }
    }

#if UNITY_2018_3_OR_NEWER
    class XCResourceImporterProvider : SettingsProvider
    {
        XCResourcesImporter m_ResourceImporter;

        public XCResourceImporterProvider() : base("Project/XCharts", SettingsScope.Project)
        { }

        public override void OnGUI(string searchContext)
        {
            if (m_ResourceImporter == null)
                m_ResourceImporter = new XCResourcesImporter();

            m_ResourceImporter.OnGUI();
        }

        public override void OnDeactivate()
        {
            if (m_ResourceImporter != null)
                m_ResourceImporter.OnDestroy();
        }

        static UnityEngine.Object GetSettings()
        {
            return Resources.Load<XCSettings>("XCSettings");
        }

        [SettingsProviderGroup]
        static SettingsProvider[] CreateXCSettingsProvider()
        {
            var providers = new System.Collections.Generic.List<SettingsProvider> { new XCResourceImporterProvider() };
            if (GetSettings() != null)
            {
                var provider = new AssetSettingsProvider("Project/XCharts/Settings", GetSettings);
                provider.PopulateSearchKeywordsFromGUIContentProperties<XCSettingsEditor.Styles>();
                providers.Add(provider);
            }

            return providers.ToArray();
        }
    }
#endif
}