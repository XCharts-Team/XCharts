using System;
using System.Reflection;
using UnityEngine;

namespace XCharts.Runtime
{
    [Serializable]
    public class UIComponentJson
    {
        public string schemaVersion = "1.0";
        public string componentType;
        public string componentVersion;
        public string exportedAt;
        public string data;
    }

    public static class UIComponentJsonSerializer
    {
        public static string Serialize(UIComponent component, bool prettyPrint = true)
        {
            if (component == null) throw new ArgumentNullException("component");

            var currentJson = JsonUtility.ToJson(component);
            var defaultJson = GetDefaultInstanceJson(component.GetType());
            var dataJson = BuildDataJson(component.GetType(), currentJson, defaultJson);

            var dto = new UIComponentJson
            {
                schemaVersion = "1.0",
                componentType = component.GetType().Name,
                componentVersion = XChartsMgr.version,
                exportedAt = DateTime.UtcNow.ToString("o"),
                data = dataJson
            };

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

        private static string BuildDataJson(Type componentType, string currentJson, string defaultJson)
        {
            if (componentType == null)
                return JsonDiffPruner.PruneDefaults(currentJson, defaultJson);

            if (string.Equals(componentType.Name, "UITable", StringComparison.Ordinal))
                return PruneUITableData(componentType, currentJson, defaultJson);

            return JsonDiffPruner.PruneDefaults(currentJson, defaultJson);
        }

        private static string PruneUITableData(Type tableType, string currentJson, string defaultJson)
        {
            object currentParsed;
            object defaultParsed;
            if (!SimpleJson.TryParse(currentJson, out currentParsed))
                return JsonDiffPruner.PruneDefaults(currentJson, defaultJson);
            if (!SimpleJson.TryParse(defaultJson, out defaultParsed))
                return JsonDiffPruner.PruneDefaults(currentJson, defaultJson);

            var currentRoot = currentParsed as System.Collections.Generic.Dictionary<string, object>;
            var defaultRoot = defaultParsed as System.Collections.Generic.Dictionary<string, object>;
            if (currentRoot == null)
                return JsonDiffPruner.PruneDefaults(currentJson, defaultJson);

            var prunedRootObj = JsonDiffPruner.PruneParsedDefaults(currentRoot, defaultRoot);
            var prunedRoot = prunedRootObj as System.Collections.Generic.Dictionary<string, object>;
            if (prunedRoot == null)
                prunedRoot = new System.Collections.Generic.Dictionary<string, object>();

            object currentRowsObj;
            if (!currentRoot.TryGetValue("m_Data", out currentRowsObj))
                return SimpleJson.Stringify(prunedRoot);

            var currentRows = currentRowsObj as System.Collections.Generic.List<object>;
            if (currentRows == null)
                return SimpleJson.Stringify(prunedRoot);

            var rowDefaultObj = CreateDefaultParsedObject(tableType.Assembly, "XCharts.Runtime.UI.TableRow");
            if (rowDefaultObj == null)
                return JsonDiffPruner.PruneDefaults(currentJson, defaultJson);

            var cellDefaultObj = CreateDefaultParsedObject(tableType.Assembly, "XCharts.Runtime.UI.TableCell");

            var prunedRows = new System.Collections.Generic.List<object>(currentRows.Count);
            for (int i = 0; i < currentRows.Count; i++)
            {
                var prunedRow = PruneTableRowKeepCellShape(currentRows[i], rowDefaultObj, cellDefaultObj);
                prunedRows.Add(prunedRow ?? new System.Collections.Generic.Dictionary<string, object>());
            }

            prunedRoot["m_Data"] = prunedRows;
            return SimpleJson.Stringify(prunedRoot);
        }

        private static object PruneTableRowKeepCellShape(object rowObj, object rowDefaultObj, object cellDefaultObj)
        {
            var rowDict = rowObj as System.Collections.Generic.Dictionary<string, object>;
            if (rowDict == null)
                return JsonDiffPruner.PruneParsedDefaults(rowObj, rowDefaultObj);

            object rawCells;
            rowDict.TryGetValue("m_Data", out rawCells);
            var currentCells = rawCells as System.Collections.Generic.List<object>;

            var prunedRowObj = JsonDiffPruner.PruneParsedDefaults(rowObj, rowDefaultObj);
            var prunedRowDict = prunedRowObj as System.Collections.Generic.Dictionary<string, object>;
            if (prunedRowDict == null)
                prunedRowDict = new System.Collections.Generic.Dictionary<string, object>();

            if (currentCells != null)
            {
                var prunedCells = new System.Collections.Generic.List<object>(currentCells.Count);
                for (int i = 0; i < currentCells.Count; i++)
                {
                    var prunedCell = cellDefaultObj != null
                        ? JsonDiffPruner.PruneParsedDefaults(currentCells[i], cellDefaultObj)
                        : JsonDiffPruner.PruneParsedDefaults(currentCells[i], null);
                    prunedCells.Add(prunedCell ?? new System.Collections.Generic.Dictionary<string, object>());
                }
                prunedRowDict["m_Data"] = prunedCells;
            }

            if (prunedRowDict.Count == 0)
                return null;
            return prunedRowDict;
        }

        private static object CreateDefaultParsedObject(Assembly assembly, string fullTypeName)
        {
            if (assembly == null || string.IsNullOrEmpty(fullTypeName))
                return null;

            try
            {
                var type = assembly.GetType(fullTypeName);
                if (type == null)
                    return null;

                var instance = Activator.CreateInstance(type);
                if (instance == null)
                    return null;

                var json = JsonUtility.ToJson(instance);
                object parsed;
                if (!SimpleJson.TryParse(json, out parsed))
                    return null;
                return parsed;
            }
            catch
            {
                return null;
            }
        }

        private static string GetDefaultInstanceJson(Type type)
        {
            if (type == null) return "{}";

            GameObject tempObject = null;
            try
            {
                if (typeof(MonoBehaviour).IsAssignableFrom(type))
                {
                    tempObject = new GameObject("__XCharts_UIJson_Default__");
                    tempObject.hideFlags = HideFlags.HideAndDontSave;
                    var defaultComponent = tempObject.AddComponent(type) as UIComponent;
                    if (defaultComponent != null)
                        return JsonUtility.ToJson(defaultComponent);
                    return "{}";
                }

                var instance = Activator.CreateInstance(type);
                if (instance == null)
                    return "{}";
                return JsonUtility.ToJson(instance);
            }
            catch
            {
                return "{}";
            }
            finally
            {
                if (tempObject != null)
                {
#if UNITY_EDITOR
                    UnityEngine.Object.DestroyImmediate(tempObject);
#else
                    UnityEngine.Object.Destroy(tempObject);
#endif
                }
            }
        }
    }

