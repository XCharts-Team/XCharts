using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class ReflectionUtil
    {
        private static Dictionary<object, MethodInfo> listClearMethodInfoCaches = new Dictionary<object, MethodInfo>();
        private static Dictionary<object, MethodInfo> listAddMethodInfoCaches = new Dictionary<object, MethodInfo>();

        public static void InvokeListClear(object obj, FieldInfo field)
        {
            var list = field.GetValue(obj);
            MethodInfo method;
            if (!listClearMethodInfoCaches.TryGetValue(list, out method))
            {
                method = list.GetType().GetMethod("Clear");
                listClearMethodInfoCaches[list] = method;
            }
            method.Invoke(list, new object[] { });
        }
        public static int InvokeListCount(object obj, FieldInfo field)
        {
            var list = field.GetValue(obj);
            return (int) list.GetType().GetProperty("Count").GetValue(list, null);
        }

        public static void InvokeListAdd(object obj, FieldInfo field, object item)
        {
            var list = field.GetValue(obj);
            MethodInfo method;
            if (!listAddMethodInfoCaches.TryGetValue(list, out method))
            {
                method = list.GetType().GetMethod("Add");
                listAddMethodInfoCaches[list] = method;
            }
            method.Invoke(list, new object[] { item });
        }

        public static T InvokeListGet<T>(object obj, FieldInfo field, int i)
        {
            var list = field.GetValue(obj);
            var item = list.GetType().GetProperty("Item").GetValue(list, new object[] { i });
            return (T) item;
        }

        public static void InvokeListAddTo<T>(object obj, FieldInfo field, Action<T> callback)
        {
            var list = field.GetValue(obj);
            var listType = list.GetType();
            var count = Convert.ToInt32(listType.GetProperty("Count").GetValue(list, null));
            for (int i = 0; i < count; i++)
            {
                var item = listType.GetProperty("Item").GetValue(list, new object[] { i });
                callback((T) item);
            }
        }

        public static object DeepCloneSerializeField(object obj)
        {
            if (obj == null)
                return null;

            var type = obj.GetType();
            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }
            else if (type.IsArray)
            {
                var elementType = Type.GetType(type.FullName.Replace("[]", string.Empty));
                var array = obj as Array;
                var copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                    copied.SetValue(DeepCloneSerializeField(array.GetValue(i)), i);
                return Convert.ChangeType(copied, obj.GetType());
            }
            else if (type.IsClass)
            {
                object returnObj;
                var listObj = obj as IList;
                if (listObj != null)
                {
                    var properties = type.GetProperties();
                    var customList = typeof(List<>).MakeGenericType((properties[properties.Length - 1]).PropertyType);
                    returnObj = (IList) Activator.CreateInstance(customList);
                    var list = (IList) returnObj;
                    foreach (var item in ((IList) obj))
                    {
                        if (item == null)
                            continue;
                        list.Add(DeepCloneSerializeField(item));
                    }
                }
                else
                {
                    try
                    {
                        returnObj = Activator.CreateInstance(type);
                    }
                    catch
                    {
                        return null;
                    }
                    var fileds = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    for (int i = 0; i < fileds.Length; i++)
                    {
                        var field = fileds[i];
                        if (!field.IsDefined(typeof(SerializeField), false))
                            continue;
                        var filedValue = field.GetValue(obj);
                        if (filedValue == null)
                        {
                            field.SetValue(returnObj, filedValue);
                        }
                        else
                        {
                            field.SetValue(returnObj, DeepCloneSerializeField(filedValue));
                        }
                    }
                }
                return returnObj;
            }
            else
            {
                throw new ArgumentException("DeepCloneSerializeField: Unknown type:" + type + "," + obj);
            }
        }
    }
}