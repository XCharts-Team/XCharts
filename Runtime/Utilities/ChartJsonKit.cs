using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts.Runtime
{
    // ════════════════════════════════════════════════════════════════════════════════
    // DTO Definitions
    // ════════════════════════════════════════════════════════════════════════════════

    /// <summary>Root DTO for chart JSON export/import.</summary>
    [Serializable]
    public class ChartJson
    {
        public string schemaVersion = "1.1";
        public string chartType;
        public string chartVersion;
        public string exportedAt;
        public List<ComponentJson> components = new List<ComponentJson>();
        public List<SerieJson> series = new List<SerieJson>();
        public ThemeSnapshotJson theme;
        public ChartSettingsJson settings;
    }

    [Serializable]
    public class ComponentJson
    {
        /// <summary>Type name (simplified, e.g. "Background", "Tooltip").</summary>
        public string type;
        public bool enabled = true;
        /// <summary>JSON-serialized component fields (via JsonUtility).</summary>
        public string data;
    }

    [Serializable]
    public class SerieJson
    {
        /// <summary>Type name (simplified, e.g. "Line", "Bar").</summary>
        public string type;
        public int index;
        public bool enabled = true;
        /// <summary>JSON-serialized serie fields (via JsonUtility).</summary>
        public string data;
    }

    [Serializable]
    public class ThemeSnapshotJson
    {
        public string themeName;
        public int themeType;
        public string data;
    }

    [Serializable]
    public class ChartSettingsJson
    {
        public string chartName;
        public bool useUtc;
        public float width;
        public float height;
        public string data;
    }

    // ════════════════════════════════════════════════════════════════════════════════
    // Serializer: BaseChart → JSON string
    // ════════════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Exports a BaseChart instance to a portable JSON string (ChartJson schema v1.0).
    /// Only serialized fields are exported; runtime-only / [NonSerialized] fields are skipped.
    /// </summary>
    public static class ChartJsonSerializer
    {
        /// <summary>
        /// Serialize <paramref name="chart"/> to a JSON string.
        /// </summary>
        public static string Serialize(BaseChart chart, bool prettyPrint = true)
        {
            if (chart == null) throw new ArgumentNullException("chart");

            EnsureChartRuntimeLists(chart);

            var dto = new ChartJson
            {
                schemaVersion = "1.1",
                chartType = chart.GetType().Name,
                chartVersion = GetChartVersion(chart),
                exportedAt = DateTime.UtcNow.ToString("o"),
                theme = SerializeTheme(chart.theme),
                settings = new ChartSettingsJson
                {
                    chartName = chart.chartName,
                    useUtc = chart.useUtc,
                    width = chart.chartWidth,
                    height = chart.chartHeight,
                    data = SerializeSettings(chart)
                }
            };

            var components = CollectComponents(chart);
            var series = CollectSeries(chart);

            foreach (var comp in components)
            {
                var compJson = SerializeComponent(comp);
                if (compJson != null)
                    dto.components.Add(compJson);
            }

            foreach (var serie in series)
            {
                var serieJson = SerializeSerie(serie);
                if (serieJson != null)
                    dto.series.Add(serieJson);
            }

            var json = JsonUtility.ToJson(dto, prettyPrint);
            json = ChartJsonDataFieldCodec.ConvertEscapedDataStringToRawObject(json);
            if (prettyPrint)
            {
                object parsedJson;
                if (SimpleJson.TryParse(json, out parsedJson))
                    json = SimpleJson.Stringify(parsedJson, true);
            }
            return json;
        }

        // ─── Component ────────────────────────────────────────────────────────

        private static ComponentJson SerializeComponent(MainComponent comp)
        {
            if (comp == null) return null;
            try
            {
                var defaultComp = CreateDefaultInstance(comp.GetType()) as MainComponent;
                var dataJson = JsonDiffPruner.PruneDefaults(JsonUtility.ToJson(comp), JsonUtility.ToJson(defaultComp));
                return new ComponentJson
                {
                    type = comp.GetType().Name,
                    enabled = true,
                    data = dataJson
                };
            }
            catch (Exception ex)
            {
                Debug.LogWarning(string.Format("[XCharts] ChartJsonSerializer: Failed to serialize component {0}: {1}", comp.GetType().Name, ex.Message));
                return null;
            }
        }

        // ─── Serie ────────────────────────────────────────────────────────────

        private static SerieJson SerializeSerie(Serie serie)
        {
            if (serie == null) return null;
            try
            {
                var defaultSerie = CreateDefaultInstance(serie.GetType()) as Serie;
                var dataJson = JsonDiffPruner.PruneDefaults(JsonUtility.ToJson(serie), JsonUtility.ToJson(defaultSerie));
                dataJson = PruneSerieDataDefaults(dataJson);
                return new SerieJson
                {
                    type = serie.GetType().Name,
                    index = serie.index,
                    enabled = serie.show,
                    data = dataJson
                };
            }
            catch (Exception ex)
            {
                Debug.LogWarning(string.Format("[XCharts] ChartJsonSerializer: Failed to serialize serie {0}: {1}", serie.GetType().Name, ex.Message));
                return null;
            }
        }

        private static string PruneSerieDataDefaults(string serieJson)
        {
            if (string.IsNullOrEmpty(serieJson)) return serieJson;

            object serieObj;
            if (!SimpleJson.TryParse(serieJson, out serieObj))
                return serieJson;

            var serieDict = serieObj as Dictionary<string, object>;
            if (serieDict == null)
                return serieJson;

            object rawDataList;
            if (!serieDict.TryGetValue("m_Data", out rawDataList))
                return serieJson;

            var dataList = rawDataList as List<object>;
            if (dataList == null || dataList.Count == 0)
                return serieJson;

            object defaultSerieDataObj;
            if (!SimpleJson.TryParse(JsonUtility.ToJson(new SerieData()), out defaultSerieDataObj))
                return serieJson;

            var prunedList = new List<object>();
            for (int i = 0; i < dataList.Count; i++)
            {
                var item = dataList[i];
                var prunedItem = JsonDiffPruner.PruneParsedDefaults(item, defaultSerieDataObj);
                if (prunedItem != null)
                    prunedList.Add(prunedItem);
                else
                    prunedList.Add(item);
            }
            serieDict["m_Data"] = prunedList;
            return SimpleJson.Stringify(serieDict);
        }

        // ─── Theme ────────────────────────────────────────────────────────────

        private static ThemeSnapshotJson SerializeTheme(ThemeStyle themeStyle)
        {
            if (themeStyle == null) return new ThemeSnapshotJson();

            var theme = themeStyle.sharedTheme;
            var snapshot = new ThemeSnapshotJson
            {
                themeName = theme != null ? theme.themeName : "Default",
                themeType = theme != null ? (int)theme.themeType : 0,
                data = SerializeThemeStyleData(themeStyle)
            };

            return snapshot;
        }

        private static string SerializeThemeStyleData(ThemeStyle themeStyle)
        {
            if (themeStyle == null) return "{}";
            try
            {
                var defaultThemeStyle = new ThemeStyle();
                return JsonDiffPruner.PruneDefaults(JsonUtility.ToJson(themeStyle), JsonUtility.ToJson(defaultThemeStyle));
            }
            catch
            {
                return JsonUtility.ToJson(themeStyle);
            }
        }

        // ─── Helpers ──────────────────────────────────────────────────────────

        private static string GetChartVersion(BaseChart chart)
        {
            string moduleVersion;
            string coreVersion = XChartsMgr.version;
            string moduleName = GetModuleNameFromAssembly(chart == null ? null : chart.GetType());
            if (!TryGetVersionFromModulePackageJson(moduleName, out moduleVersion))
            {
                // No module-specific package found: fallback to core package version
                if (!TryGetVersionFromCorePackageJson(out moduleVersion))
                    moduleVersion = coreVersion;
            }
            return coreVersion + "/" + moduleVersion;
        }

        private static string GetModuleNameFromAssembly(Type chartType)
        {
            if (chartType == null) return string.Empty;
            string asmName = chartType.Assembly.GetName().Name;
            if (string.IsNullOrEmpty(asmName)) return string.Empty;
            if (asmName == "XCharts.Runtime") return string.Empty;

            const string prefix = "XCharts.";
            const string suffix = ".Runtime";
            if (asmName.StartsWith(prefix, StringComparison.Ordinal) && asmName.EndsWith(suffix, StringComparison.Ordinal))
            {
                int start = prefix.Length;
                int len = asmName.Length - prefix.Length - suffix.Length;
                if (len > 0)
                    return asmName.Substring(start, len);
            }
            return string.Empty;
        }

        private static bool TryGetVersionFromModulePackageJson(string moduleName, out string version)
        {
            version = null;
            if (string.IsNullOrEmpty(moduleName))
                return false;

            var candidates = new List<string>();
            string projectRoot = Directory.GetCurrentDirectory();

            // embedded/source mode
            candidates.Add(Path.Combine(Path.Combine(Path.Combine(projectRoot, "Assets"), "XCharts-" + moduleName), "package.json"));

            // package manager mode (known naming convention)
            string modulePkgName = "com.monitor1394.xcharts." + moduleName.ToLower();
            candidates.Add(Path.Combine(Path.Combine(Path.Combine(projectRoot, "Packages"), modulePkgName), "package.json"));

#if UNITY_EDITOR
            // If core package root is known, also probe sibling package folders
            try
            {
                string coreRoot = XChartsMgr.GetPackageFullPath();
                if (!string.IsNullOrEmpty(coreRoot))
                {
                    string parent = Path.GetDirectoryName(coreRoot);
                    if (!string.IsNullOrEmpty(parent))
                        candidates.Add(Path.Combine(Path.Combine(parent, modulePkgName), "package.json"));
                }
            }
            catch { }
#endif

            for (int i = 0; i < candidates.Count; i++)
            {
                if (TryReadVersionFromPackageJson(candidates[i], out version))
                    return true;
            }
            return false;
        }

        private static bool TryGetVersionFromCorePackageJson(out string version)
        {
            version = null;
            var candidates = new List<string>();
            string projectRoot = Directory.GetCurrentDirectory();

            // embedded/source mode
            candidates.Add(Path.Combine(Path.Combine(Path.Combine(projectRoot, "Assets"), "XCharts"), "package.json"));

            // package manager mode
            candidates.Add(Path.Combine(Path.Combine(Path.Combine(projectRoot, "Packages"), "com.monitor1394.xcharts"), "package.json"));

#if UNITY_EDITOR
            try
            {
                string coreRoot = XChartsMgr.GetPackageFullPath();
                if (!string.IsNullOrEmpty(coreRoot))
                    candidates.Add(Path.Combine(coreRoot, "package.json"));
            }
            catch { }
#endif

            for (int i = 0; i < candidates.Count; i++)
            {
                if (TryReadVersionFromPackageJson(candidates[i], out version))
                    return true;
            }
            return false;
        }

        private static bool TryReadVersionFromPackageJson(string packageJsonPath, out string version)
        {
            version = null;
            try
            {
                if (string.IsNullOrEmpty(packageJsonPath) || !File.Exists(packageJsonPath))
                    return false;
                string content = File.ReadAllText(packageJsonPath);
                var match = Regex.Match(content, "\"version\"\\s*:\\s*\"([^\"]+)\"");
                if (!match.Success)
                    return false;
                version = match.Groups[1].Value;
                return !string.IsNullOrEmpty(version);
            }
            catch
            {
                return false;
            }
        }

        private static string ColorToHex(Color32 color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.r, color.g, color.b, color.a);
        }

        private static string SerializeSettings(BaseChart chart)
        {
            if (chart == null || chart.settings == null)
                return "{}";
            try
            {
                var defaultSettings = CreateDefaultInstance(typeof(Settings)) as Settings;
                if (defaultSettings == null)
                    return JsonUtility.ToJson(chart.settings);
                return JsonDiffPruner.PruneDefaults(JsonUtility.ToJson(chart.settings), JsonUtility.ToJson(defaultSettings));
            }
            catch
            {
                return JsonUtility.ToJson(chart.settings);
            }
        }


        private static void EnsureChartRuntimeLists(BaseChart chart)
        {
            if (chart == null) return;
            if (chart.series != null && chart.series.Count > 0) return;
            try
            {
                chart.OnAfterDeserialize();
            }
            catch { }
        }

        private static List<MainComponent> CollectComponents(BaseChart chart)
        {
            var result = new List<MainComponent>();
            if (chart.components != null && chart.components.Count > 0)
            {
                result.AddRange(chart.components);
                return result;
            }

            foreach (var kv in chart.typeListForComponent)
            {
                var field = kv.Value;
                var count = ReflectionUtil.InvokeListCount(chart, field);
                for (int i = 0; i < count; i++)
                {
                    var comp = ReflectionUtil.InvokeListGet<MainComponent>(chart, field, i);
                    if (comp != null)
                        result.Add(comp);
                }
            }
            result.Sort();
            return result;
        }

        private static List<Serie> CollectSeries(BaseChart chart)
        {
            var result = new List<Serie>();
            if (chart.series != null && chart.series.Count > 0)
            {
                result.AddRange(chart.series);
                return result;
            }

            foreach (var kv in chart.typeListForSerie)
            {
                var field = kv.Value;
                var count = ReflectionUtil.InvokeListCount(chart, field);
                for (int i = 0; i < count; i++)
                {
                    var serie = ReflectionUtil.InvokeListGet<Serie>(chart, field, i);
                    if (serie != null)
                        result.Add(serie);
                }
            }
            result.Sort();
            return result;
        }

        private static object CreateDefaultInstance(Type type)
        {
            if (type == null) return null;
            object instance = null;
            try
            {
                instance = Activator.CreateInstance(type);
                var method = type.GetMethod("SetDefaultValue", BindingFlags.Public | BindingFlags.Instance);
                if (method != null)
                    method.Invoke(instance, new object[] { });
            }
            catch
            {
                // fall back to raw Activator result or null
            }
            return instance;
        }
    }

    // ════════════════════════════════════════════════════════════════════════════════
    // Deserializer: JSON string → BaseChart
    // ════════════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Imports a ChartJson (v1.0) into an existing BaseChart instance or creates a new chart.
    /// </summary>
    public static class ChartJsonDeserializer
    {
        private const string LOG_TAG = "[XCharts] ChartJsonDeserializer";

        /// <summary>
        /// Deserialize <paramref name="json"/> and apply the configuration to <paramref name="chart"/>.
        /// Existing components/series are updated or replaced. Calls Refresh at the end.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when json is invalid or schema version is unsupported.</exception>
        public static void Deserialize(string json, BaseChart chart)
        {
            if (string.IsNullOrEmpty(json)) throw new ArgumentNullException("json");
            if (chart == null) throw new ArgumentNullException("chart");

            EnsureTypeMapsInitialized(chart);

            // Support both formats:
            // 1) legacy: "data":"{...escaped...}"
            // 2) v1.1+:   "data":{...raw object...}
            json = ChartJsonDataFieldCodec.ConvertRawObjectDataToEscapedString(json);

            ChartJson dto;
            try
            {
                dto = JsonUtility.FromJson<ChartJson>(json);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Invalid JSON format: {0}", ex.Message), ex);
            }

            if (dto == null || string.IsNullOrEmpty(dto.schemaVersion))
                throw new ArgumentException("JSON does not appear to be a valid XCharts chart export (missing schemaVersion).");

            ValidateSchema(dto);

            // Apply components
            ImportComponents(dto.components, chart);

            // Apply series
            ImportSeries(dto.series, chart);

            // Apply chart base fields/settings
            if (dto.settings != null)
                ImportSettings(dto.settings, chart);

            // Apply theme
            if (dto.theme != null)
                ImportTheme(dto.theme, chart);

            chart.RefreshChart();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(chart);
#endif
            Debug.Log(string.Format("{0}: Import complete - {1} component(s), {2} serie(s).", LOG_TAG, dto.components.Count, dto.series.Count));
        }

        private static void ImportSettings(ChartSettingsJson settingsJson, BaseChart chart)
        {
            if (settingsJson == null || chart == null) return;
            try
            {
                if (!string.IsNullOrEmpty(settingsJson.chartName))
                    SetChartNameRaw(chart, settingsJson.chartName);

                chart.useUtc = settingsJson.useUtc;

                if (!string.IsNullOrEmpty(settingsJson.data) && settingsJson.data != "{}")
                {
                    var settingsTarget = chart.settings;
                    if (settingsTarget == null)
                    {
                        settingsTarget = Activator.CreateInstance(typeof(Settings)) as Settings;
                        if (settingsTarget != null)
                            SetChartSettingsRaw(chart, settingsTarget);
                    }
                    if (settingsTarget != null)
                        JsonUtility.FromJsonOverwrite(settingsJson.data, settingsTarget);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(string.Format("{0}: Failed to import chart settings: {1}", LOG_TAG, ex.Message));
            }
        }

        private static void SetChartNameRaw(BaseChart chart, string name)
        {
            if (chart == null) return;
            try
            {
                var field = chart.GetType().GetField("m_ChartName", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                    field.SetValue(chart, name);
                else
                    chart.chartName = name;
            }
            catch
            {
                chart.chartName = name;
            }
        }

        private static void SetChartSettingsRaw(BaseChart chart, Settings settings)
        {
            if (chart == null) return;
            try
            {
                var field = chart.GetType().GetField("m_Settings", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                    field.SetValue(chart, settings);
            }
            catch
            {
            }
        }

        private static void EnsureTypeMapsInitialized(BaseChart chart)
        {
            if (chart == null) return;
            if (chart.typeListForSerie != null && chart.typeListForSerie.Count > 0 &&
                chart.typeListForComponent != null && chart.typeListForComponent.Count > 0)
                return;
            try
            {
                chart.OnAfterDeserialize();
            }
            catch
            {
            }
        }

        // ─── Validation ───────────────────────────────────────────────────────

        private static void ValidateSchema(ChartJson dto)
        {
            // Only v1.0 is supported in this version
            if (!dto.schemaVersion.StartsWith("1."))
                throw new ArgumentException(string.Format("Unsupported schema version '{0}'. This version only supports '1.x'.", dto.schemaVersion));
        }

        // ─── Components ───────────────────────────────────────────────────────

        private static void ImportComponents(List<ComponentJson> componentJsons, BaseChart chart)
        {
            if (componentJsons == null || componentJsons.Count == 0) return;
            foreach (var compJson in componentJsons)
            {
                if (string.IsNullOrEmpty(compJson.type))
                {
                    Debug.LogWarning(string.Format("{0}: Skipping component with empty type.", LOG_TAG));
                    continue;
                }
                var type = ResolveType(compJson.type, typeof(MainComponent));
                if (type == null)
                {
                    Debug.LogWarning(string.Format("{0}: Component type not found: '{1}'. Skipping.", LOG_TAG, compJson.type));
                    continue;
                }
                if (!typeof(MainComponent).IsAssignableFrom(type))
                {
                    Debug.LogWarning(string.Format("{0}: Type '{1}' is not a MainComponent. Skipping.", LOG_TAG, type.Name));
                    continue;
                }
                try
                {
                    // Find or add the component
                    MainComponent target = null;
                    foreach (var comp in chart.components)
                    {
                        if (comp.GetType() == type)
                        {
                            target = comp;
                            break;
                        }
                    }
                    if (target == null)
                    {
                        if (!chart.CanAddChartComponent(type))
                        {
                            Debug.LogWarning(string.Format("{0}: Cannot add component '{1}'. Skipping.", LOG_TAG, type.Name));
                            continue;
                        }
                        target = chart.AddChartComponent(type);
                    }
                    if (target != null && !string.IsNullOrEmpty(compJson.data))
                        JsonUtility.FromJsonOverwrite(compJson.data, target);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(string.Format("{0}: Failed to import component '{1}': {2}", LOG_TAG, compJson.type, ex.Message));
                }
            }
        }

        // ─── Series ───────────────────────────────────────────────────────────

        private static void ImportSeries(List<SerieJson> serieJsons, BaseChart chart)
        {
            if (serieJsons == null || serieJsons.Count == 0) return;
            // Remove all series
            for (int i = chart.series.Count - 1; i >= 0; i--)
                chart.RemoveSerie(chart.series[i]);
            foreach (var serieJson in serieJsons)
            {
                if (string.IsNullOrEmpty(serieJson.type))
                {
                    Debug.LogWarning(string.Format("{0}: Skipping serie with empty type.", LOG_TAG));
                    continue;
                }
                var type = ResolveType(serieJson.type, typeof(Serie));
                if (type == null)
                {
                    Debug.LogWarning(string.Format("{0}: Serie type not found: '{1}'. The extension module may not be installed. Skipping.", LOG_TAG, serieJson.type));
                    continue;
                }
                if (!typeof(Serie).IsAssignableFrom(type))
                {
                    Debug.LogWarning(string.Format("{0}: Type '{1}' is not a Serie. Skipping.", LOG_TAG, type.Name));
                    continue;
                }
                try
                {
                    if (!chart.CanAddSerie(type))
                    {
                        Debug.LogWarning(string.Format("{0}: Cannot add serie '{1}'. Skipping.", LOG_TAG, type.Name));
                        continue;
                    }
                    // Use reflection to call AddSerie<T>()
                    var method = chart.GetType().GetMethod("AddSerie", BindingFlags.Public | BindingFlags.Instance);
                    var genericMethod = method.MakeGenericMethod(type);
                    var serie = genericMethod.Invoke(chart, new object[] { null, true, false }) as Serie;
                    if (serie != null && !string.IsNullOrEmpty(serieJson.data))
                    {
                        JsonUtility.FromJsonOverwrite(serieJson.data, serie);
                        serie.show = serieJson.enabled;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(string.Format("{0}: Failed to import serie '{1}': {2}", LOG_TAG, serieJson.type, ex.Message));
                }
            }
        }

        // ─── Theme ────────────────────────────────────────────────────────────

        private static void ImportTheme(ThemeSnapshotJson snapshot, BaseChart chart)
        {
            var themeStyle = chart.theme;
            if (themeStyle == null) return;

            try
            {
                if (!string.IsNullOrEmpty(snapshot.data) && snapshot.data != "{}")
                    JsonUtility.FromJsonOverwrite(snapshot.data, themeStyle);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(string.Format("{0}: Failed to restore theme style data: {1}", LOG_TAG, ex.Message));
            }

            if (themeStyle.sharedTheme == null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(snapshot.themeName))
                        themeStyle.sharedTheme = XCThemeMgr.GetTheme(snapshot.themeName);
                    if (themeStyle.sharedTheme == null)
                        themeStyle.sharedTheme = XCThemeMgr.GetTheme((ThemeType)snapshot.themeType);
                }
                catch
                {
                    themeStyle.sharedTheme = XCThemeMgr.GetTheme(ThemeType.Default);
                }
                if (themeStyle.sharedTheme == null)
                    themeStyle.sharedTheme = XCThemeMgr.GetTheme(ThemeType.Default);
            }
            themeStyle.SetAllDirty();
        }

        // ─── Type resolution ──────────────────────────────────────────────────

        /// <summary>
        /// Resolves an assembly-qualified type name, with fallback to short name or namespace search.
        /// </summary>
        private static Type ResolveType(string typeName, Type expectedBaseType)
        {
            if (string.IsNullOrEmpty(typeName)) return null;

            // 1. Direct resolution (same project / same assembly)
            var type = Type.GetType(typeName);
            if (IsExpectedType(type, expectedBaseType)) return type;

            // 2. Try resolving by short name across all loaded assemblies
            var shortName = typeName.Split(',')[0].Trim();
            // Strip namespace prefix for a simple name match
            var simpleName = shortName.Contains(".") ? shortName.Substring(shortName.LastIndexOf(".") + 1) : shortName;

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    // Try by full type name first
                    type = asm.GetType(shortName);
                    if (IsExpectedType(type, expectedBaseType)) return type;

                    // Try by simple name in XCharts.Runtime namespace
                    type = asm.GetType("XCharts.Runtime." + simpleName);
                    if (IsExpectedType(type, expectedBaseType)) return type;

                    // Try by simple type name across all types in the assembly
                    var types = asm.GetTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        var candidate = types[i];
                        if (candidate == null) continue;
                        if (!string.Equals(candidate.Name, simpleName, StringComparison.Ordinal))
                            continue;
                        if (!IsExpectedType(candidate, expectedBaseType))
                            continue;
                        return candidate;
                    }
                }
                catch (ReflectionTypeLoadException rtle)
                {
                    var partialTypes = rtle.Types;
                    if (partialTypes == null) continue;
                    for (int i = 0; i < partialTypes.Length; i++)
                    {
                        var candidate = partialTypes[i];
                        if (candidate == null) continue;
                        if (!string.Equals(candidate.Name, simpleName, StringComparison.Ordinal))
                            continue;
                        if (!IsExpectedType(candidate, expectedBaseType))
                            continue;
                        return candidate;
                    }
                }
                catch { }
            }

            return null;
        }

        private static bool IsExpectedType(Type candidate, Type expectedBaseType)
        {
            if (candidate == null) return false;
            if (expectedBaseType == null) return true;
            return expectedBaseType.IsAssignableFrom(candidate);
        }

    }

    internal static class ChartJsonDataFieldCodec
    {
        private const string DATA_KEY = "\"data\"";

        public static string ConvertEscapedDataStringToRawObject(string json)
        {
            if (string.IsNullOrEmpty(json)) return json;
            var sb = new System.Text.StringBuilder(json.Length + 64);
            int i = 0;
            while (i < json.Length)
            {
                int keyIndex = json.IndexOf(DATA_KEY, i, StringComparison.Ordinal);
                if (keyIndex < 0)
                {
                    sb.Append(json, i, json.Length - i);
                    break;
                }

                sb.Append(json, i, keyIndex - i);
                int colonIndex = FindNextNonWhitespace(json, keyIndex + DATA_KEY.Length);
                if (colonIndex < 0 || json[colonIndex] != ':')
                {
                    sb.Append(DATA_KEY);
                    i = keyIndex + DATA_KEY.Length;
                    continue;
                }
                sb.Append(DATA_KEY);
                sb.Append(':');

                int valueIndex = FindNextNonWhitespace(json, colonIndex + 1);
                if (valueIndex < 0)
                {
                    i = json.Length;
                    break;
                }

                if (json[valueIndex] != '"')
                {
                    int copyEnd = FindValueEnd(json, valueIndex);
                    sb.Append(json, valueIndex, copyEnd - valueIndex);
                    i = copyEnd;
                    continue;
                }

                int stringEnd;
                string stringContent = ReadJsonStringContent(json, valueIndex, out stringEnd);
                string decoded = DecodeJsonString(stringContent);
                string trimmed = decoded == null ? string.Empty : decoded.TrimStart();
                if (trimmed.StartsWith("{") || trimmed.StartsWith("["))
                    sb.Append(decoded);
                else
                    sb.Append('"').Append(EncodeJsonString(decoded)).Append('"');
                i = stringEnd;
            }
            return sb.ToString();
        }

        public static string ConvertRawObjectDataToEscapedString(string json)
        {
            if (string.IsNullOrEmpty(json)) return json;
            var sb = new System.Text.StringBuilder(json.Length + 64);
            int i = 0;
            while (i < json.Length)
            {
                int keyIndex = json.IndexOf(DATA_KEY, i, StringComparison.Ordinal);
                if (keyIndex < 0)
                {
                    sb.Append(json, i, json.Length - i);
                    break;
                }

                sb.Append(json, i, keyIndex - i);
                int colonIndex = FindNextNonWhitespace(json, keyIndex + DATA_KEY.Length);
                if (colonIndex < 0 || json[colonIndex] != ':')
                {
                    sb.Append(DATA_KEY);
                    i = keyIndex + DATA_KEY.Length;
                    continue;
                }
                sb.Append(DATA_KEY);
                sb.Append(':');

                int valueIndex = FindNextNonWhitespace(json, colonIndex + 1);
                if (valueIndex < 0)
                {
                    i = json.Length;
                    break;
                }

                char start = json[valueIndex];
                if (start == '"')
                {
                    int stringEnd;
                    ReadJsonStringContent(json, valueIndex, out stringEnd);
                    sb.Append(json, valueIndex, stringEnd - valueIndex);
                    i = stringEnd;
                    continue;
                }

                if (start == '{' || start == '[')
                {
                    int valueEnd = ReadJsonCompositeEnd(json, valueIndex);
                    string raw = json.Substring(valueIndex, valueEnd - valueIndex);
                    sb.Append('"').Append(EncodeJsonString(raw)).Append('"');
                    i = valueEnd;
                    continue;
                }

                int plainEnd = FindValueEnd(json, valueIndex);
                sb.Append('"').Append(EncodeJsonString(json.Substring(valueIndex, plainEnd - valueIndex))).Append('"');
                i = plainEnd;
            }
            return sb.ToString();
        }

        private static int FindNextNonWhitespace(string text, int start)
        {
            for (int i = start; i < text.Length; i++)
            {
                char c = text[i];
                if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
                    return i;
            }
            return -1;
        }

        private static int FindValueEnd(string text, int start)
        {
            int i = start;
            while (i < text.Length)
            {
                char c = text[i];
                if (c == ',' || c == '}' || c == ']')
                    return i;
                i++;
            }
            return text.Length;
        }

        private static string ReadJsonStringContent(string json, int quoteStart, out int endIndex)
        {
            var sb = new System.Text.StringBuilder();
            int i = quoteStart + 1;
            bool escaping = false;
            while (i < json.Length)
            {
                char c = json[i++];
                if (escaping)
                {
                    sb.Append(c);
                    escaping = false;
                    continue;
                }
                if (c == '\\')
                {
                    sb.Append(c);
                    escaping = true;
                    continue;
                }
                if (c == '"')
                {
                    endIndex = i;
                    return sb.ToString();
                }
                sb.Append(c);
            }
            endIndex = json.Length;
            return sb.ToString();
        }

        private static int ReadJsonCompositeEnd(string json, int start)
        {
            int depth = 0;
            bool inString = false;
            bool escaping = false;
            for (int i = start; i < json.Length; i++)
            {
                char c = json[i];
                if (inString)
                {
                    if (escaping)
                    {
                        escaping = false;
                    }
                    else if (c == '\\')
                    {
                        escaping = true;
                    }
                    else if (c == '"')
                    {
                        inString = false;
                    }
                    continue;
                }

                if (c == '"')
                {
                    inString = true;
                    continue;
                }
                if (c == '{' || c == '[') depth++;
                else if (c == '}' || c == ']')
                {
                    depth--;
                    if (depth == 0) return i + 1;
                }
            }
            return json.Length;
        }

        private static string DecodeJsonString(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c != '\\')
                {
                    sb.Append(c);
                    continue;
                }
                if (i + 1 >= s.Length)
                {
                    sb.Append('\\');
                    break;
                }
                char n = s[++i];
                switch (n)
                {
                    case '"': sb.Append('"'); break;
                    case '\\': sb.Append('\\'); break;
                    case '/': sb.Append('/'); break;
                    case 'b': sb.Append('\b'); break;
                    case 'f': sb.Append('\f'); break;
                    case 'n': sb.Append('\n'); break;
                    case 'r': sb.Append('\r'); break;
                    case 't': sb.Append('\t'); break;
                    case 'u':
                        if (i + 4 < s.Length)
                        {
                            string hex = s.Substring(i + 1, 4);
                            int code;
                            if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out code))
                            {
                                sb.Append((char) code);
                                i += 4;
                            }
                            else sb.Append("\\u").Append(hex);
                        }
                        else sb.Append("\\u");
                        break;
                    default:
                        sb.Append(n);
                        break;
                }
            }
            return sb.ToString();
        }

        private static string EncodeJsonString(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                switch (c)
                {
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (c < 32)
                            sb.Append("\\u").Append(((int)c).ToString("x4"));
                        else
                            sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }

    internal static class JsonDiffPruner
    {
        public static string PruneDefaults(string currentJson, string defaultJson)
        {
            if (string.IsNullOrEmpty(currentJson)) return "{}";
            object currentObj;
            object defaultObj;
            if (!SimpleJson.TryParse(currentJson, out currentObj)) return currentJson;
            if (!SimpleJson.TryParse(defaultJson, out defaultObj)) return currentJson;

            var pruned = DiffNode(currentObj, defaultObj);
            if (pruned == null)
                return "{}";
            return SimpleJson.Stringify(pruned);
        }

        public static object PruneParsedDefaults(object currentObj, object defaultObj)
        {
            return DiffNode(currentObj, defaultObj);
        }

        private static object DiffNode(object current, object defaults)
        {
            if (current == null)
                return null;

            var currentDict = current as Dictionary<string, object>;
            var defaultDict = defaults as Dictionary<string, object>;
            if (currentDict != null)
            {
                var result = new Dictionary<string, object>();
                foreach (var kv in currentDict)
                {
                    object defaultValue = null;
                    if (defaultDict != null)
                        defaultDict.TryGetValue(kv.Key, out defaultValue);
                    var diff = DiffNode(kv.Value, defaultValue);
                    if (diff != null)
                        result[kv.Key] = diff;
                }
                if (result.Count == 0)
                    return null;
                return result;
            }

            var currentList = current as List<object>;
            var defaultList = defaults as List<object>;
            if (currentList != null)
            {
                if (currentList.Count == 0)
                    return null;

                if (defaultList != null && AreListsEqual(currentList, defaultList))
                    return null;

                var result = new List<object>();
                for (int i = 0; i < currentList.Count; i++)
                {
                    object defaultItem = null;
                    if (defaultList != null && i < defaultList.Count)
                        defaultItem = defaultList[i];
                    var diff = DiffNode(currentList[i], defaultItem);
                    if (diff != null)
                        result.Add(diff);
                }

                if (result.Count == 0)
                    return null;
                return result;
            }

            if (AreValuesEqual(current, defaults))
                return null;
            return current;
        }

        private static bool AreListsEqual(List<object> a, List<object> b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++)
            {
                if (!AreValuesEqual(a[i], b[i]))
                    return false;
            }
            return true;
        }

        private static bool AreValuesEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;

            var aDict = a as Dictionary<string, object>;
            var bDict = b as Dictionary<string, object>;
            if (aDict != null && bDict != null)
            {
                if (aDict.Count != bDict.Count) return false;
                foreach (var kv in aDict)
                {
                    object bv;
                    if (!bDict.TryGetValue(kv.Key, out bv)) return false;
                    if (!AreValuesEqual(kv.Value, bv)) return false;
                }
                return true;
            }

            var aList = a as List<object>;
            var bList = b as List<object>;
            if (aList != null && bList != null)
                return AreListsEqual(aList, bList);

            return string.Equals(Convert.ToString(a), Convert.ToString(b), StringComparison.Ordinal);
        }
    }

    internal static class SimpleJson
    {
        public static bool TryParse(string json, out object result)
        {
            result = null;
            if (string.IsNullOrEmpty(json)) return false;
            try
            {
                int index = 0;
                result = ParseValue(json, ref index);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string Stringify(object obj)
        {
            var sb = new System.Text.StringBuilder();
            WriteValue(sb, obj, false, 0);
            return sb.ToString();
        }

        public static string Stringify(object obj, bool pretty)
        {
            var sb = new System.Text.StringBuilder();
            WriteValue(sb, obj, pretty, 0);
            return sb.ToString();
        }

        private static object ParseValue(string s, ref int i)
        {
            SkipWhitespace(s, ref i);
            if (i >= s.Length) return null;
            char c = s[i];
            if (c == '{') return ParseObject(s, ref i);
            if (c == '[') return ParseArray(s, ref i);
            if (c == '"') return ParseString(s, ref i);
            if (c == 't' || c == 'f') return ParseBool(s, ref i);
            if (c == 'n') return ParseNull(s, ref i);
            return ParseNumber(s, ref i);
        }

        private static Dictionary<string, object> ParseObject(string s, ref int i)
        {
            var dict = new Dictionary<string, object>();
            i++; // {
            while (true)
            {
                SkipWhitespace(s, ref i);
                if (i < s.Length && s[i] == '}')
                {
                    i++;
                    break;
                }
                var key = ParseString(s, ref i);
                SkipWhitespace(s, ref i);
                if (i < s.Length && s[i] == ':') i++;
                var value = ParseValue(s, ref i);
                dict[key] = value;
                SkipWhitespace(s, ref i);
                if (i < s.Length && s[i] == ',')
                {
                    i++;
                    continue;
                }
                if (i < s.Length && s[i] == '}')
                {
                    i++;
                    break;
                }
            }
            return dict;
        }

        private static List<object> ParseArray(string s, ref int i)
        {
            var list = new List<object>();
            i++; // [
            while (true)
            {
                SkipWhitespace(s, ref i);
                if (i < s.Length && s[i] == ']')
                {
                    i++;
                    break;
                }
                list.Add(ParseValue(s, ref i));
                SkipWhitespace(s, ref i);
                if (i < s.Length && s[i] == ',')
                {
                    i++;
                    continue;
                }
                if (i < s.Length && s[i] == ']')
                {
                    i++;
                    break;
                }
            }
            return list;
        }

        private static string ParseString(string s, ref int i)
        {
            var sb = new System.Text.StringBuilder();
            i++; // opening quote
            while (i < s.Length)
            {
                var c = s[i++];
                if (c == '"') break;
                if (c == '\\' && i < s.Length)
                {
                    var e = s[i++];
                    switch (e)
                    {
                        case '"': sb.Append('"'); break;
                        case '\\': sb.Append('\\'); break;
                        case '/': sb.Append('/'); break;
                        case 'b': sb.Append('\b'); break;
                        case 'f': sb.Append('\f'); break;
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        case 'u':
                            if (i + 3 < s.Length)
                            {
                                var hex = s.Substring(i, 4);
                                int code;
                                if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out code))
                                {
                                    sb.Append((char)code);
                                    i += 4;
                                }
                            }
                            break;
                        default: sb.Append(e); break;
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private static object ParseNumber(string s, ref int i)
        {
            int start = i;
            while (i < s.Length)
            {
                var c = s[i];
                if ((c >= '0' && c <= '9') || c == '-' || c == '+' || c == '.' || c == 'e' || c == 'E')
                    i++;
                else
                    break;
            }
            var token = s.Substring(start, i - start);
            double d;
            if (double.TryParse(token, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out d))
                return d;
            return 0d;
        }

        private static bool ParseBool(string s, ref int i)
        {
            if (s.IndexOf("true", i, StringComparison.Ordinal) == i)
            {
                i += 4;
                return true;
            }
            i += 5;
            return false;
        }

        private static object ParseNull(string s, ref int i)
        {
            i += 4;
            return null;
        }

        private static void SkipWhitespace(string s, ref int i)
        {
            while (i < s.Length)
            {
                var c = s[i];
                if (c == ' ' || c == '\t' || c == '\n' || c == '\r') i++;
                else break;
            }
        }

        private static void WriteValue(System.Text.StringBuilder sb, object value, bool pretty, int indent)
        {
            if (value == null)
            {
                sb.Append("null");
                return;
            }
            var dict = value as Dictionary<string, object>;
            if (dict != null)
            {
                WriteObject(sb, dict, pretty, indent);
                return;
            }
            var list = value as List<object>;
            if (list != null)
            {
                WriteArray(sb, list, pretty, indent);
                return;
            }
            var str = value as string;
            if (str != null)
            {
                WriteString(sb, str);
                return;
            }
            if (value is bool)
            {
                sb.Append((bool)value ? "true" : "false");
                return;
            }
            sb.Append(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
        }

        private static void WriteObject(System.Text.StringBuilder sb, Dictionary<string, object> dict, bool pretty, int indent)
        {
            sb.Append('{');
            if (dict.Count == 0)
            {
                sb.Append('}');
                return;
            }

            bool first = true;
            foreach (var kv in dict)
            {
                if (!first) sb.Append(',');
                if (pretty)
                {
                    sb.Append('\n');
                    AppendIndent(sb, indent + 1);
                }
                first = false;
                WriteString(sb, kv.Key);
                sb.Append(pretty ? ": " : ":");
                WriteValue(sb, kv.Value, pretty, indent + 1);
            }
            if (pretty)
            {
                sb.Append('\n');
                AppendIndent(sb, indent);
            }
            sb.Append('}');
        }

        private static void WriteArray(System.Text.StringBuilder sb, List<object> list, bool pretty, int indent)
        {
            sb.Append('[');
            if (list.Count == 0)
            {
                sb.Append(']');
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0) sb.Append(',');
                if (pretty)
                {
                    sb.Append('\n');
                    AppendIndent(sb, indent + 1);
                }
                WriteValue(sb, list[i], pretty, indent + 1);
            }
            if (pretty)
            {
                sb.Append('\n');
                AppendIndent(sb, indent);
            }
            sb.Append(']');
        }

        private static void AppendIndent(System.Text.StringBuilder sb, int indent)
        {
            for (int i = 0; i < indent; i++)
            {
                sb.Append("    ");
            }
        }

        private static void WriteString(System.Text.StringBuilder sb, string s)
        {
            sb.Append('"');
            for (int i = 0; i < s.Length; i++)
            {
                var c = s[i];
                switch (c)
                {
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (c < 32)
                            sb.Append("\\u").Append(((int)c).ToString("x4"));
                        else
                            sb.Append(c);
                        break;
                }
            }
            sb.Append('"');
        }
    }
}