    public static class UIComponentJsonDeserializer
    {
        private const string LOG_TAG = "[XCharts] UIComponentJsonDeserializer";

        public static void Deserialize(string json, UIComponent target)
        {
            if (string.IsNullOrEmpty(json)) throw new ArgumentNullException("json");
            if (target == null) throw new ArgumentNullException("target");

            json = ChartJsonDataFieldCodec.ConvertRawObjectDataToEscapedString(json);

            UIComponentJson dto;
            try
            {
                dto = JsonUtility.FromJson<UIComponentJson>(json);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Invalid JSON format: {0}", ex.Message), ex);
            }

            if (dto == null || string.IsNullOrEmpty(dto.schemaVersion))
                throw new ArgumentException("JSON does not appear to be a valid XCharts UI component export (missing schemaVersion).");
            if (!dto.schemaVersion.StartsWith("1."))
                throw new ArgumentException(string.Format("Unsupported schema version '{0}'. This version only supports '1.x'.", dto.schemaVersion));

            ValidateComponentType(dto.componentType, target);

            if (!string.IsNullOrEmpty(dto.data))
                JsonUtility.FromJsonOverwrite(dto.data, target);

            target.RefreshAllComponent();
            target.RefreshGraph();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(target);
#endif
            Debug.Log(string.Format("{0}: Import complete for '{1}'.", LOG_TAG, target.GetType().Name));
        }

        private static void ValidateComponentType(string typeName, UIComponent target)
        {
            if (string.IsNullOrEmpty(typeName) || target == null) return;

            var resolved = ResolveType(typeName);
            if (resolved == null) return;

            if (!resolved.IsAssignableFrom(target.GetType()))
                throw new ArgumentException(string.Format("JSON is for UI component '{0}', target is '{1}'.", resolved.Name, target.GetType().Name));
        }

        private static Type ResolveType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) return null;

            var type = Type.GetType(typeName);
            if (type != null) return type;

            var shortName = typeName.Split(',')[0].Trim();
            var simpleName = shortName.Contains(".") ? shortName.Substring(shortName.LastIndexOf(".") + 1) : shortName;

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    type = asm.GetType(shortName);
                    if (type != null) return type;

                    var types = asm.GetTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        var candidate = types[i];
                        if (candidate == null) continue;
                        if (string.Equals(candidate.Name, simpleName, StringComparison.Ordinal))
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
                        if (string.Equals(candidate.Name, simpleName, StringComparison.Ordinal))
                            return candidate;
                    }
                }
                catch
                {
                }
            }

            return null;
        }
    }
}
