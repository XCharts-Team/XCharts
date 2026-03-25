using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts.Runtime
{
    /// <summary>
    /// Portable resource reference that supports a multi-level lookup strategy:
    /// GUID → path → name → fallbackName → null/default
    /// </summary>
    [Serializable]
    public class ResourceRef
    {
        /// <summary>Unity AssetDatabase GUID (editor-only, same project).</summary>
        public string guid;
        /// <summary>Asset path relative to project root e.g. "Assets/Fonts/Arial.ttf".</summary>
        public string path;
        /// <summary>Asset.name used for cross-project name search.</summary>
        public string name;
        /// <summary>Optional secondary name (system font alias, built-in default, etc.).</summary>
        public string fallbackName;
        /// <summary>Optional Base64-encoded asset bytes for full portability (< 100 KB recommended).</summary>
        public string base64;

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(guid)
                && string.IsNullOrEmpty(path)
                && string.IsNullOrEmpty(name)
                && string.IsNullOrEmpty(fallbackName)
                && string.IsNullOrEmpty(base64);
        }

        public override string ToString()
        {
            return string.Format("ResourceRef{{name={0}, path={1}, guid={2}}}", name, path, guid);
        }
    }

    /// <summary>
    /// Handles serialization and resolution of Unity asset references for chart JSON export/import.
    /// Supports Font, TMP_FontAsset, Sprite, Material, Texture2D.
    /// </summary>
    public static class ResourceRefHandler
    {
        // ─── Serialize ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Serializes a Unity Object into a portable ResourceRef.
        /// </summary>
        public static ResourceRef Serialize(UnityEngine.Object asset, bool includeBase64 = false)
        {
            if (asset == null) return null;

            var refData = new ResourceRef
            {
                name = asset.name
            };

#if UNITY_EDITOR
            var assetPath = AssetDatabase.GetAssetPath(asset);
            if (!string.IsNullOrEmpty(assetPath))
            {
                refData.path = assetPath;
                refData.guid = AssetDatabase.AssetPathToGUID(assetPath);
            }
#endif

            // Optional base64 embedding for portability
            if (includeBase64)
            {
                var b64 = TryEncodeBase64(asset);
                if (!string.IsNullOrEmpty(b64))
                    refData.base64 = b64;
            }

            return refData;
        }

        /// <summary>
        /// Serializes a Font with a fallback name hint.
        /// </summary>
        public static ResourceRef SerializeFont(Font font, string fallbackName = null)
        {
            if (font == null) return null;
            var refData = Serialize(font) ?? new ResourceRef();
            refData.fallbackName = fallbackName ?? "Arial";
            return refData;
        }

#if dUI_TextMeshPro
        /// <summary>
        /// Serializes a TMP_FontAsset with a fallback name hint.
        /// </summary>
        public static ResourceRef SerializeTMPFont(TMP_FontAsset font, string fallbackName = null)
        {
            if (font == null) return null;
            var refData = Serialize(font) ?? new ResourceRef();
            refData.fallbackName = fallbackName ?? "LiberationSans SDF";
            return refData;
        }
#endif

        // ─── Resolve ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Attempts to resolve a ResourceRef back to a Unity asset using the fallback chain:<br/>
        /// GUID → path → name → fallbackName → null
        /// </summary>
        public static T TryResolve<T>(ResourceRef refData) where T : UnityEngine.Object
        {
            if (refData == null || refData.IsEmpty()) return null;

#if UNITY_EDITOR
            // 1. GUID lookup (same project, exact)
            if (!string.IsNullOrEmpty(refData.guid))
            {
                var guidPath = AssetDatabase.GUIDToAssetPath(refData.guid);
                if (!string.IsNullOrEmpty(guidPath))
                {
                    var asset = AssetDatabase.LoadAssetAtPath<T>(guidPath);
                    if (asset != null) return asset;
                }
            }

            // 2. Path-based lookup
            if (!string.IsNullOrEmpty(refData.path))
            {
                var asset = AssetDatabase.LoadAssetAtPath<T>(refData.path);
                if (asset != null) return asset;
            }

            // 3. Name-based search across project
            if (!string.IsNullOrEmpty(refData.name))
            {
                var found = FindAssetByName<T>(refData.name);
                if (found != null) return found;
            }
#endif

            // 4. Resources.Load by name
            if (!string.IsNullOrEmpty(refData.name))
            {
                var asset = Resources.Load<T>(refData.name);
                if (asset != null) return asset;
            }

            // 5. Fallback name
            if (!string.IsNullOrEmpty(refData.fallbackName))
            {
#if UNITY_EDITOR
                var found = FindAssetByName<T>(refData.fallbackName);
                if (found != null) return found;
#endif
                var asset = Resources.Load<T>(refData.fallbackName);
                if (asset != null) return asset;
            }

            // 6. Base64 decode
            if (!string.IsNullOrEmpty(refData.base64))
            {
                var decoded = TryDecodeBase64<T>(refData.base64, refData.name ?? "imported_asset");
                if (decoded != null) return decoded;
            }

            if (!IsUnityBuiltinDefaultResourceRef(refData))
                Debug.LogWarning(string.Format("[XCharts] ResourceRefHandler: Could not resolve asset '{0}'. Using default.", refData));
            return null;
        }

        private static bool IsUnityBuiltinDefaultResourceRef(ResourceRef refData)
        {
            if (refData == null) return false;

            bool pathIsBuiltin = !string.IsNullOrEmpty(refData.path)
                && refData.path.IndexOf("Library/unity default resources", StringComparison.OrdinalIgnoreCase) >= 0;

            bool guidIsBuiltin = !string.IsNullOrEmpty(refData.guid)
                && string.Equals(refData.guid, "0000000000000000e000000000000000", StringComparison.OrdinalIgnoreCase);

            bool nameIsBuiltinFont = string.Equals(refData.name, "Arial", StringComparison.OrdinalIgnoreCase)
                || string.Equals(refData.fallbackName, "Arial", StringComparison.OrdinalIgnoreCase)
#if dUI_TextMeshPro
                || string.Equals(refData.name, "LiberationSans SDF", StringComparison.OrdinalIgnoreCase)
                || string.Equals(refData.fallbackName, "LiberationSans SDF", StringComparison.OrdinalIgnoreCase)
#endif
                ;

            return pathIsBuiltin || guidIsBuiltin || nameIsBuiltinFont;
        }

#if UNITY_EDITOR
        private static T FindAssetByName<T>(string assetName) where T : UnityEngine.Object
        {
            string typeName = typeof(T).Name;
            var guids = AssetDatabase.FindAssets(string.Format("{0} t:{1}", assetName, typeName));
            foreach (var g in guids)
            {
                var p = AssetDatabase.GUIDToAssetPath(g);
                var asset = AssetDatabase.LoadAssetAtPath<T>(p);
                if (asset != null && asset.name == assetName)
                    return asset;
            }
            // Looser match (name contains)
            foreach (var g in guids)
            {
                var p = AssetDatabase.GUIDToAssetPath(g);
                var asset = AssetDatabase.LoadAssetAtPath<T>(p);
                if (asset != null) return asset;
            }
            return null;
        }
#endif

        // ─── Base64 helpers ──────────────────────────────────────────────────────────

        private static string TryEncodeBase64(UnityEngine.Object asset)
        {
            var texture = asset as Texture2D;
            if (texture != null)
                return Convert.ToBase64String(texture.EncodeToPNG());
            // Font/Material: not trivially serializable as bytes at runtime; skip.
            return null;
        }

        private static T TryDecodeBase64<T>(string base64, string assetName) where T : UnityEngine.Object
        {
            try
            {
                if (typeof(T) == typeof(Texture2D) || typeof(T) == typeof(Sprite))
                {
                    var bytes = Convert.FromBase64String(base64);
                    var tex = new Texture2D(2, 2);
                    if (tex.LoadImage(bytes))
                    {
                        tex.name = assetName;
                        if (typeof(T) == typeof(Sprite))
                        {
                            var sprite = Sprite.Create(tex,
                                new Rect(0, 0, tex.width, tex.height),
                                new Vector2(0.5f, 0.5f));
                            sprite.name = assetName;
                            return sprite as T;
                        }
                        return tex as T;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(string.Format("[XCharts] ResourceRefHandler: Base64 decode failed: {0}", ex.Message));
            }
            return null;
        }

        // ─── Convenience overloads ───────────────────────────────────────────────────

        public static Font TryResolveFont(ResourceRef refData)
        {
            return TryResolve<Font>(refData);
        }

        public static Sprite TryResolveSprite(ResourceRef refData)
        {
            return TryResolve<Sprite>(refData);
        }

        public static Material TryResolveMaterial(ResourceRef refData)
        {
            return TryResolve<Material>(refData);
        }

        public static Texture2D TryResolveTexture(ResourceRef refData)
        {
            return TryResolve<Texture2D>(refData);
        }

#if dUI_TextMeshPro
        public static TMP_FontAsset TryResolveTMPFont(ResourceRef refData)
        {
            return TryResolve<TMP_FontAsset>(refData);
        }
#endif
    }
}
