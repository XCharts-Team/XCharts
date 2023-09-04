using System;

namespace XCharts.Runtime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DefaultAnimationAttribute : Attribute
    {
        public readonly AnimationType type;
        public readonly bool enableSerieDataAddedAnimation = true;

        public DefaultAnimationAttribute(AnimationType handler)
        {
            this.type = handler;
        }

        public DefaultAnimationAttribute(AnimationType handler, bool enableSerieDataAddedAnimation)
        {
            this.type = handler;
            this.enableSerieDataAddedAnimation = enableSerieDataAddedAnimation;
        }
    }
}