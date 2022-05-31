using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace XCharts.Runtime
{
    public static class RuntimeUtil
    {
        public static bool HasSubclass(Type type)
        {
            var typeMap = GetAllTypesDerivedFrom(type);
            foreach (var t in typeMap)
            {
                return true;
            }
            return false;
        }

        public static IEnumerable<Type> GetAllTypesDerivedFrom<T>()
        {
#if UNITY_EDITOR && UNITY_2019_2_OR_NEWER
            return UnityEditor.TypeCache.GetTypesDerivedFrom<T>();
#else
            return GetAllAssemblyTypes().Where(t => t.IsSubclassOf(typeof(T)));
#endif
        }
        public static IEnumerable<Type> GetAllTypesDerivedFrom(Type type)
        {
#if UNITY_EDITOR && UNITY_2019_2_OR_NEWER
            return UnityEditor.TypeCache.GetTypesDerivedFrom(type);
#else
            return GetAllAssemblyTypes().Where(t => t.IsSubclassOf(type));
#endif
        }

        static IEnumerable<Type> m_AssemblyTypes;

        public static IEnumerable<Type> GetAllAssemblyTypes()
        {
            if (m_AssemblyTypes == null)
            {
                m_AssemblyTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(t =>
                    {
                        var innerTypes = new Type[0];
                        try
                        {
                            innerTypes = t.GetTypes();
                        }
                        catch { }
                        return innerTypes;
                    });
            }
            return m_AssemblyTypes;
        }

        public static T GetAttribute<T>(this Type type, bool check = true) where T : Attribute
        {
            if (type.IsDefined(typeof(T), false))
                return (T) type.GetCustomAttributes(typeof(T), false) [0];
            else
            {
                if (check)
                    Assert.IsTrue(false, "Attribute not found:" + type.Name);
                return null;
            }
        }
        public static T GetAttribute<T>(this MemberInfo type, bool check = true) where T : Attribute
        {
            if (type.IsDefined(typeof(T), false))
                return (T) type.GetCustomAttributes(typeof(T), false) [0];
            else
            {
                if (check)
                    Assert.IsTrue(false, "Attribute not found:" + type.Name);
                return null;
            }
        }

        

    }
}