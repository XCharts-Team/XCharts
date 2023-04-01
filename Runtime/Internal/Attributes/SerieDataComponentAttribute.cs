using System;
using System.Collections.Generic;

namespace XCharts.Runtime
{
    /// <summary>
    /// The attribute for serie data component.
    /// |可添加到SerieData的组件。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class SerieDataComponentAttribute : Attribute
    {
        public readonly List<Type> types = new List<Type>();

        public SerieDataComponentAttribute()
        {
        }
        public SerieDataComponentAttribute(Type type1)
        {
            AddType(type1);
        }
        public SerieDataComponentAttribute(Type type1, Type type2)
        {
            AddType(type1);
            AddType(type2);
        }
        public SerieDataComponentAttribute(Type type1, Type type2, Type type3)
        {
            AddType(type1);
            AddType(type2);
            AddType(type3);
        }
        public SerieDataComponentAttribute(Type type1, Type type2, Type type3, Type type4)
        {
            AddType(type1);
            AddType(type2);
            AddType(type3);
            AddType(type4);
        }
        public SerieDataComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5)
        {
            AddType(type1);
            AddType(type2);
            AddType(type3);
            AddType(type4);
            AddType(type5);
        }
        public SerieDataComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6)
        {
            AddType(type1);
            AddType(type2);
            AddType(type3);
            AddType(type4);
            AddType(type5);
            AddType(type6);
        }
        public SerieDataComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7)
        {
            AddType(type1);
            AddType(type2);
            AddType(type3);
            AddType(type4);
            AddType(type5);
            AddType(type6);
            AddType(type7);
        }

        private void AddType(Type type)
        {
            if (!SerieData.extraComponentMap.ContainsKey(type))
                throw new ArgumentException("SerieData not support extra component:" + type);
            types.Add(type);
        }

        public bool Contains<T>() where T : ISerieComponent
        {
            return Contains(typeof(T));
        }

        public bool Contains(Type type)
        {
            return types.Contains(type);
        }
    }
}