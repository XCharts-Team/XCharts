/******************************************/
/*                                        */
/*     Copyright (c) 2021 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/
using System;

namespace XCharts
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class SerieConvertAttribute : Attribute
    {
        public readonly Type type0;
        public readonly Type type1;
        public readonly Type type2;
        public readonly Type type3;

        public SerieConvertAttribute(Type serie)
        {
            type0 = serie;
        }
        public SerieConvertAttribute(Type serie, Type serie2)
        {
            type0 = serie;
            type1 = serie2;
        }
        public SerieConvertAttribute(Type serie, Type serie2, Type serie3)
        {
            type0 = serie;
            type1 = serie2;
            type2 = serie3;
        }
        public SerieConvertAttribute(Type serie, Type serie2, Type serie3, Type serie4)
        {
            type0 = serie;
            type1 = serie2;
            type2 = serie3;
            type3 = serie4;
        }

        public bool Contains<T>() where T : CoordSystem
        {
            var type = typeof(T);
            return (type == type0 || type == type1 || type == type2 || type == type3);
        }
    }
}