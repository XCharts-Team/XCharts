using System;

namespace XCharts.Runtime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CoordOptionsAttribute : Attribute
    {
        public readonly Type type0;
        public readonly Type type1;
        public readonly Type type2;
        public readonly Type type3;

        public CoordOptionsAttribute(Type coord)
        {
            type0 = coord;
        }
        public CoordOptionsAttribute(Type coord, Type coord2)
        {
            type0 = coord;
            type1 = coord2;
        }
        public CoordOptionsAttribute(Type coord, Type coord2, Type coord3)
        {
            type0 = coord;
            type1 = coord2;
            type2 = coord3;
        }
        public CoordOptionsAttribute(Type coord, Type coord2, Type coord3, Type coord4)
        {
            type0 = coord;
            type1 = coord2;
            type2 = coord3;
            type3 = coord4;
        }

        public bool Contains<T>() where T : CoordSystem
        {
            var type = typeof(T);
            return (type == type0 || type == type1 || type == type2 || type == type3);
        }
    }
}