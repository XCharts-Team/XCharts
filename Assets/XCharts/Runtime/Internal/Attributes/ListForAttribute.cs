/******************************************/
/*                                        */
/*     Copyright (c) 2021 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/
using System;

namespace XCharts
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ListFor : Attribute
    {
        public readonly Type type;

        public ListFor(Type type)
        {
            this.type = type;
        }
    }
}