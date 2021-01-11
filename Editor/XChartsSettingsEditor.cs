/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomEditor(typeof(XChartsSettings))]
    public class XChartsSettingsEditor : Editor
    {
        internal class Styles
        {
            public static readonly GUIContent defaultFontAssetLabel = new GUIContent("Default Font Asset", "The Font Asset that will be assigned by default to newly created text objects when no Font Asset is specified.");
            public static readonly GUIContent defaultFontAssetPathLabel = new GUIContent("Path:        Resources/", "The relative path to a Resources folder where the Font Assets and Material Presets are located.\nExample \"Fonts & Materials/\"");
        }
    }

#if UNITY_2018_3_OR_NEWER
    class XChartsResourceImporterProvider : SettingsProvider
    {
        XChartsPackageResourceImporter m_ResourceImporter;

        public XChartsResourceImporterProvider()
            : base("Project/XCharts", SettingsScope.Project)
        {
        }

        public override void OnGUI(string searchContext)
        {
            if (m_ResourceImporter == null)
                m_ResourceImporter = new XChartsPackageResourceImporter();

            m_ResourceImporter.OnGUI();
        }

        public override void OnDeactivate()
        {
            if (m_ResourceImporter != null)
                m_ResourceImporter.OnDestroy();
        }

        static UnityEngine.Object GetSettings()
        {
            return Resources.Load<XChartsSettings>("XChartsSettings");
        }

        [SettingsProviderGroup]
        static SettingsProvider[] CreateXChartsSettingsProvider()
        {
            var providers = new List<SettingsProvider> { new XChartsResourceImporterProvider() };

            if (GetSettings() != null)
            {
                var provider = new AssetSettingsProvider("Project/XCharts/Settings", GetSettings);
                provider.PopulateSearchKeywordsFromGUIContentProperties<XChartsSettingsEditor.Styles>();
                providers.Add(provider);
            }

            return providers.ToArray();
        }
    }
#endif
}