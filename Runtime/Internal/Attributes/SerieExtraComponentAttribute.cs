using System;
using System.Collections.Generic;

namespace XCharts.Runtime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class SerieExtraComponentAttribute : Attribute
    {
        public readonly List<Type> types = new List<Type>();

        public SerieExtraComponentAttribute()
        {
        }
        public SerieExtraComponentAttribute(Type type1)
        {
            types.Add(type1);
        }
        public SerieExtraComponentAttribute(Type type1, Type type2)
        {
            types.Add(type1);
            types.Add(type2);
        }
        public SerieExtraComponentAttribute(Type type1, Type type2, Type type3)
        {
            types.Add(type1);
            types.Add(type2);
            types.Add(type3);
        }
        public SerieExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4)
        {
            types.Add(type1);
            types.Add(type2);
            types.Add(type3);
            types.Add(type4);
        }
        public SerieExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5)
        {
            types.Add(type1);
            types.Add(type2);
            types.Add(type3);
            types.Add(type4);
            types.Add(type5);
        }
        public SerieExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6)
        {
            types.Add(type1);
            types.Add(type2);
            types.Add(type3);
            types.Add(type4);
            types.Add(type5);
            types.Add(type6);
        }
        public SerieExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7)
        {
            types.Add(type1);
            types.Add(type2);
            types.Add(type3);
            types.Add(type4);
            types.Add(type5);
            types.Add(type6);
            types.Add(type7);
        }

        public bool Contains<T>() where T : ISerieExtraComponent
        {
            return Contains(typeof(T));
        }

        public bool Contains(Type type)
        {
            return types.Contains(type);
        }
    }
}