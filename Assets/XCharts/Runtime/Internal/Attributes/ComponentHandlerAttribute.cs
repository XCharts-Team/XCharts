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
    public sealed class ComponentHandlerAttribute : Attribute
    {
        public readonly Type handler;
        public readonly bool allowMultiple = true;

        public ComponentHandlerAttribute(Type handler)
        {
            this.handler = handler;
            this.allowMultiple = true;
        }

        public ComponentHandlerAttribute(Type handler, bool allowMultiple)
        {
            this.handler = handler;
            this.allowMultiple = allowMultiple;
        }
    }
}