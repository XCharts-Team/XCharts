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
    public sealed class ListForComponent : ListFor
    {
        public ListForComponent(Type type) : base(type)
        {
        }
    }
}