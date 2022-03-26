
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomEditor(typeof(XCSettings))]
    public class XCSettingsEditor : UnityEditor.Editor
    {
    }

#if UNITY_2018_3_OR_NEWER
    class XCResourceImporterProvider : SettingsProvider
    {
        XCResourcesImporter m_ResourceImporter;

        public XCResourceImporterProvider()
            : base("Project/XCharts", SettingsScope.Project)
        {
        }

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
            var isExist = File.Exists("Assets/XCharts/Resources/XCSettings.asset");
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