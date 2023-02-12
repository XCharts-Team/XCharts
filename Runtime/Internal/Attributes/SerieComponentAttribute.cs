using System;
using System.Collections.Generic;

namespace XCharts.Runtime
{
    /// <summary>
    /// The attribute for serie component.
    /// |可添加到Serie的组件。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class SerieComponentAttribute : Attribute
    {
        public readonly List<Type> types = new List<Type>();

        public SerieComponentAttribute()
        { }
        public SerieComponentAttribute(Type type1)
        {
            AddType(type1);
        }
        public SerieComponentAttribute(Type type1, Type type2)
        {
            AddType(type1);
            AddType(type2);
        }
        public SerieComponentAttribute(Type type1, Type type2, Type type3)
        {
            AddType(type1);
            AddType(type2);
            AddType(type3);
        }
        public SerieComponentAttribute(Type type1, Type type2, Type type3, Type type4)
        {
            AddType(type1);
            AddType(type2);
            AddType(type3);
            AddType(type4);
        }
        public SerieComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5)
        {
            AddType(type1);
            AddType(type2);
            AddType(type3);
            AddType(type4);
            AddType(type5);
        }
        public SerieComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6)
        {
            AddType(type1);
            AddType(type2);
            AddType(type3);
            AddType(type4);
            AddType(type5);
            AddType(type6);
        }
        public SerieComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7)
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
            if (!Serie.extraComponentMap.ContainsKey(type))
                throw new ArgumentException("Serie not support extra component:" + type);
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