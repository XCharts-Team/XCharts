#if UNITY_EDITOR

using System;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class DefineSymbolsUtil
    {
        private static readonly StringBuilder s_StringBuilder = new StringBuilder();

        public static void AddGlobalDefine(string symbol)
        {
            var flag = false;
            var num = 0;
#if UNITY_2022_1_OR_NEWER
            foreach (var buildTargetGroup in (BuildTargetGroup[]) Enum.GetValues(typeof(BuildTargetGroup)))
            {
                if (IsValidBuildTargetGroup(buildTargetGroup))
                {
                    var buildTargetName = NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);
                    var symbols = PlayerSettings.GetScriptingDefineSymbols(buildTargetName);
                    symbols = symbols.Replace(" ", "");
                    if (Array.IndexOf(symbols.Split(';'), symbol) != -1) continue;
                    flag = true;
                    num++;
                    var defines = symbols + (symbols.Length > 0 ? ";" + symbol : symbol);
                    PlayerSettings.SetScriptingDefineSymbols(buildTargetName, defines);
                }
            }
#else
            foreach (var buildTargetGroup in (BuildTargetGroup[]) Enum.GetValues(typeof(BuildTargetGroup)))
            {
                if (IsValidBuildTargetGroup(buildTargetGroup))
                {
                    var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
                    symbols = symbols.Replace(" ", "");
                    if (Array.IndexOf(symbols.Split(';'), symbol) != -1) continue;
                    flag = true;
                    num++;
                    var defines = symbols + (symbols.Length > 0 ? ";" + symbol : symbol);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
                }
            }
#endif
            if (flag)
            {
                Debug.LogFormat("Added global define symbol \"{0}\" to {1} BuildTargetGroups.", symbol, num);
            }
        }

        public static void RemoveGlobalDefine(string symbol)
        {
            var flag = false;
            var num = 0;
#if UNITY_2022_1_OR_NEWER
            foreach (var buildTargetGroup in (BuildTargetGroup[]) Enum.GetValues(typeof(BuildTargetGroup)))
            {
                if (IsValidBuildTargetGroup(buildTargetGroup))
                {
                    var buildTargetName = NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);
                    var symbols = PlayerSettings.GetScriptingDefineSymbols(buildTargetName).Split(';');
                    if (Array.IndexOf(symbols, symbol) == -1) continue;
                    flag = true;
                    num++;
                    s_StringBuilder.Length = 0;
                    foreach (var str in symbols)
                    {
                        if (!str.Equals(symbol))
                        {
                            if (s_StringBuilder.Length > 0) s_StringBuilder.Append(";");
                            s_StringBuilder.Append(str);
                        }
                    }
                    PlayerSettings.SetScriptingDefineSymbols(buildTargetName, s_StringBuilder.ToString());
                }
            }
#else
            foreach (var buildTargetGroup in (BuildTargetGroup[]) Enum.GetValues(typeof(BuildTargetGroup)))
            {
                if (IsValidBuildTargetGroup(buildTargetGroup))
                {
                    var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';');
                    if (Array.IndexOf(symbols, symbol) == -1) continue;
                    flag = true;
                    num++;
                    s_StringBuilder.Length = 0;
                    foreach (var str in symbols)
                    {
                        if (!str.Equals(symbol))
                        {
                            if (s_StringBuilder.Length > 0) s_StringBuilder.Append(";");
                            s_StringBuilder.Append(str);
                        }
                    }
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, s_StringBuilder.ToString());
                }
            }
#endif
            if (flag)
            {
                Debug.LogFormat("Removed global define symbol \"{0}\" to {1} BuildTargetGroups.", symbol, num);
            }
        }

        private static bool IsValidBuildTargetGroup(BuildTargetGroup group)
        {
            if (group == BuildTargetGroup.Unknown) return false;
            var type = Type.GetType("UnityEditor.Modules.ModuleManager, UnityEditor.dll");
            if (type == null) return true;
            var method1 = type.GetMethod("GetTargetStringFromBuildTargetGroup", BindingFlags.Static | BindingFlags.NonPublic);
            var method2 = typeof(PlayerSettings).GetMethod("GetPlatformName", BindingFlags.Static | BindingFlags.NonPublic);
            if (method1 == null || method2 == null) return true;
            var str1 = (string) method1.Invoke(null, new object[] { group });
            var str2 = (string) method2.Invoke(null, new object[] { group });
            if (string.IsNullOrEmpty(str1))
            {
                return !string.IsNullOrEmpty(str2);
            }
            else
            {
                return true;
            }
        }
    }
}
#endif