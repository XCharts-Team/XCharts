/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace XCharts
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

        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            Assert.IsTrue(type.IsDefined(typeof(T), false), "Attribute not found:" + type.Name);
            return (T)type.GetCustomAttributes(typeof(T), false)[0];
        }

        public static D Mapper<D, S>(S s)
        {
            D d = Activator.CreateInstance<D>();
            try
            {
                var Types = s.GetType();//获得类型
                var Typed = typeof(D);
                foreach (PropertyInfo sp in Types.GetProperties())//获得类型的属性字段
                {
                    foreach (PropertyInfo dp in Typed.GetProperties())
                    {
                        if (dp.Name == sp.Name)//判断属性名是否相同
                        {
                            dp.SetValue(d, sp.GetValue(s, null), null);//获得s对象属性的值复制给d对象的属性
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return d;
        }

        public static bool CopyFolder(string sourPath, string destPath)
        {
            try
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                var files = Directory.GetFiles(sourPath);
                foreach (var file in files)
                {
                    var name = Path.GetFileName(file);
                    var path = Path.Combine(destPath, name);
                    File.Copy(file, path);
                }
                var folders = Directory.GetDirectories(sourPath);
                foreach (var folder in folders)
                {
                    var name = Path.GetFileName(folder);
                    var path = Path.Combine(destPath, name);
                    CopyFolder(folder, path);
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("CopyFolder:" + e.Message);
                return false;
            }
        }

    }
}