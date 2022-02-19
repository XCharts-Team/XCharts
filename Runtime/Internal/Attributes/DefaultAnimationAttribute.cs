using System;

namespace XCharts.Runtime
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