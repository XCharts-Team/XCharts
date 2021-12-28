
using System;
using System.Reflection;

namespace XCharts
{
    public static class ReflectionUtil
    {
        public static void InvokeListClear(object obj, FieldInfo field)
        {
            var list = field.GetValue(obj);
            var method = list.GetType().GetMethod("Clear");
            method.Invoke(list, new object[] { });
        }
        public static int InvokeListCount(object obj, FieldInfo field)
        {
            var list = field.GetValue(obj);
            return (int)list.GetType().GetProperty("Count").GetValue(list);
        }

        public static void InvokeListAdd(object obj, FieldInfo field, object item)
        {
            var list = field.GetValue(obj);
            var method = list.GetType().GetMethod("Add");
            method.Invoke(list, new object[] { item });
        }

        public static T InvokeListGet<T>(object obj, FieldInfo field, int i)
        {
            var list = field.GetValue(obj);
            var item = list.GetType().GetProperty("Item").GetValue(list, new object[] { i });
            return (T)item;
        }


        public static void InvokeListAddTo<T>(object obj, FieldInfo field, Action<T> callback)
        {
            var list = field.GetValue(obj);
            var listType = list.GetType();
            var count = Convert.ToInt32(listType.GetProperty("Count").GetValue(list));
            for (int i = 0; i < count; i++)
            {
                var item = listType.GetProperty("Item").GetValue(list, new object[] { i });
                callback((T)item);
            }
        }
    }
}