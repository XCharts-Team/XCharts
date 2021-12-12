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
    public sealed class DefaultAnimationAttribute : Attribute
    {
        public readonly AnimationType type;

        public DefaultAnimationAttribute(AnimationType handler)
        {
            this.type = handler;
        }
    }
}